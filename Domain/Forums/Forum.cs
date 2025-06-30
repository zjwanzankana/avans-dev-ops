using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            if (thread.GetActivity().GetStatus() == Backlogs.ActivityStatus.Done)
            { 
                throw new Exception("Can't add a thread to a done activity");
            }

            if (string.IsNullOrWhiteSpace(thread.GetTitle()))
            { 
                throw new Exception("Can't add a thread without a title");
            }

            _threads.Add(thread);
        }

        public void RemoveThread(Thread thread)
        {

            if (!_threads.Contains(thread))
            {
                throw new Exception("Can't remove a thread that doesn't exist");
            }

            _threads.Remove(thread);
        }

        public List<Thread> GetThreads()
        {
            return _threads;
        }
    }
}
