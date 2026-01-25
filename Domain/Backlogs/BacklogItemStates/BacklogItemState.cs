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

        protected BacklogItemState(BacklogItem backlogItem)
        {
            _backlogItem = backlogItem;
        }
        public virtual void AddActivity(Activity activity)
        {
            BacklogItem.AddActivity(activity);
        }

        public virtual void RemoveActivity(Activity activity)
        {
            BacklogItem.RemoveActivity(activity);
        }

        public virtual void SetDescription(string description)
        {
            BacklogItem.Description = description;
        }

        public virtual void SetEffort(int newEffort)
        {
            BacklogItem.Effort = newEffort;
        }

        public virtual void SetName(string newName)
        {
            BacklogItem.Name = newName;
        }

        public BacklogItem BacklogItem => _backlogItem;

        public abstract EBacklogStates GetState();

        public abstract void NextState();
        public abstract void PreviousState();
    }
}
