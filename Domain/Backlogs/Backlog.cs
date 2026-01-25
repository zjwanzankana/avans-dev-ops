using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Domain.Backlogs
{
    public class Backlog
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
                throw new InvalidOperationException("Can't add the same backlogItem twice");

            _backlogItems.Add(backlogItem);
        }

        public void RemoveBacklogItem(BacklogItem backlogItem)
        { 
            if(!_backlogItems.Contains(backlogItem))
            {
                throw new KeyNotFoundException("BacklogItem not found");
            }

            _backlogItems.Remove(backlogItem);
        }

        public ReadOnlyCollection<BacklogItem> BacklogItems => _backlogItems.AsReadOnly();

        public Project Project => _project;
    }
}
