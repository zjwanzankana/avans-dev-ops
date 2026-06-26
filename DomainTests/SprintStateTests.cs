using Domain.Pipelines;
using Domain.Sprints;
using Domain.Sprints.SprintStates;
using Moq;
using System;
using System.Collections.Generic;

namespace DomainTests
{
    /// <summary>FR_S3 / FR_S4 - sprint levenscyclus (Scheduled -> InProgress -> Finished -> Closed/Cancelled).</summary>
    public class SprintStateTests
    {
        [Fact]
        public void A_Scheduled_Sprint_Allows_Configuration_And_Backlog_Additions()
        {
            var productOwner = TestHelpers.CreateDeveloper("John", Role.Developer);
            var project = new Project(productOwner, "Project 1");
            var sprint = new ReviewSprint(project, "Sprint 1", DateTime.Today, DateTime.Today.AddDays(7), productOwner, new List<Developer>());
            var backlogItem = new BacklogItem("task1", "desc", 3, project.Backlog);

            sprint.State.SetName("Sprint Updated");
            sprint.State.SetStartDate(DateTime.Today.AddDays(1));
            sprint.State.SetEndDate(DateTime.Today.AddDays(10));
            sprint.State.AddDeveloper(productOwner);
            sprint.State.AddToSprintBacklog(backlogItem);

            Assert.Equal("Sprint Updated", sprint.Name);
            Assert.Contains(backlogItem, sprint.BacklogItems);
            Assert.Contains(productOwner, sprint.Developers);
            Assert.Equal(ESprintStates.Scheduled, sprint.State.GetSprintState());
        }

        [Fact]
        public void A_Scheduled_Sprint_Refuses_Finish_And_Close()
        {
            var productOwner = TestHelpers.CreateDeveloper("John", Role.Developer);
            var project = new Project(productOwner, "Project 1");
            var sprint = new ReviewSprint(project, "Sprint 1", DateTime.Today, DateTime.Today.AddDays(7), productOwner, new List<Developer>());

            Assert.Throws<InvalidOperationException>(() => sprint.State.Finish());
            Assert.Throws<InvalidOperationException>(() => sprint.State.Close());
        }

        [Fact]
        public void A_Sprint_Can_Move_From_Scheduled_To_InProgress_To_Finished()
        {
            var productOwner = TestHelpers.CreateDeveloper("John", Role.Developer);
            var project = new Project(productOwner, "Project 1");
            var sprint = new ReviewSprint(project, "Sprint 1", DateTime.Today, DateTime.Today.AddDays(7), productOwner, new List<Developer>());

            sprint.Start();
            Assert.Equal(ESprintStates.InProgress, sprint.State.GetSprintState());

            sprint.Finish();
            Assert.Equal(ESprintStates.Finished, sprint.State.GetSprintState());
        }

        [Fact]
        public void A_Release_Sprint_Runs_Its_Pipeline_When_Finished()
        {
            var productOwner = TestHelpers.CreateDeveloper("John", Role.Developer);
            var project = new Project(productOwner, "Project 1");
            var pipelineCommand = CreateCommand(PipelineJobStatus.FINISHED, "Analyze done");
            var pipeline = new Pipeline(new List<PipelineJobCommand> { pipelineCommand.Object }, "release");
            var sprint = new ReleaseSprint(project, "Release 1", DateTime.Today, DateTime.Today.AddDays(7), productOwner, new List<Developer> { productOwner }, pipeline);

            sprint.Start();
            sprint.Finish();

            Assert.Equal(ESprintStates.Finished, sprint.State.GetSprintState());
            Assert.Equal(PipelineJobStatus.FINISHED, pipeline.Status);
            pipelineCommand.Verify(c => c.Execute(), Times.Once);
        }

        [Fact]
        public void A_Release_Sprint_Can_Be_Closed_When_Pipeline_Succeeds()
        {
            var productOwner = TestHelpers.CreateDeveloper("John", Role.Developer);
            var project = new Project(productOwner, "Project 1");
            var pipeline = new Pipeline(new List<PipelineJobCommand> { CreateCommand(PipelineJobStatus.FINISHED, "ok").Object }, "release");
            var sprint = new ReleaseSprint(project, "Release 1", DateTime.Today, DateTime.Today.AddDays(7), productOwner, new List<Developer> { productOwner }, pipeline);

            sprint.Start();
            sprint.Finish();
            sprint.Close();

            Assert.Equal(ESprintStates.Closed, sprint.State.GetSprintState());
        }

        [Fact]
        public void A_Release_Sprint_Cannot_Be_Closed_When_Pipeline_Fails_But_Can_Be_Cancelled()
        {
            var productOwner = TestHelpers.CreateDeveloper("John", Role.Developer);
            var notify = new Mock<INotificatorService>();
            var owner = TestHelpers.CreateDeveloper("PO", Role.Developer, notify.Object);
            var project = new Project(owner, "Project 1");
            var failing = CreateCommand(PipelineJobStatus.FAILED, "boom");
            var pipeline = new Pipeline(new List<PipelineJobCommand> { failing.Object }, "release");
            var sprint = new ReleaseSprint(project, "Release 1", DateTime.Today, DateTime.Today.AddDays(7), owner, new List<Developer> { owner }, pipeline);

            sprint.Start();
            sprint.Finish();

            Assert.Equal(PipelineJobStatus.FAILED, pipeline.Status);
            Assert.Throws<InvalidOperationException>(() => sprint.Close());

            sprint.CancelRelease();
            Assert.Equal(ESprintStates.Cancelled, sprint.State.GetSprintState());
            notify.Verify(s => s.SendNotification(It.IsAny<string>(), owner), Times.AtLeastOnce);
        }

