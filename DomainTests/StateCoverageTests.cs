using Domain.Pipelines;
using Domain.Reports;
using Domain.Sprints;
using Moq;
using System;
using System.Collections.Generic;

namespace DomainTests
{
    /// <summary>
    /// Aanvullende dekking voor de uitzonderingspaden van het State-pattern (geweigerde
    /// transities en guards) plus de Factory- en stub-randgevallen. Hoort bij NF1/NF2:
    /// elke guard en elke ongeldige transitie heeft een eigen test (branch coverage).
    /// </summary>
    public class StateCoverageTests
    {
        // ---- Helpers ----------------------------------------------------------
        private static (Project project, ReviewSprint sprint, Developer dev, Developer scrum, Mock<INotificatorService> scrumService) NewSprint()
        {
            var scrumService = new Mock<INotificatorService>();
            var scrum = TestHelpers.CreateDeveloper("Scrum", Role.LeadDeveloper, scrumService.Object);
            var dev = TestHelpers.CreateDeveloper("Hans", Role.Developer);

            var project = new Project(scrum, "Project 1");
            var sprint = new ReviewSprint(project, "Sprint 1", DateTime.Today, DateTime.Today.AddDays(14), scrum, new List<Developer> { dev, scrum });
            project.AddSprint(sprint);
            return (project, sprint, dev, scrum, scrumService);
        }

        private static BacklogItem AddItem(Project project, ReviewSprint sprint, Developer dev)
        {
            var item = new BacklogItem("Item", "Desc", 1, project.Backlog);
            sprint.AddToSprintBacklog(item);
            item.AssignDeveloper(dev);
            return item;
        }

        // ---- BacklogItemState: bewerkingen zijn toegestaan in 'todo' ----------
        [Fact]
        public void Todo_State_Allows_Editing_The_Item()
        {
            var (project, sprint, dev, _, _) = NewSprint();
            var item = AddItem(project, sprint, dev);
            var activity = new Activity("A1");

            item.State.SetName("New name");
            item.State.SetDescription("New desc");
            item.State.SetEffort(8);
            item.State.AddActivity(activity);
            item.State.RemoveActivity(activity);

            Assert.Equal("New name", item.Name);
            Assert.Equal("New desc", item.Description);
            Assert.Equal(8, item.Effort);
            Assert.DoesNotContain(activity, item.Activities);
        }

        // ---- BacklogItemState: stappen overslaan wordt geweigerd --------------
        [Fact]
        public void Todo_State_Refuses_Transitions_That_Skip_Steps()
        {
            var (project, sprint, dev, _, _) = NewSprint();
            var item = AddItem(project, sprint, dev);

            Assert.Throws<InvalidOperationException>(() => item.State.SubmitForTesting());
            Assert.Throws<InvalidOperationException>(() => item.State.StartTesting());
            Assert.Throws<InvalidOperationException>(() => item.State.CompleteTesting());
            Assert.Throws<InvalidOperationException>(() => item.State.Approve());
            Assert.Throws<InvalidOperationException>(() => item.State.RejectByLeadDeveloper());
            Assert.Equal(EBacklogStates.todo, item.StateType);
        }

        // Eenmaal uit 'todo' kan het werk niet opnieuw gestart worden.
        [Fact]
        public void BeginWork_Is_Refused_Once_The_Item_Left_Todo()
        {
            var (project, sprint, dev, _, _) = NewSprint();
            var item = AddItem(project, sprint, dev);

            item.State.BeginWork();

            Assert.Throws<InvalidOperationException>(() => item.State.BeginWork());
        }

        // ---- DoneState: eindfase weigert elke bewerking -----------------------
        [Fact]
        public void Done_State_Refuses_All_Edits()
        {
            var (project, sprint, dev, _, _) = NewSprint();
            var item = AddItem(project, sprint, dev);

            item.State.BeginWork();
            item.State.SubmitForTesting();
            item.State.StartTesting();
            item.State.CompleteTesting();
            item.State.Approve();

            Assert.Equal(EBacklogStates.done, item.StateType);
            Assert.Throws<InvalidOperationException>(() => item.State.AddActivity(new Activity("x")));
            Assert.Throws<InvalidOperationException>(() => item.State.RemoveActivity(new Activity("x")));
            Assert.Throws<InvalidOperationException>(() => item.State.SetDescription("x"));
            Assert.Throws<InvalidOperationException>(() => item.State.SetEffort(5));
            Assert.Throws<InvalidOperationException>(() => item.State.SetName("x"));
        }

        // ---- ReadyForTestingState ---------------------------------------------
        [Fact]
        public void ReadyForTesting_State_Refuses_Adding_Activities()
        {
            var (project, sprint, dev, _, _) = NewSprint();
            var item = AddItem(project, sprint, dev);

            item.State.BeginWork();
            item.State.SubmitForTesting();

            Assert.Equal(EBacklogStates.readyfortesting, item.StateType);
            Assert.Throws<InvalidOperationException>(() => item.State.AddActivity(new Activity("x")));
        }

        // Terugpad: tester wijst af vanuit 'ready for testing' -> terug naar todo + bericht.
        [Fact]
        public void Tester_Can_Reject_From_ReadyForTesting_Back_To_Todo()
        {
            var (project, sprint, dev, scrum, scrumService) = NewSprint();
            var item = AddItem(project, sprint, dev);

            item.State.BeginWork();
            item.State.SubmitForTesting();
            item.State.RejectByTester();

            Assert.Equal(EBacklogStates.todo, item.StateType);
            scrumService.Verify(s => s.SendNotification(It.IsAny<string>(), scrum), Times.AtLeastOnce);
        }

