using Domain.Backlogs;
using Domain.Backlogs.BacklogItemStates;
using Domain.Pipelines;
using Domain.Pipelines.PipelineCommands;
using Domain.Sprints;
using Domain.Sprints.SprintStates;
using System;
using System.Reflection;

namespace DomainTests
{
    public class SprintTests
    {
        //FR_S1 Het systeem moet de mogelijkheid bieden om sprints aan te maken
        //•	Een sprint moet toegevoegd kunnen worden
        [Fact]
        public void A_Sprint_Can_Be_Added_To_A_Project()
        {
            //Arrange
            var productOwner = TestHelpers.CreateDeveloper("John", Role.Developer);

            var developer1 = TestHelpers.CreateDeveloper("Hans", Role.Developer);
            var developer2 = TestHelpers.CreateDeveloper("Jan", Role.Developer);
            var developer3 = TestHelpers.CreateDeveloper("Hans2", Role.Tester);
            var developers = new List<Developer> { developer1, developer2, developer3, productOwner };

            var name = "Project 1";
            var project = new Project(productOwner, name);
            var backlog = project.Backlog;
            var sprint = new ReviewSprint(project, "Sprint 1", DateTime.Now, DateTime.Now.AddDays(14), productOwner, developers);

            //Act
            project.AddSprint(sprint);

            //Assert
            Assert.Contains(sprint, project.Sprints);
        }

        //•	Een sprint moet aangepast kunnen worden.
        [Fact]
        public void A_Sprint_Can_Be_Edited()
        {
            //Arrange
            var productOwner = TestHelpers.CreateDeveloper("John", Role.Developer);

            var developer1 = TestHelpers.CreateDeveloper("Hans", Role.Developer);
            var developer2 = TestHelpers.CreateDeveloper("Jan", Role.Developer);
            var developer3 = TestHelpers.CreateDeveloper("Hans2", Role.Tester);
            var developers = new List<Developer> { developer1, developer2, developer3, productOwner };

            var name = "Project 1";
            var project = new Project(productOwner, name);
            var backlog = project.Backlog;
            var sprint = new ReviewSprint(project, "Sprint 1", DateTime.Now, DateTime.Now.AddDays(14), productOwner, developers);

            //Act
            project.AddSprint(sprint);
            sprint.SetName("Sprint 2");

            //Assert
            Assert.Equal("Sprint 2", sprint.Name);
        }

        //•	Een sprint moet ingezien kunnen worden.

        [Fact]
        public void A_Sprint_Can_Be_Viewed()
        {
            //Arrange
            var productOwner = TestHelpers.CreateDeveloper("John", Role.Developer);

            var developer1 = TestHelpers.CreateDeveloper("Hans", Role.Developer);
            var developer2 = TestHelpers.CreateDeveloper("Jan", Role.Developer);
            var developer3 = TestHelpers.CreateDeveloper("Hans2", Role.Tester);
            var developers = new List<Developer> { developer1, developer2, developer3, productOwner };

            var name = "Project 1";
            var project = new Project(productOwner, name);
            var backlog = project.Backlog;
            var sprint = new ReviewSprint(project, "Sprint 1", DateTime.Now, DateTime.Now.AddDays(14), productOwner, developers);

            //Act
            project.AddSprint(sprint);

            //Assert
            Assert.Contains(sprint, project.Sprints);
            Assert.Equal("Sprint 1", sprint.Name);
            Assert.Equal(productOwner, sprint.ScrumMaster);
        }

