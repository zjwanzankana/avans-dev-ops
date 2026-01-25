using System;
using System.Collections.Generic;

namespace Domain.SCM
{
    public class Branch : IBranchObservable
    {
        private string _name;
        private DateTime _changeDate;
        private Code _code;

        private readonly List<IBranchObserver> _observers;

        public Branch(string name)
        {
            this._name = name;
            this._changeDate = DateTime.Now;
            this._code = new Code("");
            this._observers = new List<IBranchObserver>();
        }

        public string Name => _name;

        public DateTime ChangeDate => _changeDate;

        public Code Code => _code;

        public void PushCommit(Commit commit)
        {
            // Add commit logic here if needed in future
            _changeDate = DateTime.Now;
            Notify();
        }

        public void Register(IBranchObserver observer)
        {
            _observers.Add(observer);
        }

        public void UnRegister(IBranchObserver observer)
        {
            _observers.Remove(observer);
        }

        public void Notify()
        {
           foreach(IBranchObserver observer in _observers)
            {
                observer.Update();
            }
        }
    }
}
