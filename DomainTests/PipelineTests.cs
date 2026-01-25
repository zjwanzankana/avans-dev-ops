using Domain.Pipelines;
using Moq;
using Domain.Sprints;
using System;
using System.Collections.Generic;

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
            var productOwner = TestHelpers.CreateDeveloper("John", Role.Developer);
            var name = "Project 1";
            var project = new Project(productOwner, name);


            //Act
            var pipelineCommand = CreateCommand("Analyze the code using blabla", PipelineJobStatus.FINISHED);

            var pipeline = new Pipeline(new List<PipelineJobCommand> { pipelineCommand.Object } , "First pipeline");

            pipeline.Execute();

            //Assert
            Assert.True(pipeline.Commands.Count ==1);
            pipelineCommand.Verify(c => c.Execute(), Times.Once);
        }

        //•	De acties worden correct uitgevoerd volgens de gedefinieerde volgorde in de pipeline.
        [Fact]
        public void A_Pipeline_Can_Be_Executed()
        {
            //Arrange
            var productOwner = TestHelpers.CreateDeveloper("John", Role.Developer);
            var name = "Project 1";
            var project = new Project(productOwner, name);

            var pipelineCommand1 = CreateCommand("Analyze the code using blabla", PipelineJobStatus.FINISHED, "Analyze done");
            var pipelineCommand2 = CreateCommand("Build the code", PipelineJobStatus.FINISHED, "Build done");
            var pipelineCommand3 = CreateCommand("ADeploy", PipelineJobStatus.FINISHED, "Deploy done");

            var pipeline = new Pipeline(new List<PipelineJobCommand>
            {
                pipelineCommand1.Object,
                pipelineCommand2.Object,
                pipelineCommand3.Object
            }, "First pipeline");

            //Act
            pipeline.Execute();

            //Assert
            Assert.Equal(PipelineJobStatus.FINISHED, pipeline.Status);
            pipelineCommand1.Verify(c => c.Execute(), Times.Once);
            pipelineCommand2.Verify(c => c.Execute(), Times.Once);
            pipelineCommand3.Verify(c => c.Execute(), Times.Once);
        }



        //FR_P2 Het systeem moet de mogelijkheid bieden om een development pipeline te koppelen aan een deployment sprint.

        //•	Gebruikers kunnen een pipeline definiëren en koppelen aan een Sprint.
        [Fact]
        public void A_Pipeline_Can_Be_Added_To_A_Sprint()
        {
            //Arrange
            var productOwner = TestHelpers.CreateDeveloper("John", Role.Developer);
            var name = "Project 1";
            var project = new Project(productOwner, name);

            var pipelineCommand1 = CreateCommand("Analyze the code using blabla", PipelineJobStatus.FINISHED);
            var pipelineCommand2 = CreateCommand("Build the code", PipelineJobStatus.FINISHED);
            var pipelineCommand3 = CreateCommand("ADeploy", PipelineJobStatus.FINISHED);

            var pipeline = new Pipeline(new List<PipelineJobCommand>
            {
                pipelineCommand1.Object,
                pipelineCommand2.Object,
                pipelineCommand3.Object
            }, "First pipeline");

            var sprint = new ReviewSprint(project, "Sprint 1", DateTime.Now, DateTime.Now.AddDays(14), productOwner, new List<Developer>());

            //Act
            sprint.SetPipeline(pipeline);

            //Assert
            Assert.Equal(sprint.Pipeline, pipeline);
        }

        private static Mock<PipelineJobCommand> CreateCommand(string name, PipelineJobStatus status, string output = "ok")
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
