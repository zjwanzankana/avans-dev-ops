using Domain.Pipelines;
using Domain.Sprints;
using Domain.Sprints.SprintStates;
using Moq;
using System;
using System.Collections.Generic;

namespace DomainTests
{
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
        public void Scheduled_State_Has_No_Previous_Or_Start_Action()
        {
            var productOwner = TestHelpers.CreateDeveloper("John", Role.Developer);
            var project = new Project(productOwner, "Project 1");
            var sprint = new ReviewSprint(project, "Sprint 1", DateTime.Today, DateTime.Today.AddDays(7), productOwner, new List<Developer>());

            Assert.Throws<InvalidOperationException>(() => sprint.State.PreviousState());
            Assert.Throws<InvalidOperationException>(() => sprint.State.StartStateAction());
        }

        [Fact]
        public void A_Sprint_Can_Move_To_In_Progress_And_Back()
        {
            var productOwner = TestHelpers.CreateDeveloper("John", Role.Developer);
            var project = new Project(productOwner, "Project 1");
            var sprint = new ReviewSprint(project, "Sprint 1", DateTime.Today, DateTime.Today.AddDays(7), productOwner, new List<Developer>());

            sprint.State.NextState();
            Assert.Equal(ESprintStates.InProgress, sprint.State.GetSprintState());

            sprint.State.PreviousState();
            Assert.Equal(ESprintStates.Scheduled, sprint.State.GetSprintState());
        }

        [Fact]
        public void A_Sprint_Can_Finish_And_Run_Pipeline_For_Release()
        {
            var productOwner = TestHelpers.CreateDeveloper("John", Role.Developer);
            var project = new Project(productOwner, "Project 1");
            var pipelineCommand = CreateCommand("Analyze", PipelineJobStatus.FINISHED, "Analyze done");
            var pipeline = new Pipeline(new List<PipelineJobCommand> { pipelineCommand.Object }, "release");
            var sprint = new ReleaseSprint(project, "Release 1", DateTime.Today, DateTime.Today.AddDays(7), productOwner, new List<Developer> { productOwner }, pipeline);

            sprint.State.NextState();
            sprint.State.NextState();

            Assert.Equal(ESprintStates.Finished, sprint.State.GetSprintState());
            Assert.Equal(PipelineJobStatus.FINISHED, pipeline.Status);
            pipelineCommand.Verify(c => c.Execute(), Times.Once);
        }

        [Fact]
        public void A_Finished_Review_Sprint_SetReview_Throws_But_Sets_Review()
        {
            var productOwner = TestHelpers.CreateDeveloper("John", Role.Developer);
            var project = new Project(productOwner, "Project 1");
            var sprint = new ReviewSprint(project, "Sprint 1", DateTime.Today, DateTime.Today.AddDays(7), productOwner, new List<Developer> { productOwner });
            var review = new Review("Looks good", productOwner, sprint);

            sprint.State.NextState();
            sprint.State.NextState();

            Assert.Throws<InvalidOperationException>(() => sprint.State.SetReview(review));
            Assert.Equal(review, sprint.Review);
        }

        [Fact]
        public void A_Finished_Review_Sprint_Can_Set_Review_Item()
        {
            var productOwner = TestHelpers.CreateDeveloper("John", Role.Developer);
            var project = new Project(productOwner, "Project 1");
            var sprint = new ReviewSprint(project, "Sprint 1", DateTime.Today, DateTime.Today.AddDays(7), productOwner, new List<Developer> { productOwner });
            var review = new Review("Looks good", productOwner, sprint);

            sprint.State.NextState();
            sprint.State.NextState();
            sprint.SetReviewItem(review);

            Assert.True(sprint.IsReviewDone);
            Assert.Equal(review, sprint.Review);
        }

        [Fact]
        public void A_Finished_Release_Sprint_Does_Not_Accept_Reviews()
        {
            var productOwner = TestHelpers.CreateDeveloper("John", Role.Developer);
            var project = new Project(productOwner, "Project 1");
            var pipelineCommand = CreateCommand("Analyze", PipelineJobStatus.FINISHED, "Analyze done");
            var pipeline = new Pipeline(new List<PipelineJobCommand> { pipelineCommand.Object }, "release");
            var sprint = new ReleaseSprint(project, "Release 1", DateTime.Today, DateTime.Today.AddDays(7), productOwner, new List<Developer> { productOwner }, pipeline);
            var review = new Review("Looks good", productOwner, sprint);

            sprint.State.NextState();
            sprint.State.NextState();

            Assert.Throws<InvalidOperationException>(() => sprint.State.SetReview(review));
        }

        [Fact]
        public void Sprint_Dates_And_Name_Cannot_Change_When_Pipeline_Is_Running()
        {
            var productOwner = TestHelpers.CreateDeveloper("John", Role.Developer);
            var project = new Project(productOwner, "Project 1");
            var sprint = new ReviewSprint(project, "Sprint 1", DateTime.Today, DateTime.Today.AddDays(7), productOwner, new List<Developer> { productOwner });
            var pipelineCommand = CreateCommand("Analyze", PipelineJobStatus.FINISHED, "Analyze done");
            var pipeline = new Pipeline(new List<PipelineJobCommand> { pipelineCommand.Object }, "pipeline");

            pipeline.SetStatus(PipelineJobStatus.Running);
            sprint.SetPipeline(pipeline);

            Assert.Throws<InvalidOperationException>(() => sprint.SetName("Blocked"));
            Assert.Throws<InvalidOperationException>(() => sprint.SetStartDate(DateTime.Today.AddDays(1)));
            Assert.Throws<InvalidOperationException>(() => sprint.SetEndDate(DateTime.Today.AddDays(10)));
        }

        private static Mock<PipelineJobCommand> CreateCommand(string name, PipelineJobStatus status, string output)
        {
            var mock = new Mock<PipelineJobCommand>(name, "command");
            mock.Setup(c => c.Execute()).Callback(() =>
            {
                SetCommandStatus(mock.Object, status);
                SetCommandOutput(mock.Object, output);
            });
            return mock;
        }

        private static void SetCommandStatus(PipelineJobCommand command, PipelineJobStatus status)
        {
            typeof(PipelineJobCommand).GetProperty("Status")!.SetValue(command, status);
        }

        private static void SetCommandOutput(PipelineJobCommand command, string output)
        {
            typeof(PipelineJobCommand).GetProperty("Output")!.SetValue(command, output);
        }
    }
}
