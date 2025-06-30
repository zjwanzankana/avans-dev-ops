using Domain.Backlogs;
using Domain.Developers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Sprints.SprintStates
{
    public class ScheduledState : SprintState
    {
        private readonly Sprint _sprint;
        public ScheduledState(Sprint sprint) : base(sprint)
        {
            _sprint = sprint;
        }

        public override void SetName(string name)
        {
            this._sprint.SetName(name);
        }

        public override void SetStartDate(DateTime startDate)
        {
            this._sprint.SetStartDate(startDate);
        }

        public override void SetEndDate(DateTime endDate)
        {
            this._sprint.SetEndDate(endDate);
        }

        public override void AddDeveloper(Developer developer)
        {
            this._sprint.AddDeveloper(developer);
        }

        public override void AddToSprintBacklog(BacklogItem backlogItem)
        {
            this._sprint.AddToSprintBacklog(backlogItem);
        }

        public override void NextState()
        {
            this._sprint.ChangeState(new InProgressState(this._sprint));
        }

        public override void PreviousState()
        {
            throw new Exception($"No previous state for Scheduled state");
        }

        public override void StartStateAction()
        {
            throw new Exception($"No action for Scheduled state");
        }

        public override ESprintStates GetSprintState()
        {
            return ESprintStates.Scheduled;
        }
    }
}
