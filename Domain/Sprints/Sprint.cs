using Domain.Backlogs;
using Domain.Developers;
using Domain.Pipelines;
using Domain.Reports;
using Domain.Sprints.SprintStates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Domain.Sprints
{
    public abstract class Sprint
    {
        private readonly Project _project;
        private readonly List<BacklogItem> _backlogItems;

        private string _name;
        private DateTime _startDate;
        private DateTime _endDate;
        private SprintState _state;

        private readonly Developer _scrumMaster;
        private readonly List<Developer> _developers;

        private Pipeline _pipeline;


        public Sprint(Project project, string name, DateTime startDate, DateTime endDate, Developer scrumMaster, List<Developer> developers)
        {
            _project = project;
            _name = name;
            _startDate = startDate;
            _endDate = endDate;
            _scrumMaster = scrumMaster;
            _developers = developers;

            _backlogItems = new List<BacklogItem>();

            _state = new ScheduledState(this);
        }

        public void SetPipeline(Pipeline pipeline)
        {
            _pipeline = pipeline;
        }

        public Pipeline GetPipeline()
        {
            return _pipeline;
        }

        public void AddDeveloper(Developer developer)
        {
            _developers.Add(developer);
        }

        public void AddToSprintBacklog(BacklogItem backlogItem)
        {
            //Add backlogItem to sprintbacklog only when it is not already in the sprintbacklog
            if (!_backlogItems.Contains(backlogItem))
                backlogItem.SetSprint(this);
                _backlogItems.Add(backlogItem);
        }

        public void ChangeState(SprintState state)
        {
            this._state = state;
        }

        public List<BacklogItem> GetBacklogItems()
        {
            return this._backlogItems;
        }

        public List<Developer> GetDevelopers()
        {
            return this._developers;
        }

        public DateTime GetEndDate()
        {
            return this._endDate;
        }

        public string GetName()
        {
            return this._name;
        }

        public Project GetProject()
        {
            return this._project;
        }

        public Developer GetScrumMaster()
        {
            return this._scrumMaster;
        }

        public DateTime GetStartDate()
        {
            return this._startDate;
        }

        public SprintState GetState()
        {
            return this._state;
        }

        public void SetEndDate(DateTime endDate)
        {
            if (_pipeline == null || _pipeline.GetStatus() != PipelineJobStatus.Running)
            {
                _endDate = endDate;
            }
            else
            {
                throw new Exception("Can't change end date when pipeline is running");
            }
        }

        public void SetName(string name)
        {
            if (_pipeline ==null || _pipeline.GetStatus() != PipelineJobStatus.Running)
            {
                _name = name;
            }
            else
            {
                throw new Exception("Can't change end date when pipeline is running");
            }
        }

        public void SetStartDate(DateTime startDate)
        {
            if (_pipeline == null || _pipeline.GetStatus() != PipelineJobStatus.Running)
            {
                _startDate = startDate;
            }
            else
            {
                throw new Exception("Can't change end date when pipeline is running");
            }
        }

 
       

      
    }
}
