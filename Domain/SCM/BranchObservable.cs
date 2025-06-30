using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.SCM
{
    public interface BranchObservable
    {
        void Register(BranchObserver observer);
        void UnRegister(BranchObserver observer);

        void Notify();
    }
}
