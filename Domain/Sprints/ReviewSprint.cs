using Domain.Developers;
using Domain.Reports;
using Domain.Sprints.SprintStates;
using System;
using System.Collections.Generic;

namespace Domain.Sprints
{
        public class ReviewSprint : Sprint
    {
        private Review _review;
        private bool _isReviewDone;
        public ReviewSprint(Project project, string name, DateTime startDate, DateTime endDate, Developer scrumMaster, IReadOnlyList<Developer> developers) : base(project, name, startDate, endDate, scrumMaster, developers)
        {
        }

        public void AddReview(Review review)
        {
            _review = review;
        }

        public void SetReviewItem(Review review)
        {
            if (State.GetSprintState() == ESprintStates.Finished)
            {
                _review = review;
                _isReviewDone = true;
            }
            else 
            { 
                throw new InvalidOperationException("Review can't be added to sprint that is not finished");
            }
        }

        public Review Review => _review;

        public bool IsReviewDone => _isReviewDone;

        public Report GenerateReviewReport(string content, string name, DateTime date, Format format)
        {
            return ReportBuilderDirector.BuildStudentReport(this, content, name, date, format);
        }
    }
}
