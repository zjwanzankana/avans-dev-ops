

namespace Domain.Backlogs.BacklogItemStates
{
    public class TodoState : BacklogItemState
    {
        private readonly BacklogItem _backlogItem;
        public TodoState(BacklogItem backlogItem) : base(backlogItem)
        {
            // Additional constructor logic if needed
            _backlogItem = backlogItem;
        }

        public override EBacklogStates GetState()
        {
            return EBacklogStates.todo;
        }
        public override void NextState()
        {
            if (_backlogItem.GetAssignedDeveloper() == null)
            {
                throw new System.Exception("Can't go to doing state when no developer is assigned");
            }

            _backlogItem.ChangeState(new DoingState(_backlogItem));
        }

        public override void PreviousState()
        {
            //throw new system error because we can't go back from todo state
            throw new System.Exception("Can't go back from todo state");
        }

        
    }
}
