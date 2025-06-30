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

        private bool _buildWithDebugOn = false;

        public PipelineJobBuildCommand(string name, string command, bool buildWithDebugOn) : base(name, command)
        {
            _buildWithDebugOn = buildWithDebugOn;
        }

        public override void Execute()
        {
            base.SetStatus(PipelineJobStatus.Running);

            Console.WriteLine($"Running command {base.GetName()} type {this.GetType().Name} with command {base.GetCommand()} BuildDebugOnStats {_buildWithDebugOn}");

            base.SetStatus(PipelineJobStatus.Running);

            if (_isSucces)
            {
                base.SetOutput($"Sources succesfully retrieved BuildDebugOnStatus = {_buildWithDebugOn}");
                base.SetStatus(PipelineJobStatus.FINISHED);
            }
            else
            {
                base.SetOutput($"Sources unsuccesfully retrieved BuildDebugOnStatus = {_buildWithDebugOn}");
                base.SetStatus(PipelineJobStatus.FAILED);
            }
        }

        //Just for testing purposes
        public void MakePipelineFail()
        {
            _isSucces = false;
        }
    }
}
