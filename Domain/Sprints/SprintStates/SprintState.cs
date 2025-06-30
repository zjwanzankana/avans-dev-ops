using Domain.Backlogs;
using Domain.Developers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Sprints.SprintStates
{
    public abstract class SprintState
    {
        private readonly Sprint _sprint;

        public SprintState(Sprint sprint)
        {
            this._sprint = sprint;
        }

        public virtual void SetReview(Review review)
        {
            throw new NotSupportedException($"Can't add a review in {GetSprintState()} State");
        }

        public virtual void SetName(string name)
        {
            throw new Exception($"Can't set name in this {GetSprintState()}");
        }

        public virtual void SetStartDate(DateTime startDate)
        {
            throw new Exception($"Can't set start date in this {GetSprintState()}");
        }

        public virtual void SetEndDate(DateTime endDate)
        {
            throw new Exception($"Can't set end date in this {GetSprintState()}");
        }

        public virtual void AddDeveloper(Developer developer)
        {
            throw new Exception($"Can't add developer in this {GetSprintState()}");
        }

        public virtual void AddToSprintBacklog(BacklogItem backlogItem)
        {
           throw new Exception($"Can't add backlog item in this {GetSprintState()}");
        }

        public abstract ESprintStates GetSprintState();
        public abstract void StartStateAction();
        public abstract void NextState();
        public abstract void PreviousState();
    }
}
