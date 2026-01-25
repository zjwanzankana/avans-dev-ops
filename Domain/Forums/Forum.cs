using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Domain.Forums
{
    public class Forum
    {
        private readonly List<Thread> _threads;

        public Forum()
        {
            _threads = new List<Thread>();
        }

        public void AddThread(Thread thread)
        {
            ArgumentNullException.ThrowIfNull(thread);

            if (thread.Activity.Status == Backlogs.ActivityStatus.Done)
            { 
                throw new InvalidOperationException("Can't add a thread to a done activity");
            }

            if (string.IsNullOrWhiteSpace(thread.Title))
            { 
                throw new InvalidOperationException("Can't add a thread without a title");
            }

            _threads.Add(thread);
        }

        public void RemoveThread(Thread thread)
        {

            if (!_threads.Contains(thread))
            {
                throw new InvalidOperationException("Can't remove a thread that doesn't exist");
            }

            _threads.Remove(thread);
        }

        public ReadOnlyCollection<Thread> Threads => _threads.AsReadOnly();
    }
}
