using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Pipelines
{
    /// <summary>
    /// Abstract base class voor pipeline commands.
    /// Implementeert het Command design pattern voor verschillende pipeline acties.
    /// </summary>
    public abstract class PipelineJobCommand
    {
        private string _name;

        private string _command;

        private string _output;

        private PipelineJobStatus _status;

        protected PipelineJobCommand(string name, string command)
        {
            _name = name;
            _command = command;
            _status = PipelineJobStatus.Off;
        }

        public string Output
        {
            get => _output;
            protected set => _output = value;
        }

        public string Command => _command;

        public string Name => _name;

        public PipelineJobStatus Status
        {
            get => _status;
            internal set => _status = value;
        }

        public abstract void Execute();
    }
}
