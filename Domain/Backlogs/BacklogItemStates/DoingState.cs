using System;

namespace Domain.Backlogs.BacklogItemStates
{
    public class DoingState : BacklogItemState
    {
        public DoingState(BacklogItem backlogItem) : base(backlogItem)
        {
            // Additional constructor logic if needed
        }

        public override EBacklogStates GetState()
        {
            return EBacklogStates.doing;
        }

        public override void NextState()
        {
            //Only allow next state when all activities are done
            if (BacklogItem.AllActivitiesDone())
            {


                BacklogItem.ChangeState(new ReadyForTestingState(BacklogItem));
            }
            else
            {
                throw new InvalidOperationException("Can't go to next state when not all activities are done");
            }
        }

        public override void PreviousState()
        {
            BacklogItem.ChangeState(new TodoState(BacklogItem));
        }
    }
}
