using Domain.Backlogs;
using System.Reflection;
using Xunit.Abstractions;

namespace DomainTests
{
    public class BacklogTests
    {
        //FR_B1 Het systeem moet een product backlog kunnen aanmaken en bijhouden. 

        //Het systeem moet een nieuwe backlog item kunnen toevoegen.
        [Fact]
        public void A_New_BacklogItem_Can_Be_Created()
        { //Arrange
            var productOwner = TestHelpers.CreateDeveloper("John", Role.Developer);
            var name = "Project 1";
            var project = new Project(productOwner, name);
            var backlog = project.Backlog;

            BacklogItem backlogItem1 = new BacklogItem("task1", "Hello world", 5, backlog);
            backlogItem1.AssignDeveloper(productOwner);
            //Act
            backlog.AddBacklogItem(backlogItem1);

            //Assert
            Assert.Contains(backlogItem1, project.Backlog.BacklogItems);
        }


        //�	Het systeem moet een backlog item kunnen bewerken.
        [Fact]
        public void A_BacklogItem_Can_Be_Edited()
        { //Arrange
            var productOwner = TestHelpers.CreateDeveloper("John", Role.Developer);
            var developer = TestHelpers.CreateDeveloper("Hans", Role.Developer);
            var name = "Project 1";
            var project = new Project(productOwner, name);
            var backlog = project.Backlog;

            BacklogItem backlogItem1 = new BacklogItem("task1", "Hello world", 5, backlog);
            backlogItem1.AssignDeveloper(productOwner);

            //Act
            backlog.AddBacklogItem(backlogItem1);

            backlogItem1.Name = "Task Hello world";
            backlogItem1.Description = "Hello world2";
            backlogItem1.Effort = 10;
            backlogItem1.AssignDeveloper(developer);

            //Assert
            Assert.Contains(backlogItem1, project.Backlog.BacklogItems);
            Assert.Equal("Task Hello world", backlogItem1.Name);
            Assert.Equal("Hello world2", backlogItem1.Description);
            Assert.Equal(10, backlogItem1.Effort);
            Assert.Equal(developer, backlogItem1.AssignedDeveloper);
        }

        //�	Gebruikers moeten de details van een backlog item kunnen bekijken, zoals beschrijving, gebruikersverhalen en bijlagen.
        [Fact]
        public void The_Details_Of_A_BacklogItem_Can_Be_Viewed()
        {
            //Arrange
            var productOwner = TestHelpers.CreateDeveloper("John", Role.Developer);
            var name = "Project 1";
            var project = new Project(productOwner, name);
            var backlog = project.Backlog;

            BacklogItem backlogItem1 = new BacklogItem("task1", "Hello world", 5, backlog);
            backlogItem1.AssignDeveloper(productOwner);

            //Act
            backlog.AddBacklogItem(backlogItem1);

            //Assert
            Assert.Contains(backlogItem1, project.Backlog.BacklogItems);
            Assert.Equal("task1", backlogItem1.Name);
            Assert.Equal("Hello world", backlogItem1.Description);
            Assert.Equal(5, backlogItem1.Effort);
            Assert.Equal(productOwner, backlogItem1.AssignedDeveloper);
        }

        //�	Het systeem moet de mogelijkheid bieden om backlog items te verwijderen.
        [Fact]
        public void A_BacklogItem_Can_Be_Deleted()
        {
            //Arrange
            var productOwner = TestHelpers.CreateDeveloper("John", Role.Developer);
            var name = "Project 1";
            var project = new Project(productOwner, name);
            var backlog = project.Backlog;

            BacklogItem backlogItem1 = new BacklogItem("task1", "Hello world", 5, backlog);
            backlogItem1.AssignDeveloper(productOwner);

            //Act
            backlog.AddBacklogItem(backlogItem1);
            backlog.RemoveBacklogItem(backlogItem1);

            //Assert
            Assert.DoesNotContain(backlogItem1, project.Backlog.BacklogItems);
        }