        //FR_S2 Het systeem moet de mogelijkheid bieden om backlog items aan een sprintbacklog toe te voegen.
        //•	Een backlog item moet aan een sprintbacklog toegevoegd kunnen worden.
        [Fact]
        public void A_BacklogItem_Can_Be_Added_To_A_Sprint()
        {
            //Arrange
            var productOwner = TestHelpers.CreateDeveloper("John", Role.Developer);

            var developer1 = TestHelpers.CreateDeveloper("Hans", Role.Developer);
            var developer2 = TestHelpers.CreateDeveloper("Jan", Role.Developer);
            var developer3 = TestHelpers.CreateDeveloper("Hans2", Role.Tester);
            var developers = new List<Developer> { developer1, developer2, developer3, productOwner };

            var name = "Project 1";
            var project = new Project(productOwner, name);
            var backlog = project.Backlog;
            var sprint = new ReviewSprint(project, "Sprint 1", DateTime.Now, DateTime.Now.AddDays(14), productOwner, developers);

            var backlogItem = new BacklogItem("BacklogItem 1", "Description 1", 1, backlog);


            //Act
            project.AddSprint(sprint);
            sprint.AddToSprintBacklog(backlogItem);

            //Assert
            Assert.Contains(backlogItem, sprint.BacklogItems);
        }


        //FR_S3 Een sprint moet de fases Scheduled, InProgress en Finished ondersteunen.
        //•	Een sprint moet de fase Scheduled ondersteunen.
        [Fact]
        public void A_Sprint_Can_Be_Scheduled()
        {
            //Arrange
            var productOwner = TestHelpers.CreateDeveloper("John", Role.Developer);

            var developer1 = TestHelpers.CreateDeveloper("Hans", Role.Developer);
            var developer2 = TestHelpers.CreateDeveloper("Jan", Role.Developer);
            var developer3 = TestHelpers.CreateDeveloper("Hans2", Role.Tester);
            var developers = new List<Developer> { developer1, developer2, developer3, productOwner };

            var name = "Project 1";
            var project = new Project(productOwner, name);
            var backlog = project.Backlog;
            var sprint = new ReviewSprint(project, "Sprint 1", DateTime.Now, DateTime.Now.AddDays(14), productOwner, developers);

            //Act
            project.AddSprint(sprint);

            //Assert
            Assert.Equal(ESprintStates.Scheduled, sprint.State.GetSprintState());
        }

        //•	Een sprint moet de fase InProgress ondersteunen.
        [Fact]
        public void A_Sprint_Can_Go_From_Scheduled_State_To_InProgress()
        {
            // Arrange
            var productOwner = TestHelpers.CreateDeveloper("John", Role.Developer);

            var developer1 = TestHelpers.CreateDeveloper("Hans", Role.Developer);
            var developer2 = TestHelpers.CreateDeveloper("Jan", Role.Developer);
            var developer3 = TestHelpers.CreateDeveloper("Hans2", Role.Tester);
            var developers = new List<Developer> { developer1, developer2, developer3, productOwner };

            var name = "Project 1";
            var project = new Project(productOwner, name);
            var backlog = project.Backlog;
            var sprint = new ReviewSprint(project, "Sprint 1", DateTime.Now, DateTime.Now.AddDays(14), productOwner, developers);

            //Act
            project.AddSprint(sprint);
            sprint.State.NextState();

            //Assert
            Assert.Equal(ESprintStates.InProgress, sprint.State.GetSprintState());
        }

        //•	Een sprint moet de fase Finished ondersteunen.
        [Fact]
        public void A_Sprint_Can_Go_From_InProgress_State_To_Finished()
        {
            // Arrange
            var productOwner = TestHelpers.CreateDeveloper("John", Role.Developer);

            var developer1 = TestHelpers.CreateDeveloper("Hans", Role.Developer);
            var developer2 = TestHelpers.CreateDeveloper("Jan", Role.Developer);
            var developer3 = TestHelpers.CreateDeveloper("Hans2", Role.Tester);
            var developers = new List<Developer> { developer1, developer2, developer3, productOwner };

            var name = "Project 1";
            var project = new Project(productOwner, name);
            var backlog = project.Backlog;
            var sprint = new ReviewSprint(project, "Sprint 1", DateTime.Now, DateTime.Now.AddDays(14), productOwner, developers);

            project.AddSprint(sprint);
            sprint.State.NextState();

            //Act
            sprint.State.NextState();

            //Assert
            Assert.Equal(ESprintStates.Finished, sprint.State.GetSprintState());
        }

