using System;
using System.Collections.Generic;

namespace Domain.SCM
{
    public class Branch : BranchObservable
    {
        private string _name;
        private DateTime _changeDate;
        private Code _code;

        private List<BranchObserver> _observers;

        public Branch(string name)
        {
            this._name = name;
            this._changeDate = DateTime.Now;
            this._code = new Code("");
        }


        public string GetName()
        {
            return this._name;
        }

        public DateTime GetChangeDate()
        {
            return this._changeDate;
        }

        public Code GetCode()
        {
            return this._code;
        }

        public void PushCommit(Commit commit)
        {
#warning implment
        }

        public void Register(BranchObserver observer)
        {
            _observers.Add(observer);
        }

        public void UnRegister(BranchObserver observer)
        {
            _observers.Remove(observer);
        }

        public void Notify()
        {
           foreach(BranchObserver observer in _observers)
            {
                observer.Update();
            }
        }
    }
}