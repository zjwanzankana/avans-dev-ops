namespace Domain.Backlogs.BacklogItemStates
{
    public class TestedState : BacklogItemState
    {
        private readonly BacklogItem _backlogItem;

        public TestedState(BacklogItem backlogItem) : base(backlogItem)
        {
            // Lead dev checks DOD
            _backlogItem = backlogItem;
        }

        public override EBacklogStates GetState()
        {
            return EBacklogStates.tested;
        }
        public override void NextState()
        {
            //only allow next state when all activities are done
            if (_backlogItem.AllActivitiesDone())
            {
                _backlogItem.ChangeState(new DoneState(_backlogItem));
            }
            else
            {
                throw new System.Exception("Can't go to next state when not all activities are done");
            }
        }

        public override void PreviousState()
        {
            _backlogItem.ChangeState(new ReadyForTestingState(_backlogItem));
        }
    }
}
