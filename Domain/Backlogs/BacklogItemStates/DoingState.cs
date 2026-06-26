using System;

namespace Domain.Backlogs.BacklogItemStates
{
    /// <summary>State pattern - ConcreteState 'Doing'. Developer werkt aan het item.</summary>
    public class DoingState : BacklogItemState
    {
        public DoingState(BacklogItem backlogItem) : base(backlogItem)
        {
        }

        public override EBacklogStates GetState() => EBacklogStates.doing;

        public override void SubmitForTesting()
        {
            if (!BacklogItem.AllActivitiesDone())
            {
                throw new InvalidOperationException("Can't go to next state when not all activities are done");
            }

            BacklogItem.ChangeState(new ReadyForTestingState(BacklogItem));

            // Business rule (FR_N1): zodra een item 'ready for testing' is, krijgen testers een notificatie.
            BacklogItem.NotifyTesters($"Backlog item '{BacklogItem.Name}' is ready for testing.");
        }
    }
}
