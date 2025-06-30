using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Pipelines
{
    public abstract class PipelineJobCommand
    {
        private string _name;

        private string _command;

        private string _output;

        private PipelineJobStatus _status;

        public PipelineJobCommand(string name, string command)
        {
            _name = name;
            _command = command;
            _status = PipelineJobStatus.Off;
        }

        public string GetOutput()
        { 
            return _output;
        }

        public void SetOutput(string output)
        {
            _output = output;
        }

        public string GetCommand()
        {
            return _command;
        }

        public string GetName()
        {
            return _name;
        }

        public PipelineJobStatus GetStatus()
        {
            return _status;
        }

        public void SetStatus(PipelineJobStatus status)
        {
            _status = status;
        }

        public abstract void Execute();
    }
}
