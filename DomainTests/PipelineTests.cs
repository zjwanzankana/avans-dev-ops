using Domain.Pipelines;
using Domain.Pipelines.PipelineCommands;
using Domain.Sprints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainTests
{
    public class PipelineTests
    {
        //FR_P1 Het systeem moet ondersteuning bieden voor verschillende typen acties binnen een development pipeline, zoals Sources, Package, Build, Test, Analyse, Deploy en Utility.

        //•	Gebruikers kunnen activiteiten van elk type toevoegen aan een development pipeline.
        [Fact]
        public void A_Pipeline_Can_Be_Created()
        {
            //Arrange
            var productOwner = new Developer("John", Role.Developer);
            var name = "Project 1";
            var project = new Project(productOwner, name);


            //Act
            var pipelineCommand = new PipelineJobAnalyzeCommand("Analyze the code using blabla", "Command to use");

            var pipeline = new Pipeline(new List<PipelineJobCommand> { pipelineCommand } , "First pipeline");

            pipeline.Execute();

            //Assert
            Assert.True(pipeline.GetCommands().Count ==1);
        }

        //•	De acties worden correct uitgevoerd volgens de gedefinieerde volgorde in de pipeline.
        [Fact]
        public void A_Pipeline_Can_Be_Executed()
        {
            //Arrange
            var productOwner = new Developer("John", Role.Developer);
            var name = "Project 1";
            var project = new Project(productOwner, name);

            var pipelineCommand1 = new PipelineJobAnalyzeCommand("Analyze the code using blabla", "Command to use");
            var pipelineCommand2 = new PipelineJobBuildCommand("Build the code", "Command to use", false);
            var pipelineCommand3 = new PipelineJobDeployCommand("ADeploy", "Command to use upload.exe");

            var pipeline = new Pipeline(new List<PipelineJobCommand> { pipelineCommand1, pipelineCommand2, pipelineCommand3 }, "First pipeline");

            //Act
            pipeline.Execute();

            //Assert
            Assert.True(pipelineCommand1.GetStatus() == PipelineJobStatus.FINISHED);
        }



        //FR_P2 Het systeem moet de mogelijkheid bieden om een development pipeline te koppelen aan een deployment sprint.

        //•	Gebruikers kunnen een pipeline definiëren en koppelen aan een Sprint.
        [Fact]
        public void A_Pipeline_Can_Be_Added_To_A_Sprint()
        {
            //Arrange
            var productOwner = new Developer("John", Role.Developer);
            var name = "Project 1";
            var project = new Project(productOwner, name);

            var pipelineCommand1 = new PipelineJobAnalyzeCommand("Analyze the code using blabla", "Command to use");
            var pipelineCommand2 = new PipelineJobBuildCommand("Build the code", "Command to use", false);
            var pipelineCommand3 = new PipelineJobDeployCommand("ADeploy", "Command to use upload.exe");

            var pipeline = new Pipeline(new List<PipelineJobCommand> { pipelineCommand1, pipelineCommand2, pipelineCommand3 }, "First pipeline");

            var sprint = new ReviewSprint(project, "Sprint 1", DateTime.Now, DateTime.Now.AddDays(14), productOwner, new List<Developer>());

            //Act
            sprint.SetPipeline(pipeline);

            //Assert
            Assert.Equal(sprint.GetPipeline(), pipeline);
        }
        
    }
}
