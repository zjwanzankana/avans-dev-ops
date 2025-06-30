using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Pipelines
{
    public class Pipeline
    {
        private string _name;
        private List<PipelineJobCommand> _commands;
        private PipelineJobStatus _status;

        private PipelineJobCommand _currentCommand;

        public Pipeline(List<PipelineJobCommand> commands, string name)
        {
            _commands = commands;
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
                throw new Exception("Can't execute pipeline without commands");
            }

            foreach (var command in _commands)
            {
                command.SetStatus(PipelineJobStatus.Queued);
            }

            _status = PipelineJobStatus.Running;
            foreach (var command in _commands)
            {
                _currentCommand = command;
                command.Execute();
                if (command.GetStatus() == PipelineJobStatus.FAILED)
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

        public List<string> PipelineOutput()
        { 
            List<string> commandOutputs = new List<string>();

            foreach (PipelineJobCommand c in _commands)
            {
                commandOutputs.Add(c.GetOutput());
            }

            return commandOutputs;
        }

        public PipelineJobStatus GetStatus()
        { 
            return _status;
        }

        public List<PipelineJobCommand> GetCommands()
        {
            if (_status == PipelineJobStatus.FINISHED || _status == PipelineJobStatus.FAILED)
            {
                return _commands;
            }

            throw new Exception("Can't get commands while pipeline is not done");
        }

        public PipelineJobCommand GetCurrentCommand()
        {
            if (_status == PipelineJobStatus.Off || _status == PipelineJobStatus.Queued)
            {
                throw new Exception("Can't get command when pipeline is not yet running");
            }
            return _currentCommand;
        } 

        public string GetName()
        {
            return _name;
        }

        public void SetName(string name)
        {
            _name = name;
        }
    }
}
