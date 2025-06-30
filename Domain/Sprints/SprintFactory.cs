using Domain.Developers;
using Domain.Pipelines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Domain.Sprints
{
    public class SprintFactory
    {
        public ReleaseSprint GetReleaseSprint(Project project, string name, DateTime startDate, DateTime endDate,Developer scumMaster, List<Developer> developers, Pipeline pipeline)
        {
            return new ReleaseSprint(project, name,startDate,endDate, scumMaster, developers, pipeline);
        }

        public ReviewSprint GetReviewSprint(Project project, string name, DateTime startDate, DateTime endDate, Developer scumMaster, List<Developer> developers)
        {
            return new ReviewSprint(project, name, startDate, endDate, scumMaster, developers);
        }
    }
}
