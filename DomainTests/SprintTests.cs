using Domain.Pipelines;
using Domain.Sprints;
using Domain.Sprints.SprintStates;
using Moq;
using System;
using System.Collections.Generic;

namespace DomainTests
{
    public class SprintTests
    {
        private static List<Developer> TeamWith(Developer productOwner)
        {
            return new List<Developer>
            {
                TestHelpers.CreateDeveloper("Hans", Role.Developer),
                TestHelpers.CreateDeveloper("Jan", Role.Developer),
                TestHelpers.CreateDeveloper("Hans2", Role.Tester),
                productOwner
            };
        }

        // FR_S1 - sprint aanmaken/toevoegen.
        [Fact]
        public void A_Sprint_Can_Be_Added_To_A_Project()
        {
            var productOwner = TestHelpers.CreateDeveloper("John", Role.Developer);
            var project = new Project(productOwner, "Project 1");
            var sprint = new ReviewSprint(project, "Sprint 1", DateTime.Now, DateTime.Now.AddDays(14), productOwner, TeamWith(productOwner));

            project.AddSprint(sprint);

            Assert.Contains(sprint, project.Sprints);
        }

        // FR_S1 - sprint aanpassen.
        [Fact]
        public void A_Sprint_Can_Be_Edited()
        {
            var productOwner = TestHelpers.CreateDeveloper("John", Role.Developer);
            var project = new Project(productOwner, "Project 1");
            var sprint = new ReviewSprint(project, "Sprint 1", DateTime.Now, DateTime.Now.AddDays(14), productOwner, TeamWith(productOwner));

            project.AddSprint(sprint);
            sprint.SetName("Sprint 2");

            Assert.Equal("Sprint 2", sprint.Name);
        }

        // FR_S1 - sprint inzien.
        [Fact]
        public void A_Sprint_Can_Be_Viewed()
        {
            var productOwner = TestHelpers.CreateDeveloper("John", Role.Developer);
            var project = new Project(productOwner, "Project 1");
            var sprint = new ReviewSprint(project, "Sprint 1", DateTime.Now, DateTime.Now.AddDays(14), productOwner, TeamWith(productOwner));

            project.AddSprint(sprint);

            Assert.Contains(sprint, project.Sprints);
            Assert.Equal("Sprint 1", sprint.Name);
            Assert.Equal(productOwner, sprint.ScrumMaster);
        }

        // FR_S2 - backlog item aan sprint backlog toevoegen.
        [Fact]
        public void A_BacklogItem_Can_Be_Added_To_A_Sprint()
        {
            var productOwner = TestHelpers.CreateDeveloper("John", Role.Developer);
            var project = new Project(productOwner, "Project 1");
            var sprint = new ReviewSprint(project, "Sprint 1", DateTime.Now, DateTime.Now.AddDays(14), productOwner, TeamWith(productOwner));
            var backlogItem = new BacklogItem("BacklogItem 1", "Description 1", 1, project.Backlog);

            project.AddSprint(sprint);
            sprint.AddToSprintBacklog(backlogItem);

            Assert.Contains(backlogItem, sprint.BacklogItems);
        }

        // FR_S3 - Scheduled fase.
        [Fact]
        public void A_Sprint_Can_Be_Scheduled()
        {
            var productOwner = TestHelpers.CreateDeveloper("John", Role.Developer);
            var project = new Project(productOwner, "Project 1");
            var sprint = new ReviewSprint(project, "Sprint 1", DateTime.Now, DateTime.Now.AddDays(14), productOwner, TeamWith(productOwner));

            project.AddSprint(sprint);

            Assert.Equal(ESprintStates.Scheduled, sprint.State.GetSprintState());
        }

        // FR_S3 - Scheduled -> InProgress.
        [Fact]
        public void A_Sprint_Can_Go_From_Scheduled_State_To_InProgress()
        {
            var productOwner = TestHelpers.CreateDeveloper("John", Role.Developer);
            var project = new Project(productOwner, "Project 1");
            var sprint = new ReviewSprint(project, "Sprint 1", DateTime.Now, DateTime.Now.AddDays(14), productOwner, TeamWith(productOwner));

            project.AddSprint(sprint);
            sprint.Start();

            Assert.Equal(ESprintStates.InProgress, sprint.State.GetSprintState());
        }

        // FR_S3 - InProgress -> Finished.
        [Fact]
        public void A_Sprint_Can_Go_From_InProgress_State_To_Finished()
        {
            var productOwner = TestHelpers.CreateDeveloper("John", Role.Developer);
            var project = new Project(productOwner, "Project 1");
            var sprint = new ReviewSprint(project, "Sprint 1", DateTime.Now, DateTime.Now.AddDays(14), productOwner, TeamWith(productOwner));

            project.AddSprint(sprint);
            sprint.Start();
            sprint.Finish();

            Assert.Equal(ESprintStates.Finished, sprint.State.GetSprintState());
        }

