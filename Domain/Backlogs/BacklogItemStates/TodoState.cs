using System;

namespace Domain.Backlogs.BacklogItemStates
{
    public class TodoState : BacklogItemState
    {
        public TodoState(BacklogItem backlogItem) : base(backlogItem)
        {
            // Additional constructor logic if needed
        }

        public override EBacklogStates GetState()
        {
            return EBacklogStates.todo;
        }
        public override void NextState()
        {
            if (BacklogItem.AssignedDeveloper == null)
            {
                throw new InvalidOperationException("Can't go to doing state when no developer is assigned");
            }

            BacklogItem.ChangeState(new DoingState(BacklogItem));
        }

        public override void PreviousState()
        {
            //throw new system error because we can't go back from todo state
            throw new InvalidOperationException("Can't go back from todo state");
        }

        
    }
}
