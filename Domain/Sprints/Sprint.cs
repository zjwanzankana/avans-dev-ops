using Domain.Backlogs;
using Domain.Developers;
using Domain.Pipelines;
using Domain.Sprints.SprintStates;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

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


        protected Sprint(Project project, string name, DateTime startDate, DateTime endDate, Developer scrumMaster, IReadOnlyList<Developer> developers)
        {
            _project = project;
            _name = name;
            _startDate = startDate;
            _endDate = endDate;
            _scrumMaster = scrumMaster;
            _developers = developers == null ? new List<Developer>() : new List<Developer>(developers);

            _backlogItems = new List<BacklogItem>();

            _state = new ScheduledState(this);
        }

        public void SetPipeline(Pipeline pipeline)
        {
            _pipeline = pipeline;
        }

        public Pipeline Pipeline => _pipeline;

        public void AddDeveloper(Developer developer)
        {
            _developers.Add(developer);
        }

        public void AddToSprintBacklog(BacklogItem backlogItem)
        {
            ArgumentNullException.ThrowIfNull(backlogItem);

            //Add backlogItem to sprintbacklog only when it is not already in the sprintbacklog
            if (!_backlogItems.Contains(backlogItem))
            {
                backlogItem.Sprint = this;
                _backlogItems.Add(backlogItem);
            }
        }

        public void ChangeState(SprintState state)
        {
            this._state = state;
        }

        public ReadOnlyCollection<BacklogItem> BacklogItems => _backlogItems.AsReadOnly();

        public ReadOnlyCollection<Developer> Developers => _developers.AsReadOnly();

        public DateTime EndDate => _endDate;

        public string Name => _name;

        public Project Project => _project;

        public Developer ScrumMaster => _scrumMaster;

        public DateTime StartDate => _startDate;

        public SprintState State => _state;

        public void SetEndDate(DateTime endDate)
        {
            if (_pipeline == null || _pipeline.Status != PipelineJobStatus.Running)
            {
                _endDate = endDate;
            }
            else
            {
                throw new InvalidOperationException("Can't change end date when pipeline is running");
            }
        }

        public void SetName(string name)
        {
            if (_pipeline == null || _pipeline.Status != PipelineJobStatus.Running)
            {
                _name = name;
            }
            else
            {
                throw new InvalidOperationException("Can't change end date when pipeline is running");
            }
        }

        public void SetStartDate(DateTime startDate)
        {
            if (_pipeline == null || _pipeline.Status != PipelineJobStatus.Running)
            {
                _startDate = startDate;
            }
            else
            {
                throw new InvalidOperationException("Can't change end date when pipeline is running");
            }
        }

 
       

      
    }
}