        // FR_S3 - vanuit Scheduled kan niet afgerond worden (geen ongeldige sprongen).
        [Fact]
        public void A_Sprint_Cannot_Finish_From_Scheduled()
        {
            var productOwner = TestHelpers.CreateDeveloper("John", Role.Developer);
            var project = new Project(productOwner, "Project 1");
            var sprint = new ReviewSprint(project, "Sprint 1", DateTime.Now, DateTime.Now.AddDays(14), productOwner, TeamWith(productOwner));

            project.AddSprint(sprint);

            Assert.Equal(ESprintStates.Scheduled, sprint.State.GetSprintState());
            Assert.Throws<InvalidOperationException>(() => sprint.Finish());
        }

        // FR_S3 - finished release sprint start de pipeline.
        [Fact]
        public void A_ReleaseSprint_Starts_Pipeline_When_Finished()
        {
            var productOwner = TestHelpers.CreateDeveloper("John", Role.Developer);
            var project = new Project(productOwner, "Project 1");
            var pipeline = new Pipeline(new List<PipelineJobCommand>
            {
                CreateCommand(PipelineJobStatus.FINISHED, "Analyze done").Object,
                CreateCommand(PipelineJobStatus.FINISHED, "Build done").Object
            }, "First");

            var sprint = SprintFactory.GetReleaseSprint(project, "Sprint 1", DateTime.Now, DateTime.Now.AddDays(14), productOwner, TeamWith(productOwner), pipeline);
            project.AddSprint(sprint);

            sprint.Start();
            sprint.Finish();

            Assert.Equal(PipelineJobStatus.FINISHED, pipeline.Status);
        }

        // FR_S3 - sprint kan niet wijzigen terwijl de pipeline draait.
        [Fact]
        public void IF_Pipeline_Is_Running_Sprint_Cant_Be_Changed()
        {
            var productOwner = TestHelpers.CreateDeveloper("John", Role.Developer);
            var project = new Project(productOwner, "Project 1");
            var pipeline = new Pipeline(new List<PipelineJobCommand>
            {
                CreateCommand(PipelineJobStatus.FINISHED, "Analyze done").Object
            }, "First");

            var sprint = SprintFactory.GetReleaseSprint(project, "Sprint 1", DateTime.Now, DateTime.Now.AddDays(14), productOwner, TeamWith(productOwner), pipeline);
            project.AddSprint(sprint);
            sprint.Start();
            sprint.Finish();

            pipeline.SetStatus(PipelineJobStatus.Running);

            Assert.Throws<InvalidOperationException>(() => sprint.SetName("Finished sprint2"));
        }

        [Fact]
        public void IF_Pipeline_Is_Not_Running_Sprint_Can_Be_Changed()
        {
            var productOwner = TestHelpers.CreateDeveloper("John", Role.Developer);
            var project = new Project(productOwner, "Project 1");
            var pipeline = new Pipeline(new List<PipelineJobCommand>
            {
                CreateCommand(PipelineJobStatus.FINISHED, "Analyze done").Object
            }, "First");

            var sprint = SprintFactory.GetReleaseSprint(project, "Sprint 1", DateTime.Now, DateTime.Now.AddDays(14), productOwner, TeamWith(productOwner), pipeline);
            project.AddSprint(sprint);
            sprint.Start();
            sprint.Finish();

            sprint.SetName("Finished sprint");

            Assert.Equal("Finished sprint", sprint.Name);
        }

        // FR_S4 - review sprint sluiten kan alleen wanneer de sprint is afgerond.
        [Fact]
        public void A_ReviewSprint_Can_Only_Be_Closed_When_Sprint_Is_Finished()
        {
            var productOwner = TestHelpers.CreateDeveloper("John", Role.Developer);
            var project = new Project(productOwner, "Project 1");
            ReviewSprint sprint = SprintFactory.GetReviewSprint(project, "Sprint 1", DateTime.Now, DateTime.Now.AddDays(14), productOwner, TeamWith(productOwner));

            project.AddSprint(sprint);
            sprint.Start();
            sprint.Finish();

            var review = new Review("This is the review", productOwner, sprint);
            sprint.SetReviewItem(review);

            Assert.Equal(ESprintStates.Finished, sprint.State.GetSprintState());
            Assert.True(sprint.IsReviewDone);
            Assert.Equal(sprint.Review, review);
        }

        // FR_S4 - review kan niet geupload worden voordat de sprint afgerond is.
        [Fact]
        public void A_ReviewSprint_Cannot_Be_Closed_When_Sprint_Is_Not_Finished()
        {
            var productOwner = TestHelpers.CreateDeveloper("John", Role.Developer);
            var project = new Project(productOwner, "Project 1");
            ReviewSprint sprint = SprintFactory.GetReviewSprint(project, "Sprint 1", DateTime.Now, DateTime.Now.AddDays(14), productOwner, TeamWith(productOwner));

            project.AddSprint(sprint);
            sprint.Start();

            var review = new Review("This is the review", productOwner, sprint);

            Assert.Throws<InvalidOperationException>(() => sprint.SetReviewItem(review));
        }

        private static Mock<PipelineJobCommand> CreateCommand(PipelineJobStatus status, string output)
        {
            var mock = new Mock<PipelineJobCommand>("cmd", "command");
            mock.Setup(c => c.Execute()).Callback(() =>
            {
                typeof(PipelineJobCommand).GetProperty("Status")!.SetValue(mock.Object, status);
                typeof(PipelineJobCommand).GetProperty("Output")!.SetValue(mock.Object, output);
            });
            return mock;
        }
    }
}
