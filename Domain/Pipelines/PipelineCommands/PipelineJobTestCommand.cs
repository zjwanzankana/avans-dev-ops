using System;

namespace Domain.Pipelines.PipelineCommands
{
    public class PipelineJobTestCommand : PipelineJobCommand
    {
        private bool _isSucces = true;

        public PipelineJobTestCommand(string name, string command) : base(name, command)
        {

        }

        public override void Execute()
        {
            Status = PipelineJobStatus.Running;

            Console.WriteLine($"Running command {Name} type {GetType().Name} with command {Command}");

            Status = PipelineJobStatus.Running;

            if (_isSucces)
            {
                Output = "Sources succesfully retrieved";
                Status = PipelineJobStatus.FINISHED;
            }
            else
            {
                Output = "Sources unsuccesfully retrieved";
                Status = PipelineJobStatus.FAILED;
            }
        }

        //Just for testing purposes
        public void MakePipelineFail()
        {
            _isSucces = false;
        }
    }
}
