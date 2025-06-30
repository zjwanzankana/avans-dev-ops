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
            var productOwner = new Developer("John", Role.Developer);
            var name = "Project 1";

            //Act
            var project = new Project(productOwner, name);

            //Assert
            Assert.NotNull(project);
            Assert.Equal(project.GetName(), name);
            Assert.NotNull(project.GetBacklog());
            Assert.NotNull(project.GetRepository());
            Assert.NotNull(project.GetPipelines());
            Assert.Equal(project.GetProductOwner(), productOwner);

            Assert.Null(project.GetForum());
        }


    }
}
