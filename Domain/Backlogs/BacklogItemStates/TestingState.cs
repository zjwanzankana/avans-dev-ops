using System;

namespace Domain.Backlogs.BacklogItemStates
{
    /// <summary>State pattern - ConcreteState 'Testing'. Een tester verifieert het item.</summary>
    public class TestingState : BacklogItemState
    {
        public TestingState(BacklogItem backlogItem) : base(backlogItem)
        {
        }

        public override EBacklogStates GetState() => EBacklogStates.testing;

        public override void AddActivity(Activity activity)
        {
            throw new InvalidOperationException("Can't add activity when backlogitem is in testing fase");
        }

        public override void RemoveActivity(Activity activity)
        {
            throw new InvalidOperationException("Can't remove activity when backlogitem is in testing fase");
        }

        public override void CompleteTesting()
        {
            if (!BacklogItem.AllActivitiesDone())
            {
                throw new InvalidOperationException("Can't go to next state when not all activities are done");
            }

            BacklogItem.ChangeState(new TestedState(BacklogItem));
        }

        public override void RejectByTester()
        {
            BacklogItem.NotifyScrumMaster(
                $"Tester found a defect in backlog item '{BacklogItem.Name}'; it went back to todo.");

            BacklogItem.ChangeState(new TodoState(BacklogItem));
        }
    }
}
