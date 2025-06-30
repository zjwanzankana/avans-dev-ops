using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Pipelines.PipelineCommands
{
    public class PipelineJobSourcesCommand : PipelineJobCommand
    {
        private bool isSucces = true;

        public PipelineJobSourcesCommand(string name, string command) : base(name, command)
        {

        }

        public override void Execute()
        {
            base.SetStatus(PipelineJobStatus.Running);

            Console.WriteLine($"Running command {base.GetName()} type {this.GetType().Name} with command {base.GetCommand()}");

            base.SetStatus(PipelineJobStatus.Running);

            if (isSucces)
            {
                base.SetOutput("Sources succesfully retrieved");
                base.SetStatus(PipelineJobStatus.FINISHED);
            }
            else
            {
                base.SetOutput("Sources unsuccesfully retrieved");
                base.SetStatus(PipelineJobStatus.FAILED);
            }
        }

        //Just for testing purposes
        public void MakePipelineFail()
        {
            isSucces = false;
        }
    }
}
