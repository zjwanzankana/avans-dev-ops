using Domain.Pipelines;
using Domain.Pipelines.PipelineCommands;
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
                new PipelineJobAnalyzeCommand("Analyze", "analyze")
            }, "pipeline");

            Assert.Throws<InvalidOperationException>(() => pipeline.Commands);

            pipeline.Execute();

            Assert.Single(pipeline.Commands);
        }

        [Fact]
        public void Current_Command_Is_Available_After_Execution()
        {
            var pipeline = new Pipeline(new List<PipelineJobCommand>
            {
                new PipelineJobAnalyzeCommand("Analyze", "analyze"),
                new PipelineJobBuildCommand("Build", "build", false)
            }, "pipeline");

            Assert.Throws<InvalidOperationException>(() => pipeline.CurrentCommand);

            pipeline.Execute();

            Assert.Equal("Build", pipeline.CurrentCommand.Name);
        }

        [Fact]
        public void A_Pipeline_Fails_When_A_Command_Fails()
        {
            var analyze = new PipelineJobAnalyzeCommand("Analyze", "analyze");
            analyze.MakePipelineFail();
            var pipeline = new Pipeline(new List<PipelineJobCommand>
            {
                analyze,
                new PipelineJobBuildCommand("Build", "build", false)
            }, "pipeline");

            pipeline.Execute();

            Assert.Equal(PipelineJobStatus.FAILED, pipeline.Status);
        }

        [Fact]
        public void Pipeline_Output_Collects_Command_Outputs()
        {
            var pipeline = new Pipeline(new List<PipelineJobCommand>
            {
                new PipelineJobAnalyzeCommand("Analyze", "analyze"),
                new PipelineJobBuildCommand("Build", "build", false)
            }, "pipeline");

            pipeline.Execute();

            Assert.Equal(2, pipeline.PipelineOutput.Count);
            Assert.All(pipeline.PipelineOutput, output => Assert.False(string.IsNullOrWhiteSpace(output)));
        }
    }
}
