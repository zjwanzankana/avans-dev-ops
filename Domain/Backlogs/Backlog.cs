using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Backlogs
{
    public  class Backlog
    {
        private readonly List<BacklogItem> _backlogItems;
        private readonly Project _project;

        public Backlog(Project project)
        {
            _project = project;
            _backlogItems = new List<BacklogItem>();
        }

        public void AddBacklogItem(BacklogItem backlogItem)
        {
            if (_backlogItems.Contains(backlogItem))
                throw new Exception("Can't add the same backlogItem twice");

            _backlogItems.Add(backlogItem);
        }

        public void RemoveBacklogItem(BacklogItem backlogItem)
        { 
            if(!_backlogItems.Contains(backlogItem))
            {
                throw new Exception("BacklogItem not found");
            }

            _backlogItems.Remove(backlogItem);
        }

        public List<BacklogItem> GetBacklogItems()
        {
            return _backlogItems;
        }

        public Project GetProject()
        {
            return _project;
        }
    }
}
