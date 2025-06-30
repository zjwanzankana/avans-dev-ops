using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Backlogs;

namespace Domain.Backlogs.BacklogItemStates
{
    public abstract class BacklogItemState
    {
        private readonly BacklogItem _backlogItem;

        public BacklogItemState(BacklogItem backlogItem)
        {
            _backlogItem = backlogItem;
        }
        public virtual void AddActivity(Activity activity)
        {
            _backlogItem.AddActivity(activity);
        }

        public virtual void RemoveActivity(Activity activity)
        {
            _backlogItem.RemoveActivity(activity);
        }

        public virtual void SetDescription(string description)
        {
            _backlogItem.SetDescription(description);
        }

        public virtual void SetEffort(int newEffort)
        {
            _backlogItem.SetEffort(newEffort);
        }

        public virtual void SetName(string newName)
        {
            _backlogItem.SetName(newName);
        }

        public BacklogItem GetBacklogItem()
        {
            return _backlogItem;
        }

        public abstract EBacklogStates GetState();

        public abstract void NextState();
        public abstract void PreviousState();
    }
}
