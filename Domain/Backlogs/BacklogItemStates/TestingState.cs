using System;

namespace Domain.Backlogs.BacklogItemStates
{
    public class TestingState : BacklogItemState
    {
        public TestingState(BacklogItem backlogItem) : base(backlogItem)
        {
        }

        public override void AddActivity(Activity activity)
        {
            throw new InvalidOperationException("Can't add activity when backlogitem is in testing fase");
        }

        public override void RemoveActivity(Activity activity)
        {
            throw new InvalidOperationException("Can't remove activity when backlogitem is in testing fase");
        }

        public override EBacklogStates GetState()
        {
            return EBacklogStates.testing;
        }

        public override void NextState()
        {
            //only allow next state when all activities are done
            if (BacklogItem.AllActivitiesDone())
            {
                BacklogItem.ChangeState(new TestedState(BacklogItem));
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
