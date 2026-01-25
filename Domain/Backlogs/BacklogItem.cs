using Domain.Backlogs.BacklogItemStates;
using Domain.Developers;
using Domain.Notifications;
using Domain.Sprints;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

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

        public Sprint Sprint
        {
            get => _sprint;
            set => _sprint = value;
        }

        public bool AllActivitiesDone()
        {
            foreach (Activity activity in _activities)
            {
               if (activity.Status != ActivityStatus.Done) 
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
                if (activity.Status != ActivityStatus.Todo)
                {
                    return false;
                }
            }
            return true;
        }

        public string Name
        {
            get => _name;
            set => _name = value;
        }

        public string Description
        {
            get => _description;
            set => _description = value;
        }

        public int Effort
        {
            get => _effort;
            set => _effort = value;
        }

        public void AssignDeveloper(Developer newAssignedDeveloper)
        {
            _assignedDeveloper = newAssignedDeveloper;
            Register(new Notificator(_assignedDeveloper));
        }

        public Developer AssignedDeveloper => _assignedDeveloper;

        public void ChangeState(BacklogItemState state)
        {
            //The state of the backlogItem can only be changed once it has a sprint reference
            if (_sprint == null)
            { 
                throw new InvalidOperationException("The backlogItem is not part a sprint so state can't be changed");
            }
            _state = state;
            Notify();
        }

        public EBacklogStates StateType => _state.GetState();

        public BacklogItemState State => _state;

        public ReadOnlyCollection<Activity> Activities => _activities.AsReadOnly();

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
                observer.Update(State);                
            }
        }
    }
}