        // ---- TestingState ------------------------------------------------------
        [Fact]
        public void Testing_State_Refuses_Activity_Changes()
        {
            var (project, sprint, dev, _, _) = NewSprint();
            var item = AddItem(project, sprint, dev);

            item.State.BeginWork();
            item.State.SubmitForTesting();
            item.State.StartTesting();

            Assert.Equal(EBacklogStates.testing, item.StateType);
            Assert.Throws<InvalidOperationException>(() => item.State.AddActivity(new Activity("x")));
            Assert.Throws<InvalidOperationException>(() => item.State.RemoveActivity(new Activity("x")));
        }

        // Guard: een activiteit die na het testen weer 'open' staat, blokkeert 'tested'.
        [Fact]
        public void Testing_State_Cannot_Complete_When_An_Activity_Is_Not_Done()
        {
            var (project, sprint, dev, _, _) = NewSprint();
            var item = AddItem(project, sprint, dev);
            var activity = new Activity("A1");
            item.AddActivity(activity);
            activity.NextStatus(); // Todo -> Active
            activity.NextStatus(); // Active -> Done

            item.State.BeginWork();
            item.State.SubmitForTesting();
            item.State.StartTesting();
            activity.PreviousStatus(); // Done -> Active (niet meer afgerond)

            Assert.Throws<InvalidOperationException>(() => item.State.CompleteTesting());
        }

        // ---- TestedState -------------------------------------------------------
        // Guard: DoD niet gehaald (een activiteit is niet done) -> 'approve' geweigerd.
        [Fact]
        public void Tested_State_Cannot_Approve_When_An_Activity_Is_Not_Done()
        {
            var (project, sprint, dev, _, _) = NewSprint();
            var item = AddItem(project, sprint, dev);

            item.State.BeginWork();
            item.State.SubmitForTesting();
            item.State.StartTesting();
            item.State.CompleteTesting();

            // In 'tested' is bewerken nog toegestaan; voeg een nog open activiteit toe.
            item.State.AddActivity(new Activity("late task"));

            Assert.Equal(EBacklogStates.tested, item.StateType);
            Assert.Throws<InvalidOperationException>(() => item.State.Approve());
        }

        // ---- SprintState: configuratie mag alleen in 'scheduled' --------------
        [Fact]
        public void Sprint_Cannot_Be_Configured_Once_It_Is_In_Progress()
        {
            var (project, sprint, dev, _, _) = NewSprint();
            sprint.Start();

            Assert.Equal(ESprintStates.InProgress, sprint.State.GetSprintState());
            Assert.Throws<InvalidOperationException>(() => sprint.State.SetName("x"));
            Assert.Throws<InvalidOperationException>(() => sprint.State.SetStartDate(DateTime.Today));
            Assert.Throws<InvalidOperationException>(() => sprint.State.SetEndDate(DateTime.Today));
            Assert.Throws<InvalidOperationException>(() => sprint.State.AddDeveloper(dev));
            Assert.Throws<InvalidOperationException>(() => sprint.State.AddToSprintBacklog(new BacklogItem("x", "y", 1, project.Backlog)));
            Assert.Throws<InvalidOperationException>(() => sprint.State.Start());
        }

        // Cancel/Retry horen alleen bij een afgeronde (release-)sprint.
        [Fact]
        public void Cancel_And_Retry_Are_Refused_Outside_The_Finished_State()
        {
            var (_, sprint, _, _, _) = NewSprint();

            Assert.Throws<InvalidOperationException>(() => sprint.State.Cancel());
            Assert.Throws<InvalidOperationException>(() => sprint.State.Retry());
        }

        [Fact]
        public void Sprint_Exposes_Its_Configured_Dates()
        {
            var (_, sprint, _, _, _) = NewSprint();

            Assert.Equal(DateTime.Today, sprint.StartDate);
            Assert.Equal(DateTime.Today.AddDays(14), sprint.EndDate);
        }

        // Een review-sprint mag optioneel ook een pipeline draaien bij het afronden.
        [Fact]
        public void A_Review_Sprint_Runs_Its_Optional_Pipeline_When_Finished()
        {
            var (_, sprint, _, _, _) = NewSprint();
            var command = new Mock<PipelineJobCommand>("Analyze", "cmd");
            command.Setup(c => c.Execute());
            var pipeline = new Pipeline(new List<PipelineJobCommand> { command.Object }, "review-pipeline");
            sprint.SetPipeline(pipeline);

            sprint.Start();
            sprint.Finish();

            command.Verify(c => c.Execute(), Times.Once);
        }

        // ---- Factory: onbekend rapporttype -------------------------------------
        [Fact]
        public void Report_Factory_Rejects_An_Unknown_Report_Type()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => ReportBuilderFactory.GetBuilder((ReportType)999));
        }

        // ---- Externe stubs (infrastructure ring) -------------------------------
        [Fact]
        public void External_Stub_Clients_Can_Be_Invoked_Directly()
        {
            var smtp = new SmtpServer();
            var slack = new SlackApiClient();

            var smtpError = Record.Exception(() => smtp.SendMail("dev@avans-devops.local", "subject", "body"));
            var slackError = Record.Exception(() => slack.PostMessage("@dev", "message"));

            Assert.Null(smtpError);
            Assert.Null(slackError);
        }
    }
}
