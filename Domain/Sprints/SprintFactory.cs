using Domain.Developers;
using Domain.Pipelines;
using System;
using System.Collections.Generic;

namespace Domain.Sprints
{
    /// <summary>
    /// Factory voor het creëren van verschillende sprint types.
    /// Implementeert het Factory design pattern.
    /// </summary>
    public static class SprintFactory
    {
        /// <summary>
        /// Creëert een ReleaseSprint met een gekoppelde deployment pipeline.
        /// </summary>
        public static ReleaseSprint GetReleaseSprint(Project project, string name, DateTime startDate, DateTime endDate, Developer scumMaster, IReadOnlyList<Developer> developers, Pipeline pipeline)
        {
            return new ReleaseSprint(project, name,startDate,endDate, scumMaster, developers, pipeline);
        }

        /// <summary>
        /// Creëert een ReviewSprint zonder deployment pipeline.
        /// </summary>
        public static ReviewSprint GetReviewSprint(Project project, string name, DateTime startDate, DateTime endDate, Developer scumMaster, IReadOnlyList<Developer> developers)
        {
            return new ReviewSprint(project, name, startDate, endDate, scumMaster, developers);
        }
    }
}
