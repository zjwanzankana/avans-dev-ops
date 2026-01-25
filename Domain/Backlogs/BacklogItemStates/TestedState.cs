using System;

namespace Domain.Backlogs.BacklogItemStates
{
    public class TestedState : BacklogItemState
    {
        public TestedState(BacklogItem backlogItem) : base(backlogItem)
        {
            // Lead dev checks DOD
        }

        public override EBacklogStates GetState()
        {
            return EBacklogStates.tested;
        }
        public override void NextState()
        {
            //only allow next state when all activities are done
            if (BacklogItem.AllActivitiesDone())
            {
                BacklogItem.ChangeState(new DoneState(BacklogItem));
            }
            else
            {
                throw new InvalidOperationException("Can't go to next state when not all activities are done");
            }
        }

        public override void PreviousState()
        {
            BacklogItem.ChangeState(new ReadyForTestingState(BacklogItem));
        }
    }
}