        // * Een sprint kan niet naar de vorige fase wanneer de fase schedules is.
        [Fact]
        public void A_Sprint_Can_Not_Go_From_Scheduled_State_To_Previous_State()
        {
            // Arrange
            var productOwner = TestHelpers.CreateDeveloper("John", Role.Developer);

            var developer1 = TestHelpers.CreateDeveloper("Hans", Role.Developer);
            var developer2 = TestHelpers.CreateDeveloper("Jan", Role.Developer);
            var developer3 = TestHelpers.CreateDeveloper("Hans2", Role.Tester);
            var developers = new List<Developer> { developer1, developer2, developer3, productOwner };

            var name = "Project 1";
            var project = new Project(productOwner, name);
            var backlog = project.Backlog;
            var sprint = new ReviewSprint(project, "Sprint 1", DateTime.Now, DateTime.Now.AddDays(14), productOwner, developers);

            project.AddSprint(sprint);

            //Act


            //Assert
            Assert.Equal(ESprintStates.Scheduled, sprint.State.GetSprintState());
            Assert.Throws<InvalidOperationException>(() => sprint.State.PreviousState());
        }


        // * Check if finished sprint starts a Pipeline
        [Fact]
        public void A_ReleaseSprint_Starts_Pipeline_When_Finished()
        {
            // Arrange
            var productOwner = TestHelpers.CreateDeveloper("John", Role.Developer);

            var developer1 = TestHelpers.CreateDeveloper("Hans", Role.Developer);
            var developer2 = TestHelpers.CreateDeveloper("Jan", Role.Developer);
            var developer3 = TestHelpers.CreateDeveloper("Hans2", Role.Tester);
            var developers = new List<Developer> { developer1, developer2, developer3, productOwner };

            var name = "Project 1";
            var project = new Project(productOwner, name);
            var backlog = project.Backlog;
            var pipeline = new Pipeline(new List<PipelineJobCommand> {new PipelineJobAnalyzeCommand("Hey", "analyze the code"), new PipelineJobBuildCommand("Hey", "Biuld the code", true) }, "First");

            var sprint = SprintFactory.GetReleaseSprint(project, "Sprint 1", DateTime.Now, DateTime.Now.AddDays(14), productOwner, developers, pipeline);

            //Assert.

            project.AddSprint(sprint);

            //Act
            sprint.State.NextState();
            sprint.State.NextState();


            //Assert
            Assert.Equal(PipelineJobStatus.FINISHED, pipeline.Status);

        }

        //•	Zolang de activiteiten van de development pipeline worden uitgevoerd, kan de sprint niet gewijzigd worden.

        [Fact]
        public void IF_Pipeline_Is_Running_Sprint_Cant_Be_Changed()
        {
            // Arrange
            var productOwner = TestHelpers.CreateDeveloper("John", Role.Developer);

            var developer1 = TestHelpers.CreateDeveloper("Hans", Role.Developer);
            var developer2 = TestHelpers.CreateDeveloper("Jan", Role.Developer);
            var developer3 = TestHelpers.CreateDeveloper("Hans2", Role.Tester);
            var developers = new List<Developer> { developer1, developer2, developer3, productOwner };

            var name = "Project 1";
            var project = new Project(productOwner, name);
            var backlog = project.Backlog;
            var pipeline = new Pipeline(new List<PipelineJobCommand> { new PipelineJobAnalyzeCommand("Hey", "analyze the code"), new PipelineJobBuildCommand("Hey", "Biuld the code", true) }, "First");

            var sprint = SprintFactory.GetReleaseSprint(project, "Sprint 1", DateTime.Now, DateTime.Now.AddDays(14), productOwner, developers, pipeline);

            //Assert.

            project.AddSprint(sprint);
            sprint.State.NextState();
            sprint.State.NextState();

            //Act
            pipeline.SetStatus(PipelineJobStatus.Running);


            //Assert
            Assert.Throws<InvalidOperationException>(() => sprint.SetName("Finshed sprint2"));
        }


