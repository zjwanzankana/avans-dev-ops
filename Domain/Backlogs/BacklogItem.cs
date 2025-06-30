using Domain.Backlogs.BacklogItemStates;
using Domain.Developers;
using Domain.Notifications;
using Domain.Sprints;
using System;
using System.Collections.Generic;

namespace Domain.Backlogs
{
    public class BacklogItem : IBacklogObservable
    {
        private string _name;
        private string _description;
        private int _effort;

        private List<Activity> _activities;
        private Developer _assignedDeveloper;

        private readonly Backlog _backlog;
        private Sprint _sprint;

        private BacklogItemState _state;
        private List<IBacklogObserver> _observers;

        public BacklogItem(string name, string description, int effort, Backlog backlog)
        {
            this._name = name;
            this._description = description;
            this._effort = effort;
            _state = new TodoState(this);
            _backlog = backlog;
            _observers = new List<IBacklogObserver>();

            _activities = new List<Activity>();
        }

        public void SetSprint(Sprint sprint)
        {
            _sprint = sprint;
        }

        public Sprint GetSprint()
        {
            return _sprint;
        }

        public bool AllActivitiesDone()
        {
            foreach (Activity activity in _activities)
            {
               if (activity.GetStatus() != ActivityStatus.Done) 
                {
                    return false;
                }
            }
            return true;
        }

        public bool AllActivitiesDoneOrActive()
        {
            foreach (Activity activity in _activities)
            {
                if (activity.GetStatus() != ActivityStatus.Todo)
                {
                    return false;
                }
            }
            return true;
        }

        public void SetName(string name)
        {
            _name = name;
        }

        public string GetName()
        {
            return _name;
        }

        public void SetDescription(string description)
        {
            _description = description;
        }

        public string GetDescription()
        {
            return _description;
        }

        public void SetEffort(int effort)
        {
            _effort = effort;
        }

        public int GetEffort()
        {
            return _effort;
        }

        public void AssignDeveloper(Developer newAssignedDeveloper)
        {
            _assignedDeveloper = newAssignedDeveloper;
            Register(new Notificator(_assignedDeveloper));
        }

        public Developer GetAssignedDeveloper()
        {
            return _assignedDeveloper;
        }

        public void ChangeState(BacklogItemState state)
        {
            //The state of the backlogItem can only be changed once it has a sprint reference
            if (_sprint == null)
            { 
                throw new Exception("The backlogItem is not part a sprint so state can't be changed");
            }
            _state = state;
            Notify();
        }

        public EBacklogStates GetStateType()
        {
            return _state.GetState();
        }

        public BacklogItemState GetState()
        {
            return _state;
        }

        public List<Activity> GetActivities()
        {
            return _activities;
        }

        public void AddActivity(Activity activity)
        {
            if (!_activities.Contains(activity))
            { 
                _activities.Add(activity);
            }
        }

        public bool RemoveActivity(Activity activity)
        {
            if (_activities == null)
            {
                throw new NotSupportedException("There are no tasks in this backlogItem");
            }

            return _activities.Remove(activity);
        }

        public void Register(IBacklogObserver observer)
        {
            _observers.Add(observer);
        }

        public void UnRegister(IBacklogObserver observer)
        {
            _observers.Remove(observer);
        }

        public void Notify()
        {
            foreach (var observer in _observers)
            {
                observer.Update(this.GetState());                
            }
        }
    }
}