        //�	Een backlog mag niet dezelfde backlogitems bevatten.
        [Fact]
        public void A_BacklogItem_Cannot_Be_Added_Twice()
        {
            //Arrange
            var productOwner = TestHelpers.CreateDeveloper("John", Role.Developer);
            var name = "Project 1";
            var project = new Project(productOwner, name);
            var backlog = project.Backlog;

            BacklogItem backlogItem1 = new BacklogItem("task1", "Hello world", 5, backlog);
            backlogItem1.AssignDeveloper(productOwner);

            //Act
            backlog.AddBacklogItem(backlogItem1);
     
            //Assert
            Assert.Throws<Exception>(() => backlog.AddBacklogItem(backlogItem1));
            Assert.Single(project.Backlog.BacklogItems);
        }

        //FR_B2 Het systeem moet activiteiten binnen een backlog item kunnen aanmaken.

        //�	Aan een backlog item moet een activiteit toegevoegd kunnen worden.
        [Fact]
        public void An_Activity_Can_Be_Added_To_A_BacklogItem()
        {
            //Arrange
            var productOwner = TestHelpers.CreateDeveloper("John", Role.Developer);
            var name = "Project 1";
            var project = new Project(productOwner, name);
            var backlog = project.Backlog;


            BacklogItem backlogItem1 = new BacklogItem("task1", "Hello world", 5, backlog);
            backlogItem1.AssignDeveloper(productOwner);
            Activity activity1 = new Activity("activity1");
            activity1.AssignedDeveloper = productOwner;

            //Act
            backlog.AddBacklogItem(backlogItem1);
            backlogItem1.AddActivity(activity1);

            //Assert
            Assert.Contains(activity1, backlogItem1.Activities);
        }

        //�	Een activiteit moet aangepast kunnen worden.
        [Fact]
        public void An_Activity_Can_Be_Edited()
        {
            //Arrange
            var productOwner = TestHelpers.CreateDeveloper("John", Role.Developer);
            var developer = TestHelpers.CreateDeveloper("Hans", Role.Developer);
            var name = "Project 1";
            var project = new Project(productOwner, name);
            var backlog = project.Backlog;

            BacklogItem backlogItem1 = new BacklogItem("task1", "Hello world", 5, backlog);
            backlogItem1.AssignDeveloper(productOwner);
            Activity activity1 = new Activity("activity1");
            activity1.AssignedDeveloper = productOwner;

            //Act
            backlog.AddBacklogItem(backlogItem1);
            backlogItem1.AddActivity(activity1);

            activity1.Description = "Hello world2";
            activity1.AssignedDeveloper = developer;

            //Assert
            Assert.Contains(activity1, backlogItem1.Activities);
            Assert.Equal("Hello world2", activity1.Description);
            Assert.Equal(developer, activity1.AssignedDeveloper);
        }

        //�	Een activiteit moet ingezien kunnen worden.
        [Fact]
        public void An_Activity_Can_Be_Viewed()
        {
            //Arrange
            var productOwner = TestHelpers.CreateDeveloper("John", Role.Developer);
            var developer = TestHelpers.CreateDeveloper("Hans", Role.Developer);
            var name = "Project 1";
            var project = new Project(productOwner, name);
            var backlog = project.Backlog;

            BacklogItem backlogItem1 = new BacklogItem("task1", "Hello world", 5, backlog);
            backlogItem1.AssignDeveloper(productOwner);
            Activity activity1 = new Activity("activity1");
            activity1.AssignedDeveloper = productOwner;

            //Act
            backlog.AddBacklogItem(backlogItem1);
            backlogItem1.AddActivity(activity1);

            //Assert
            Assert.Contains(activity1, backlogItem1.Activities);
            Assert.Equal("activity1", activity1.Description);
            Assert.Equal(productOwner, activity1.AssignedDeveloper);
        }


    }
}
