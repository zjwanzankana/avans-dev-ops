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

        //Set discription but one when _status is done
        public void SetDescription(string description)
        {
            if (_status != ActivityStatus.Done)
            {
                _description = description;
            }
            else 
            {
                throw new Exception("Activity is done, cannot change description when DONE");
            }
        }

        public string GetDescription()
        {
            return _description;
        }

        public Developer GetAssignedDeveloper()
        {
            return _assignedDeveloper;
        }

        public void SetAssignedDeveloper(Developer developer)
        {
            if (_status != ActivityStatus.Done)
            {
                _assignedDeveloper = developer;
            }
            else
            {
                throw new Exception("Activity is done, cannot change developer when DONE");
            }
        }

        public ActivityStatus GetStatus()
        {
            return _status;
        }


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
                    throw new Exception("Activity is already done");
            }
        }

        public void PreviousStatus()
        {
            switch (_status)
            {
                case ActivityStatus.Todo:
                    throw new Exception("Activity is already todo");
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