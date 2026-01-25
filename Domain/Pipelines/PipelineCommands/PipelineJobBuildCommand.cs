using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Pipelines.PipelineCommands
{
    public class PipelineJobBuildCommand : PipelineJobCommand
    {
        private bool _isSucces = true;

        private bool _buildWithDebugOn;

        public PipelineJobBuildCommand(string name, string command, bool buildWithDebugOn) : base(name, command)
        {
            _buildWithDebugOn = buildWithDebugOn;
        }

        public override void Execute()
        {
            Status = PipelineJobStatus.Running;

            Console.WriteLine($"Running command {Name} type {GetType().Name} with command {Command} BuildDebugOnStats {_buildWithDebugOn}");

            Status = PipelineJobStatus.Running;

            if (_isSucces)
            {
                Output = $"Sources succesfully retrieved BuildDebugOnStatus = {_buildWithDebugOn}";
                Status = PipelineJobStatus.FINISHED;
            }
            else
            {
                Output = $"Sources unsuccesfully retrieved BuildDebugOnStatus = {_buildWithDebugOn}";
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
