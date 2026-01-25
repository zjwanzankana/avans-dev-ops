using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Domain.Pipelines
{
    public class Pipeline
    {
        private string _name;
        private List<PipelineJobCommand> _commands;
        private PipelineJobStatus _status;

        private PipelineJobCommand _currentCommand;

        public Pipeline(IReadOnlyList<PipelineJobCommand> commands, string name)
        {
            _commands = commands == null ? new List<PipelineJobCommand>() : new List<PipelineJobCommand>(commands);
            _name = name;
        }

        public void SetStatus(PipelineJobStatus status)
        { 
            _status = status;
        }

        public void Execute()
        {
            if(_commands.Count == 0)
            {
                throw new InvalidOperationException("Can't execute pipeline without commands");
            }

            foreach (var command in _commands)
            {
                command.Status = PipelineJobStatus.Queued;
            }

            _status = PipelineJobStatus.Running;
            foreach (var command in _commands)
            {
                _currentCommand = command;
                command.Execute();
                if (command.Status == PipelineJobStatus.FAILED)
                {
                    _status = PipelineJobStatus.FAILED;

                    //tell here it failed

                    return;
                }
                //tell here single job succes
            }
            _status = PipelineJobStatus.FINISHED;
            //tell here whole pipeline succeeded
        }

        public ReadOnlyCollection<string> PipelineOutput
        {
            get
            {
                List<string> commandOutputs = new List<string>();

                foreach (PipelineJobCommand c in _commands)
                {
                    commandOutputs.Add(c.Output);
                }

                return new ReadOnlyCollection<string>(commandOutputs);
            }
        }

        public PipelineJobStatus Status => _status;

        public ReadOnlyCollection<PipelineJobCommand> Commands
        {
            get
            {
                if (_status == PipelineJobStatus.FINISHED || _status == PipelineJobStatus.FAILED)
                {
                    return _commands.AsReadOnly();
                }

                throw new InvalidOperationException("Can't get commands while pipeline is not done");
            }
        }

        public PipelineJobCommand CurrentCommand
        {
            get
            {
                if (_status == PipelineJobStatus.Off || _status == PipelineJobStatus.Queued)
                {
                    throw new InvalidOperationException("Can't get command when pipeline is not yet running");
                }
                return _currentCommand;
            }
        }

        public string Name => _name;

        public void SetName(string name)
        {
            _name = name;
        }
    }
}
