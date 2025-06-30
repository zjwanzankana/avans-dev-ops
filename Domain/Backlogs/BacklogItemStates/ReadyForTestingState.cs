using Domain.Backlogs;
using Domain.Developers;
using Domain.Notifications;
using System;

namespace Domain.Backlogs.BacklogItemStates
{
    public class ReadyForTestingState : BacklogItemState
    {
        private readonly BacklogItem _backlogItem;
        public ReadyForTestingState(BacklogItem backlogItem) : base(backlogItem)
        {
            _backlogItem = backlogItem;

           
        }

        public override EBacklogStates GetState()
        {
            return EBacklogStates.readyfortesting;
        }

        public override void AddActivity(Activity activity)
        { 
            throw new Exception("Can't add activity when ready to test");
        }

        public override void NextState()
        {
            // Only allow next state when all activities are done or active
            if (_backlogItem.AllActivitiesDoneOrActive())
            {
                _backlogItem.ChangeState(new TestingState(_backlogItem));
            }
            else
            {
                throw new Exception("Can't go to next state when not all activities are done or active");
            }
        }

        public override void PreviousState()
        {
            var scrumMaster = _backlogItem.GetSprint().GetScrumMaster();
            scrumMaster.SendNotification($"Hello {scrumMaster.GetName()} something is wrong with backlogitem: {_backlogItem.GetName()}");

            _backlogItem.ChangeState(new TodoState(_backlogItem));
        }
    }
}
