using Domain.Pipelines;
using Domain.Pipelines.PipelineCommands;

namespace DomainTests
{
    public class PipelineCommandTests
    {
        [Fact]
        public void Pipeline_Commands_Finish_And_Provide_Output()
        {
            var commands = new PipelineJobCommand[]
            {
                new PipelineJobAnalyzeCommand("Analyze", "analyze"),
                new PipelineJobBuildCommand("Build", "build", false),
                new PipelineJobDeployCommand("Deploy", "deploy"),
                new PipelineJobPackageCommand("Package", "package"),
                new PipelineJobSourcesCommand("Sources", "sources"),
                new PipelineJobTestCommand("Test", "test"),
                new PipelineJobUtilityCommand("Utility", "utility")
            };

            foreach (var command in commands)
            {
                command.Execute();

                Assert.Equal(PipelineJobStatus.FINISHED, command.Status);
                Assert.False(string.IsNullOrWhiteSpace(command.Output));
            }
        }

        [Fact]
        public void Pipeline_Commands_Can_Fail()
        {
            var analyze = new PipelineJobAnalyzeCommand("Analyze", "analyze");
            analyze.MakePipelineFail();
            analyze.Execute();
            Assert.Equal(PipelineJobStatus.FAILED, analyze.Status);

            var build = new PipelineJobBuildCommand("Build", "build", true);
            build.MakePipelineFail();
            build.Execute();
            Assert.Equal(PipelineJobStatus.FAILED, build.Status);

            var deploy = new PipelineJobDeployCommand("Deploy", "deploy");
            deploy.MakePipelineFail();
            deploy.Execute();
            Assert.Equal(PipelineJobStatus.FAILED, deploy.Status);

            var package = new PipelineJobPackageCommand("Package", "package");
            package.MakePipelineFail();
            package.Execute();
            Assert.Equal(PipelineJobStatus.FAILED, package.Status);

            var sources = new PipelineJobSourcesCommand("Sources", "sources");
            sources.MakePipelineFail();
            sources.Execute();
            Assert.Equal(PipelineJobStatus.FAILED, sources.Status);

            var test = new PipelineJobTestCommand("Test", "test");
            test.MakePipelineFail();
            test.Execute();
            Assert.Equal(PipelineJobStatus.FAILED, test.Status);

            var utility = new PipelineJobUtilityCommand("Utility", "utility");
            utility.MakePipelineFail();
            utility.Execute();
            Assert.Equal(PipelineJobStatus.FAILED, utility.Status);
        }
    }
}
