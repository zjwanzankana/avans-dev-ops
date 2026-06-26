using System;

namespace Domain.Backlogs.BacklogItemStates
{
    /// <summary>State pattern - ConcreteState 'Tested'. (Lead) developer controleert de Definition of Done.</summary>
    public class TestedState : BacklogItemState
    {
        public TestedState(BacklogItem backlogItem) : base(backlogItem)
        {
        }

        public override EBacklogStates GetState() => EBacklogStates.tested;

        public override void Approve()
        {
            if (!BacklogItem.AllActivitiesDone())
            {
                throw new InvalidOperationException("Can't go to done when not all activities are done");
            }

            BacklogItem.ChangeState(new DoneState(BacklogItem));
        }

        public override void RejectByLeadDeveloper()
        {
            // DoD niet gehaald: terug naar ready for testing, de tester test opnieuw.
            BacklogItem.ChangeState(new ReadyForTestingState(BacklogItem));
        }
    }
}
