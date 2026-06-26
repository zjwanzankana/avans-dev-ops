using Domain.Developers;
using Domain.Reports;
using System;
using System.Collections.Generic;

namespace Domain.Sprints
{
    /// <summary>
    /// Een sprint die wordt afgesloten met een sprint review. De scrum master kan de
    /// sprint pas sluiten nadat hij een samenvatting van de review heeft geupload.
    /// </summary>
    public class ReviewSprint : Sprint
    {
        private Review _review;
        private bool _isReviewDone;

        public ReviewSprint(Project project, string name, DateTime startDate, DateTime endDate, Developer scrumMaster, IReadOnlyList<Developer> developers)
            : base(project, name, startDate, endDate, scrumMaster, developers)
        {
        }

        protected internal override void OnFinish()
        {
            // Een review-sprint kan optioneel ook een pipeline draaien (bv. naar een testomgeving).
            Pipeline?.Execute();
        }

        protected internal override bool CanClose() => _isReviewDone;

        /// <summary>
        /// De scrum master uploadt de review-samenvatting. Dit kan alleen wanneer de
        /// sprint is afgelopen (Finished) en alleen door de scrum master zelf.
        /// </summary>
        public void UploadReview(Review review)
        {
            ArgumentNullException.ThrowIfNull(review);

            if (State.GetSprintState() != ESprintStates.Finished)
            {
                throw new InvalidOperationException("Review can't be uploaded to a sprint that is not finished");
            }

            if (review.Author != ScrumMaster)
            {
                throw new InvalidOperationException("Only the scrum master can upload the review");
            }

            _review = review;
            _isReviewDone = true;
        }

        /// <summary>Backwards-compatibele alias voor <see cref="UploadReview"/>.</summary>
        public void SetReviewItem(Review review) => UploadReview(review);

        public Review Review => _review;

        public bool IsReviewDone => _isReviewDone;

        public Report GenerateReviewReport(string content, string name, DateTime date, Format format)
        {
            return ReportBuilderDirector.BuildStudentReport(this, content, name, date, format);
        }
    }
}
