using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Notifications
{
    public interface IBacklogObservable
    {
        void Register(IBacklogObserver observer);
        void UnRegister(IBacklogObserver observer);
        void Notify();
    }
}
