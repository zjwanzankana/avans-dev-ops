using System;

namespace Domain.Backlogs.BacklogItemStates
{
    public class ReadyForTestingState : BacklogItemState
    {
        public ReadyForTestingState(BacklogItem backlogItem) : base(backlogItem)
        {
        }

        public override EBacklogStates GetState()
        {
            return EBacklogStates.readyfortesting;
        }

        public override void AddActivity(Activity activity)
        { 
            throw new InvalidOperationException("Can't add activity when ready to test");
        }

        public override void NextState()
        {
            // Only allow next state when all activities are done or active
            if (BacklogItem.AllActivitiesDoneOrActive())
            {
                BacklogItem.ChangeState(new TestingState(BacklogItem));
            }
            else
            {
                throw new InvalidOperationException("Can't go to next state when not all activities are done or active");
            }
        }

        public override void PreviousState()
        {
            var scrumMaster = BacklogItem.Sprint.ScrumMaster;
            scrumMaster.SendNotification($"Hello {scrumMaster.Name} something is wrong with backlogitem: {BacklogItem.Name}");

            BacklogItem.ChangeState(new TodoState(BacklogItem));
        }
    }
}