        [Fact]
        public void A_Failed_Release_Can_Be_Retried_And_Then_Closed()
        {
            var productOwner = TestHelpers.CreateDeveloper("John", Role.Developer);
            var project = new Project(productOwner, "Project 1");
            // Eerste run faalt, daarna 'herstelt' het commando en slaagt de retry.
            var command = new Mock<PipelineJobCommand>("Deploy", "cmd");
            var attempts = 0;
            command.Setup(c => c.Execute()).Callback(() =>
            {
                attempts++;
                SetStatus(command.Object, attempts == 1 ? PipelineJobStatus.FAILED : PipelineJobStatus.FINISHED);
                SetOutput(command.Object, "out");
            });
            var pipeline = new Pipeline(new List<PipelineJobCommand> { command.Object }, "release");
            var sprint = new ReleaseSprint(project, "Release 1", DateTime.Today, DateTime.Today.AddDays(7), productOwner, new List<Developer> { productOwner }, pipeline);

            sprint.Start();
            sprint.Finish();
            Assert.Equal(PipelineJobStatus.FAILED, pipeline.Status);

            sprint.RetryRelease();
            Assert.Equal(PipelineJobStatus.FINISHED, pipeline.Status);

            sprint.Close();
            Assert.Equal(ESprintStates.Closed, sprint.State.GetSprintState());
        }

        [Fact]
        public void A_Review_Sprint_Can_Only_Be_Closed_After_The_Scrum_Master_Uploads_A_Review()
        {
            var scrumMaster = TestHelpers.CreateDeveloper("Scrum", Role.LeadDeveloper);
            var project = new Project(scrumMaster, "Project 1");
            var sprint = new ReviewSprint(project, "Sprint 1", DateTime.Today, DateTime.Today.AddDays(7), scrumMaster, new List<Developer> { scrumMaster });

            sprint.Start();
            sprint.Finish();

            // Zonder geuploade review kan de sprint niet sluiten.
            Assert.Throws<InvalidOperationException>(() => sprint.Close());

            sprint.UploadReview(new Review("Looks good", scrumMaster, sprint));
            sprint.Close();

            Assert.True(sprint.IsReviewDone);
            Assert.Equal(ESprintStates.Closed, sprint.State.GetSprintState());
        }

        [Fact]
        public void A_Review_Cannot_Be_Uploaded_Before_The_Sprint_Is_Finished()
        {
            var scrumMaster = TestHelpers.CreateDeveloper("Scrum", Role.LeadDeveloper);
            var project = new Project(scrumMaster, "Project 1");
            var sprint = new ReviewSprint(project, "Sprint 1", DateTime.Today, DateTime.Today.AddDays(7), scrumMaster, new List<Developer> { scrumMaster });

            sprint.Start();

            Assert.Throws<InvalidOperationException>(() => sprint.UploadReview(new Review("x", scrumMaster, sprint)));
        }

        [Fact]
        public void Only_The_Scrum_Master_Can_Upload_A_Review()
        {
            var scrumMaster = TestHelpers.CreateDeveloper("Scrum", Role.LeadDeveloper);
            var someoneElse = TestHelpers.CreateDeveloper("Dev", Role.Developer);
            var project = new Project(scrumMaster, "Project 1");
            var sprint = new ReviewSprint(project, "Sprint 1", DateTime.Today, DateTime.Today.AddDays(7), scrumMaster, new List<Developer> { scrumMaster });

            sprint.Start();
            sprint.Finish();

            Assert.Throws<InvalidOperationException>(() => sprint.UploadReview(new Review("x", someoneElse, sprint)));
        }

        [Fact]
        public void Sprint_Dates_And_Name_Cannot_Change_When_Pipeline_Is_Running()
        {
            var productOwner = TestHelpers.CreateDeveloper("John", Role.Developer);
            var project = new Project(productOwner, "Project 1");
            var sprint = new ReviewSprint(project, "Sprint 1", DateTime.Today, DateTime.Today.AddDays(7), productOwner, new List<Developer> { productOwner });
            var pipeline = new Pipeline(new List<PipelineJobCommand> { CreateCommand(PipelineJobStatus.FINISHED, "ok").Object }, "pipeline");

            pipeline.SetStatus(PipelineJobStatus.Running);
            sprint.SetPipeline(pipeline);

            Assert.Throws<InvalidOperationException>(() => sprint.SetName("Blocked"));
            Assert.Throws<InvalidOperationException>(() => sprint.SetStartDate(DateTime.Today.AddDays(1)));
            Assert.Throws<InvalidOperationException>(() => sprint.SetEndDate(DateTime.Today.AddDays(10)));
        }

        private static Mock<PipelineJobCommand> CreateCommand(PipelineJobStatus status, string output)
        {
            var mock = new Mock<PipelineJobCommand>("cmd", "command");
            mock.Setup(c => c.Execute()).Callback(() =>
            {
                SetStatus(mock.Object, status);
                SetOutput(mock.Object, output);
            });
            return mock;
        }

        private static void SetStatus(PipelineJobCommand command, PipelineJobStatus status)
            => typeof(PipelineJobCommand).GetProperty("Status")!.SetValue(command, status);

        private static void SetOutput(PipelineJobCommand command, string output)
            => typeof(PipelineJobCommand).GetProperty("Output")!.SetValue(command, output);
    }
}
