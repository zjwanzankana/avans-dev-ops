using Domain.Backlogs.BacklogItemStates;
using Domain.Sprints;
using Moq;
using System;
using System.Collections.Generic;

namespace DomainTests
{
    /// <summary>
    /// FR_B3 - backlog item fases (todo, doing, ready for testing, testing, tested, done).
    /// De testen volgen de basispad- en uitzonderingspaden uit de toestandsdiagram (STD).
    /// </summary>
    public class BackLogStateTests
    {
        private static (Project project, ReviewSprint sprint, Developer dev) NewSprintWithDeveloper()
        {
            var productOwner = TestHelpers.CreateDeveloper("John", Role.Developer);
            var developer = TestHelpers.CreateDeveloper("Hans", Role.Developer);
            var tester = TestHelpers.CreateDeveloper("Hans2", Role.Tester);
            var developers = new List<Developer> { developer, tester, productOwner };

            var project = new Project(productOwner, "Project 1");
            var sprint = new ReviewSprint(project, "Sprint 1", DateTime.Now, DateTime.Now.AddDays(14), productOwner, developers);
            project.AddSprint(sprint);
            return (project, sprint, developer);
        }

        private static BacklogItem AddItem(Project project, ReviewSprint sprint, Developer dev, bool assign = true)
        {
            var item = new BacklogItem("BacklogItem 1", "Description 1", 1, project.Backlog);
            sprint.AddToSprintBacklog(item);
            if (assign)
            {
                item.AssignDeveloper(dev);
            }
            return item;
        }

        // Een backlog item krijgt status 'todo' wanneer het aan een sprint wordt toegevoegd.
        [Fact]
        public void A_BacklogItem_Gets_Status_Todo_When_Added_To_A_Sprint()
        {
            var (project, sprint, dev) = NewSprintWithDeveloper();
            var item = AddItem(project, sprint, dev, assign: false);

            Assert.Equal(EBacklogStates.todo, item.StateType);
        }

        // Vanaf 'todo' bestaat geen terugpad: een ongeldige transitie wordt geweigerd.
        [Fact]
        public void A_BacklogItem_Cannot_Do_A_Backward_Transition_From_Todo()
        {
            var (project, sprint, dev) = NewSprintWithDeveloper();
            var item = AddItem(project, sprint, dev);

            Assert.Equal(EBacklogStates.todo, item.StateType);
            Assert.Throws<InvalidOperationException>(() => item.State.RejectByTester());
            Assert.Throws<InvalidOperationException>(() => item.State.Reopen());
        }

        // Een item kan alleen van fase wisselen als het in een sprint zit.
        [Fact]
        public void A_BacklogItem_Cannot_Change_State_When_Not_Added_To_Sprint()
        {
            var productOwner = TestHelpers.CreateDeveloper("John", Role.Developer);
            var developer = TestHelpers.CreateDeveloper("Hans", Role.Developer);
            var project = new Project(productOwner, "Project 1");
            var item = new BacklogItem("BacklogItem 1", "Description 1", 1, project.Backlog);
            item.AssignDeveloper(developer);

            Assert.Throws<InvalidOperationException>(() => item.State.BeginWork());
        }

        // Basispad-stap: todo -> doing.
        [Fact]
        public void A_BacklogItem_Can_Change_State_From_Todo_To_Doing()
        {
            var (project, sprint, dev) = NewSprintWithDeveloper();
            var item = AddItem(project, sprint, dev);

            item.State.BeginWork();

            Assert.Equal(EBacklogStates.doing, item.StateType);
        }

        // Zonder toegewezen developer kan een item niet naar 'doing'.
        [Fact]
        public void A_BacklogItem_Cannot_Start_Work_When_No_Developer_Assigned()
        {
            var (project, sprint, dev) = NewSprintWithDeveloper();
            var item = AddItem(project, sprint, dev, assign: false);

            Assert.Throws<InvalidOperationException>(() => item.State.BeginWork());
        }

        // Basispad-stap: doing -> ready for testing.
        [Fact]
        public void A_BacklogItem_Can_Change_State_From_Doing_To_ReadyForTesting()
        {
            var (project, sprint, dev) = NewSprintWithDeveloper();
            var item = AddItem(project, sprint, dev);

            item.State.BeginWork();
            item.State.SubmitForTesting();

            Assert.Equal(EBacklogStates.readyfortesting, item.StateType);
        }

        // Guard: niet alle activiteiten done -> blijft in 'doing'.
        [Fact]
        public void A_BacklogItem_Cannot_Submit_For_Testing_When_Activities_Not_Done()
        {
            var (project, sprint, dev) = NewSprintWithDeveloper();
            var item = AddItem(project, sprint, dev);
            item.AddActivity(new Activity("Activity 1"));

            item.State.BeginWork();

            Assert.Throws<InvalidOperationException>(() => item.State.SubmitForTesting());
        }

