using System;

namespace Domain.Backlogs.BacklogItemStates
{
    /// <summary>State pattern - ConcreteState 'TodoState'. Startfase van elk item in de sprint.</summary>
    public class TodoState : BacklogItemState
    {
        public TodoState(BacklogItem backlogItem) : base(backlogItem)
        {
        }

        public override EBacklogStates GetState() => EBacklogStates.todo;

        public override void BeginWork()
        {
            if (BacklogItem.AssignedDeveloper == null)
            {
                throw new InvalidOperationException("Can't go to doing state when no developer is assigned");
            }

            BacklogItem.ChangeState(new DoingState(BacklogItem));
        }
    }
}
