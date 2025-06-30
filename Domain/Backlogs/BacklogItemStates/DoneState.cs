using Domain.Developers;
using System.Collections.Generic;

namespace Domain.Backlogs.BacklogItemStates
{
    public class DoneState : BacklogItemState
    {
        private readonly BacklogItem _backlogItem;
        public DoneState(BacklogItem backlogItem) : base(backlogItem)
        {
            _backlogItem = backlogItem;

            # warning Implement send every developer from backlogitem a notification that item is approved
            var developers = new List<Developer>();

          

        }

        public override EBacklogStates GetState()
        {
            return EBacklogStates.done;
        }

        public override void AddActivity(Activity activity)
        {
            throw new System.Exception("Can't add activity when backlogitem is done");
        }

        public override void NextState()
        {
            throw new System.Exception("can't go to next state when backlogitem is done");
        }

        public override void PreviousState()
        {
            throw new System.Exception("can't go to previous state when backlogitem is done");
        }

        public override void RemoveActivity(Activity activity)
        {
            throw new System.Exception("Can't remove activity when backlogitem is done");
        }

        public override void SetDescription(string description)
        {
            throw new System.Exception("Can't set description when backlogitem is done");
        }

        public override void SetEffort(int newEffort)
        {
            throw new System.Exception("Can't set effort when backlogitem is done");
        }

        public override void SetName(string newName)
        {
            throw new System.Exception("Can't set name when backlogitem is done");
        }
    }
}