        // Basispad-stap: ready for testing -> testing.
        [Fact]
        public void A_BacklogItem_Can_Change_State_From_ReadyForTesting_To_Testing()
        {
            var (project, sprint, dev) = NewSprintWithDeveloper();
            var item = AddItem(project, sprint, dev);

            item.State.BeginWork();
            item.State.SubmitForTesting();
            item.State.StartTesting();

            Assert.Equal(EBacklogStates.testing, item.StateType);
        }

        // Basispad-stap: testing -> tested.
        [Fact]
        public void A_BacklogItem_Can_Change_State_From_Testing_To_Tested()
        {
            var (project, sprint, dev) = NewSprintWithDeveloper();
            var item = AddItem(project, sprint, dev);

            item.State.BeginWork();
            item.State.SubmitForTesting();
            item.State.StartTesting();
            item.State.CompleteTesting();

            Assert.Equal(EBacklogStates.tested, item.StateType);
        }

        // Basispad-stap: tested -> done (Definition of Done akkoord).
        [Fact]
        public void A_BacklogItem_Can_Change_State_From_Tested_To_Done()
        {
            var (project, sprint, dev) = NewSprintWithDeveloper();
            var item = AddItem(project, sprint, dev);

            item.State.BeginWork();
            item.State.SubmitForTesting();
            item.State.StartTesting();
            item.State.CompleteTesting();
            item.State.Approve();

            Assert.Equal(EBacklogStates.done, item.StateType);
        }

        // Terugpad: tester vindt een defect tijdens testing -> terug naar todo + bericht scrum master.
        [Fact]
        public void A_BacklogItem_Goes_Back_To_Todo_When_Tester_Rejects()
        {
            var productOwner = TestHelpers.CreateDeveloper("John", Role.Developer);
            var scrumMasterService = new Mock<INotificatorService>();
            var scrumMaster = TestHelpers.CreateDeveloper("Scrum", Role.LeadDeveloper, scrumMasterService.Object);
            var developer = TestHelpers.CreateDeveloper("Hans", Role.Developer);

            var project = new Project(productOwner, "Project 1");
            var sprint = new ReviewSprint(project, "Sprint 1", DateTime.Now, DateTime.Now.AddDays(14), scrumMaster, new List<Developer> { developer });
            project.AddSprint(sprint);

            var item = new BacklogItem("BacklogItem 1", "Description 1", 1, project.Backlog);
            sprint.AddToSprintBacklog(item);
            item.AssignDeveloper(developer);

            item.State.BeginWork();
            item.State.SubmitForTesting();
            item.State.StartTesting();
            item.State.RejectByTester();

            Assert.Equal(EBacklogStates.todo, item.StateType);
            scrumMasterService.Verify(s => s.SendNotification(It.IsAny<string>(), scrumMaster), Times.AtLeastOnce);
        }

        // Terugpad: lead developer keurt DoD af -> tested terug naar ready for testing.
        [Fact]
        public void A_BacklogItem_Goes_Back_To_ReadyForTesting_When_DoD_Fails()
        {
            var (project, sprint, dev) = NewSprintWithDeveloper();
            var item = AddItem(project, sprint, dev);

            item.State.BeginWork();
            item.State.SubmitForTesting();
            item.State.StartTesting();
            item.State.CompleteTesting();
            item.State.RejectByLeadDeveloper();

            Assert.Equal(EBacklogStates.readyfortesting, item.StateType);
        }

        // 'done' is geen dead-end: een item kan heropend worden (-> todo).
        [Fact]
        public void A_BacklogItem_Can_Be_Reopened_From_Done()
        {
            var (project, sprint, dev) = NewSprintWithDeveloper();
            var item = AddItem(project, sprint, dev);

            item.State.BeginWork();
            item.State.SubmitForTesting();
            item.State.StartTesting();
            item.State.CompleteTesting();
            item.State.Approve();
            item.State.Reopen();

            Assert.Equal(EBacklogStates.todo, item.StateType);
        }

        // Notificatie: testers worden geinformeerd zodra een item 'ready for testing' is.
        [Fact]
        public void Testers_Are_Notified_When_Item_Is_Ready_For_Testing()
        {
            var productOwner = TestHelpers.CreateDeveloper("John", Role.Developer);
            var developer = TestHelpers.CreateDeveloper("Hans", Role.Developer);
            var testerService = new Mock<INotificatorService>();
            var tester = TestHelpers.CreateDeveloper("Tess", Role.Tester, testerService.Object);

            var project = new Project(productOwner, "Project 1");
            project.AddTester(tester);
            var sprint = new ReviewSprint(project, "Sprint 1", DateTime.Now, DateTime.Now.AddDays(14), productOwner, new List<Developer> { developer });
            project.AddSprint(sprint);

            var item = new BacklogItem("BacklogItem 1", "Description 1", 1, project.Backlog);
            sprint.AddToSprintBacklog(item);
            item.AssignDeveloper(developer);

            item.State.BeginWork();
            item.State.SubmitForTesting();

            testerService.Verify(s => s.SendNotification(It.IsAny<string>(), tester), Times.Once);
        }
    }
}
