using Domain.Reports.ReportBuilders;
using Domain.Sprints;
using Domain.Sprints.SprintStates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Reports
{
    public static class ReportBuilderDirector
    {
        public static Report BuildAvansReport(Sprint sprint, string content, string reportName, DateTime reportDate, Format format)
        { 
            var builder = new DeploymentReportBuilder();
            builder.BuildHeader(reportDate, sprint, reportName);
            builder.BuildBody(content);
            builder.BuildFooter();
            return builder.GetReport(format);
        }

        public static Report BuildStudentReport(Sprint sprint, string content, string reportName, DateTime reportDate, Format format)
        {
            var builder = new ReviewReportBuilder();
            builder.BuildHeader(reportDate, sprint, reportName);
            builder.BuildBody(content);
            builder.BuildFooter();
            return builder.GetReport(format);
        }
    }
}
