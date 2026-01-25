using Domain;
using Domain.Developers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DomainTests
{
    public class ProjectTests
    {
        //FR_P1 Het systeem moet de mogelijkheid bieden om projecten aan te maken.
        [Fact]
        public void Creating_A_Project_Should_Not_Throw_An_Exception()
        {
            //Arrange
            var productOwner = TestHelpers.CreateDeveloper("John", Role.Developer);
            var name = "Project 1";

            //Act
            var project = new Project(productOwner, name);

            //Assert
            Assert.NotNull(project);
            Assert.Equal(project.Name, name);
            Assert.NotNull(project.Backlog);
            Assert.NotNull(project.Repository);
            Assert.NotNull(project.Pipelines);
            Assert.Equal(project.ProductOwner, productOwner);

            Assert.Null(project.Forum);
        }

        [Fact]
        public void A_Project_Can_Create_A_Forum_Once()
        {
            var productOwner = TestHelpers.CreateDeveloper("John", Role.Developer);
            var project = new Project(productOwner, "Project 1");

            project.CreateForum();

            Assert.NotNull(project.Forum);
            Assert.Throws<InvalidOperationException>(() => project.CreateForum());
        }

        [Fact]
        public void A_Project_Can_Add_Testers()
        {
            var productOwner = TestHelpers.CreateDeveloper("John", Role.Developer);
            var tester = TestHelpers.CreateDeveloper("Jane", Role.Tester);
            var project = new Project(productOwner, "Project 1");

            project.AddTester(tester);

            Assert.Single(project.Testers);
        }

        [Fact]
        public void A_Project_Cannot_Add_Non_Testers()
        {
            var productOwner = TestHelpers.CreateDeveloper("John", Role.Developer);
            var developer = TestHelpers.CreateDeveloper("Jane", Role.Developer);
            var project = new Project(productOwner, "Project 1");

            Assert.Throws<InvalidOperationException>(() => project.AddTester(developer));
        }

        [Fact]
        public void A_Project_Cannot_Add_The_Same_Tester_Twice()
        {
            var productOwner = TestHelpers.CreateDeveloper("John", Role.Developer);
            var tester = TestHelpers.CreateDeveloper("Jane", Role.Tester);
            var project = new Project(productOwner, "Project 1");

            project.AddTester(tester);

            Assert.Throws<InvalidOperationException>(() => project.AddTester(tester));
        }

    }
}
