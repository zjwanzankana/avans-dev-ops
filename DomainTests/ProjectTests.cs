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


    }
}
