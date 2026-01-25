using System;

namespace Domain.Backlogs.BacklogItemStates
{
    public class DoneState : BacklogItemState
    {
        public DoneState(BacklogItem backlogItem) : base(backlogItem)
        {
            // Notification wordt al via Observer pattern afgehandeld in BacklogItem.ChangeState
        }

        public override EBacklogStates GetState()
        {
            return EBacklogStates.done;
        }

        public override void AddActivity(Activity activity)
        {
            throw new InvalidOperationException("Can't add activity when backlogitem is done");
        }

        public override void NextState()
        {
            throw new InvalidOperationException("can't go to next state when backlogitem is done");
        }

        public override void PreviousState()
        {
            throw new InvalidOperationException("can't go to previous state when backlogitem is done");
        }

        public override void RemoveActivity(Activity activity)
        {
            throw new InvalidOperationException("Can't remove activity when backlogitem is done");
        }

        public override void SetDescription(string description)
        {
            throw new InvalidOperationException("Can't set description when backlogitem is done");
        }

        public override void SetEffort(int newEffort)
        {
            throw new InvalidOperationException("Can't set effort when backlogitem is done");
        }

        public override void SetName(string newName)
        {
            throw new InvalidOperationException("Can't set name when backlogitem is done");
        }
    }
}
