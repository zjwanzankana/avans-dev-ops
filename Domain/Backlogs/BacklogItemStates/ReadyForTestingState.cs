using System;

namespace Domain.Backlogs.BacklogItemStates
{
    /// <summary>State pattern - ConcreteState 'ReadyForTesting'. Wacht tot een tester het oppakt.</summary>
    public class ReadyForTestingState : BacklogItemState
    {
        public ReadyForTestingState(BacklogItem backlogItem) : base(backlogItem)
        {
        }

        public override EBacklogStates GetState() => EBacklogStates.readyfortesting;

        public override void AddActivity(Activity activity)
        {
            throw new InvalidOperationException("Can't add activity when ready to test");
        }

        public override void StartTesting()
        {
            BacklogItem.ChangeState(new TestingState(BacklogItem));
        }

        public override void RejectByTester()
        {
            BacklogItem.NotifyScrumMaster(
                $"Backlog item '{BacklogItem.Name}' was marked ready but is not finished and went back to todo.");

            BacklogItem.ChangeState(new TodoState(BacklogItem));
        }
    }
}
