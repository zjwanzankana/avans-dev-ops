using System;

namespace Domain.Backlogs.BacklogItemStates
{
    /// <summary>
    /// State pattern - ConcreteState 'Done'. Eindfase, maar geen 'dead end':
    /// het item kan heropend worden (Reopen) naar aanleiding van een discussie.
    /// </summary>
    public class DoneState : BacklogItemState
    {
        public DoneState(BacklogItem backlogItem) : base(backlogItem)
        {
        }

        public override EBacklogStates GetState() => EBacklogStates.done;

        public override void Reopen()
        {
            BacklogItem.ChangeState(new TodoState(BacklogItem));
        }

        public override void AddActivity(Activity activity)
            => throw new InvalidOperationException("Can't add activity when backlogitem is done");

        public override void RemoveActivity(Activity activity)
            => throw new InvalidOperationException("Can't remove activity when backlogitem is done");

        public override void SetDescription(string description)
            => throw new InvalidOperationException("Can't set description when backlogitem is done");

        public override void SetEffort(int newEffort)
            => throw new InvalidOperationException("Can't set effort when backlogitem is done");

        public override void SetName(string newName)
            => throw new InvalidOperationException("Can't set name when backlogitem is done");
    }
}
