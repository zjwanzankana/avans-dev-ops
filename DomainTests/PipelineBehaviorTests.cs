using Domain.Pipelines;
using Moq;
using System;
using System.Collections.Generic;

namespace DomainTests
{
    public class PipelineBehaviorTests
    {
        [Fact]
        public void A_Pipeline_Requires_Commands_To_Execute()
        {
            var pipeline = new Pipeline(new List<PipelineJobCommand>(), "empty");

            Assert.Throws<InvalidOperationException>(() => pipeline.Execute());
        }

        [Fact]
        public void Commands_Are_Only_Available_When_Pipeline_Is_Done()
        {
            var pipeline = new Pipeline(new List<PipelineJobCommand>
            {
                CreateCommand("Analyze", PipelineJobStatus.FINISHED, "Analyze done").Object
            }, "pipeline");

            Assert.Throws<InvalidOperationException>(() => pipeline.Commands);

            pipeline.Execute();

            Assert.Single(pipeline.Commands);
        }

        [Fact]
        public void Current_Command_Is_Available_After_Execution()
        {
            var analyze = CreateCommand("Analyze", PipelineJobStatus.FINISHED, "Analyze done");
            var build = CreateCommand("Build", PipelineJobStatus.FINISHED, "Build done");
            var pipeline = new Pipeline(new List<PipelineJobCommand>
            {
                analyze.Object,
                build.Object
            }, "pipeline");

            Assert.Throws<InvalidOperationException>(() => pipeline.CurrentCommand);

            pipeline.Execute();

            Assert.Same(build.Object, pipeline.CurrentCommand);
        }

        [Fact]
        public void A_Pipeline_Fails_When_A_Command_Fails()
        {
            var analyze = CreateCommand("Analyze", PipelineJobStatus.FAILED, "Analyze failed");
            var build = CreateCommand("Build", PipelineJobStatus.FINISHED, "Build done");
            var pipeline = new Pipeline(new List<PipelineJobCommand>
            {
                analyze.Object,
                build.Object
            }, "pipeline");

            pipeline.Execute();

            Assert.Equal(PipelineJobStatus.FAILED, pipeline.Status);
            analyze.Verify(c => c.Execute(), Times.Once);
            build.Verify(c => c.Execute(), Times.Never);
        }

        [Fact]
        public void Pipeline_Output_Collects_Command_Outputs()
        {
            var pipeline = new Pipeline(new List<PipelineJobCommand>
            {
                CreateCommand("Analyze", PipelineJobStatus.FINISHED, "Analyze done").Object,
                CreateCommand("Build", PipelineJobStatus.FINISHED, "Build done").Object
            }, "pipeline");

            pipeline.Execute();

            Assert.Equal(2, pipeline.PipelineOutput.Count);
            Assert.All(pipeline.PipelineOutput, output => Assert.False(string.IsNullOrWhiteSpace(output)));
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
