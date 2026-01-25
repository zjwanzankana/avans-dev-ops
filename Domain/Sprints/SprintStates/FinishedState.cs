using System;

namespace Domain.Sprints.SprintStates
{
    internal sealed class FinishedState : SprintState
    {
        private readonly Sprint _sprint;

        public FinishedState(Sprint sprint) : base(sprint)
        {
            _sprint = sprint;
            
            // Pipeline execution wordt gestart via ReleaseSprint.Pipeline.Execute()
        }

        public override void SetReview(Review review)
        {
            if (_sprint.GetType().Name == "ReviewSprint")
            {
                if (review.Author == _sprint.ScrumMaster)
                {
                    ((ReviewSprint)_sprint).AddReview(review);
                }
                else
                {
                    throw new InvalidOperationException("Only scrummaster can add a review");
                }
            }
            throw new InvalidOperationException("Review can't be added to release sprint");
        }

        public override void NextState()
        {
            throw new InvalidOperationException($"No next state for {GetSprintState()} state");
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
                    _sprint.Pipeline.Execute();
                    break;
                case "ReviewSprint":
                    if (_sprint.Pipeline != null)
                    { 
                        _sprint.Pipeline.Execute();
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
