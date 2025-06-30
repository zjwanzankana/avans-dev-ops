namespace Domain.Backlogs.BacklogItemStates
{
    public class DoingState : BacklogItemState
    {
        private readonly BacklogItem _backlogItem;

        public DoingState(BacklogItem backlogItem) : base(backlogItem)
        {
            // Additional constructor logic if needed
            _backlogItem = backlogItem;
        }

        public override EBacklogStates GetState()
        {
            return EBacklogStates.doing;
        }

        public override void NextState()
        {
            //Only allow next state when all activities are done
            if (_backlogItem.AllActivitiesDone())
            {


                _backlogItem.ChangeState(new ReadyForTestingState(_backlogItem));
            }
            else
            {
                throw new System.Exception("Can't go to next state when not all activities are done");
            }
        }

        public override void PreviousState()
        {
            _backlogItem.ChangeState(new TodoState(_backlogItem));
        }
    }
}
