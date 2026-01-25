using Domain.Developers;
using System;

namespace Domain.Backlogs
{
    public class Activity
    {
        private string _description;
        private Developer _assignedDeveloper;
        private ActivityStatus _status;

        public Activity(string description)
        {
            _description = description;
        }

        // Set description but not when _status is done
        public string Description
        {
            get => _description;
            set
            {
                if (_status != ActivityStatus.Done)
                {
                    _description = value;
                }
                else
                {
                    throw new InvalidOperationException("Activity is done, cannot change description when DONE");
                }
            }
        }

        public Developer AssignedDeveloper
        {
            get => _assignedDeveloper;
            set
            {
                if (_status != ActivityStatus.Done)
                {
                    _assignedDeveloper = value;
                }
                else
                {
                    throw new InvalidOperationException("Activity is done, cannot change developer when DONE");
                }
            }
        }

        public ActivityStatus Status => _status;


        public void NextStatus()
        { 
            switch (_status)
            {
                case ActivityStatus.Todo:
                    _status = ActivityStatus.Active;
                    break;
                case ActivityStatus.Active:
                    _status = ActivityStatus.Done;
                    break;
                case ActivityStatus.Done:
                    throw new InvalidOperationException("Activity is already done");
            }
        }

        public void PreviousStatus()
        {
            switch (_status)
            {
                case ActivityStatus.Todo:
                    throw new InvalidOperationException("Activity is already todo");
                case ActivityStatus.Active:
                    _status = ActivityStatus.Todo;
                    break;
                case ActivityStatus.Done:
                    _status = ActivityStatus.Active;
                    break;
            }
        }
    }

    public enum ActivityStatus
    {
        Todo,
        Active,
        Done
    }
}
