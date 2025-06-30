using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Sprints.SprintStates
{
    internal class InProgressState : SprintState
    {
        private readonly Sprint _sprint;
        public InProgressState(Sprint sprint) : base(sprint)
        {
            _sprint = sprint;
        }

        public override void NextState()
        {
            this._sprint.ChangeState(new FinishedState(_sprint));
            this._sprint.GetState().StartStateAction();
        }

        public override void PreviousState()
        {
            this._sprint.ChangeState(new ScheduledState(_sprint));
        }

        public override void StartStateAction()
        {
            throw new Exception($"No action for Scheduled state");
        }

        public override ESprintStates GetSprintState()
        {
            return ESprintStates.InProgress;
        }
    }
}
