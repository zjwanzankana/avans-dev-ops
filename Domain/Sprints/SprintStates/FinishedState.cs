using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Sprints.SprintStates
{
    internal class FinishedState : SprintState
    {
        private readonly Sprint _sprint;

#warning pipeline

        public FinishedState(Sprint sprint) : base(sprint)
        {
            _sprint = sprint;
        }

        public override void SetReview(Review review)
        {
            if (_sprint.GetType().Name == "ReviewSprint")
            {
                if (review.Author == _sprint.GetScrumMaster())
                {
                    ((ReviewSprint)_sprint).AddReview(review);
                }
                else
                {
                    throw new Exception("Only scrummaster can add a review");
                }
            }
            throw new Exception("Review can't be added to release sprint");
        }

        public override void NextState()
        {
            throw new Exception($"No next state for {GetSprintState()} state");
        }

        public override void PreviousState()
        {
            this._sprint.ChangeState(new InProgressState(_sprint));
        }

        public override void StartStateAction()
        {
            switch (_sprint.GetType().Name)
            {
                case "ReleaseSprint":
                    _sprint.GetPipeline().Execute();
                    break;
                case "ReviewSprint":
                    if (_sprint.GetPipeline() != null)
                    { 
                        _sprint.GetPipeline().Execute();
                    }
                    break;
            }


        }

        public override ESprintStates GetSprintState()
        {
            return ESprintStates.Finished;
        }
    }
}
