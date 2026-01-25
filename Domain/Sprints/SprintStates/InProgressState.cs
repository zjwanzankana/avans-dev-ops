using System;

namespace Domain.Sprints.SprintStates
{
    internal sealed class InProgressState : SprintState
    {
        private readonly Sprint _sprint;
        public InProgressState(Sprint sprint) : base(sprint)
        {
            _sprint = sprint;
        }

        public override void NextState()
        {
            this._sprint.ChangeState(new FinishedState(_sprint));
            this._sprint.State.StartStateAction();
        }

        public override void PreviousState()
        {
            this._sprint.ChangeState(new ScheduledState(_sprint));
        }

        public override void StartStateAction()
        {
            throw new InvalidOperationException("No action for Scheduled state");
        }

        public override ESprintStates GetSprintState()
        {
            return ESprintStates.InProgress;
        }
    }
}