        [Fact]
        public void IF_Pipeline_Is_Not_Running_Sprint_Can_Be_Changed()
        {
            // Arrange
            var productOwner = TestHelpers.CreateDeveloper("John", Role.Developer);

            var developer1 = TestHelpers.CreateDeveloper("Hans", Role.Developer);
            var developer2 = TestHelpers.CreateDeveloper("Jan", Role.Developer);
            var developer3 = TestHelpers.CreateDeveloper("Hans2", Role.Tester);
            var developers = new List<Developer> { developer1, developer2, developer3, productOwner };

            var name = "Project 1";
            var project = new Project(productOwner, name);
            var backlog = project.Backlog;
            var pipeline = new Pipeline(new List<PipelineJobCommand> { new PipelineJobAnalyzeCommand("Hey", "analyze the code"), new PipelineJobBuildCommand("Hey", "Biuld the code", true) }, "First");

            var sprint = SprintFactory.GetReleaseSprint(project, "Sprint 1", DateTime.Now, DateTime.Now.AddDays(14), productOwner, developers, pipeline);

            //Assert.

            project.AddSprint(sprint);
            sprint.State.NextState();
            sprint.State.NextState();

            //Act
            sprint.SetName("Finshed sprint");

            //Assert
            Assert.Equal("Finshed sprint", sprint.Name);
        }

        //•	De reviewSprint kan alleen afgesloten worden als een sprint helemaal is afgerond.
        [Fact]
        public void A_ReviewSprint_Can_Only_Be_Closed_When_Sprint_Is_Finished()
        {
            // Arrange
            var productOwner = TestHelpers.CreateDeveloper("John", Role.Developer);

            var developer1 = TestHelpers.CreateDeveloper("Hans", Role.Developer);
            var developer2 = TestHelpers.CreateDeveloper("Jan", Role.Developer);
            var developer3 = TestHelpers.CreateDeveloper("Hans2", Role.Tester);
            var developers = new List<Developer> { developer1, developer2, developer3, productOwner };

            var name = "Project 1";
            var project = new Project(productOwner, name);
            var backlog = project.Backlog;
            var pipeline = new Pipeline(new List<PipelineJobCommand> { new PipelineJobAnalyzeCommand("Hey", "analyze the code"), new PipelineJobBuildCommand("Hey", "Biuld the code", true) }, "First");

            ReviewSprint sprint = SprintFactory.GetReviewSprint(project, "Sprint 1", DateTime.Now, DateTime.Now.AddDays(14), productOwner, developers);

            //Assert.

            project.AddSprint(sprint);
            sprint.State.NextState();
            sprint.State.NextState();

            //Act

            //Assert

            var review = new Review("This is the review", productOwner, sprint);
            sprint.SetReviewItem(review);

            //Act
            Assert.Equal(ESprintStates.Finished, sprint.State.GetSprintState());
            Assert.True(sprint.IsReviewDone);
            Assert.Equal(sprint.Review,review);
        }

        //•	De reviewSprint kan alleen afgesloten worden als een sprint helemaal is afgerond.
        [Fact]
        public void A_ReviewSprint_Cannot_Be_Closed_When_Sprint_Is_Not_Finished()
        {
            // Arrange
            var productOwner = TestHelpers.CreateDeveloper("John", Role.Developer);

            var developer1 = TestHelpers.CreateDeveloper("Hans", Role.Developer);
            var developer2 = TestHelpers.CreateDeveloper("Jan", Role.Developer);
            var developer3 = TestHelpers.CreateDeveloper("Hans2", Role.Tester);
            var developers = new List<Developer> { developer1, developer2, developer3, productOwner };

            var name = "Project 1";
            var project = new Project(productOwner, name);
            var backlog = project.Backlog;
            var pipeline = new Pipeline(new List<PipelineJobCommand> { new PipelineJobAnalyzeCommand("Hey", "analyze the code"), new PipelineJobBuildCommand("Hey", "Biuld the code", true) }, "First");

            ReviewSprint sprint = SprintFactory.GetReviewSprint(project, "Sprint 1", DateTime.Now, DateTime.Now.AddDays(14), productOwner, developers);



            project.AddSprint(sprint);
            sprint.State.NextState();

            //Act
            var review = new Review("This is the review", productOwner, sprint);

            //Assert
            Assert.Throws<InvalidOperationException>(() => sprint.SetReviewItem(review));
        }
    }
}
