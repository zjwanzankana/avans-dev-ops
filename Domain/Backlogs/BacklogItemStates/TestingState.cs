namespace Domain.Backlogs.BacklogItemStates
{
    public class TestingState : BacklogItemState
    {
        private readonly BacklogItem _backlogItem;
        public TestingState(BacklogItem backlogItem) : base(backlogItem)
        {
            _backlogItem = backlogItem;
        }

        public override void AddActivity(Activity activity)
        {
            throw new System.Exception("Can't add activity when backlogitem is in testing fase");
        }

        public override void RemoveActivity(Activity activity)
        {
            throw new System.Exception("Can't remove activity when backlogitem is in testing fase");
        }

        public override EBacklogStates GetState()
        {
            return EBacklogStates.testing;
        }

        public override void NextState()
        {
            //only allow next state when all activities are done
            if (_backlogItem.AllActivitiesDone())
            {
                _backlogItem.ChangeState(new TestedState(_backlogItem));
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
