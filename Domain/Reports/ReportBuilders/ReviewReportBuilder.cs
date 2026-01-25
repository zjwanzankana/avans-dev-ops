using Domain.Sprints;
using System;

namespace Domain.Reports.ReportBuilders
{
    public class ReviewReportBuilder : IReportBuilder
    {
        private readonly Report _report;

        public ReviewReportBuilder()
        {
            _report = new Report();
        }
        public void BuildBody(string content)
        {
            _report.Body = new Body {Content = content };
        }

        public void BuildFooter()
        {
            _report.Footer = new Footer {Companyname = "Student Report", CompanyLogo = "" };
        }

        public void BuildHeader(DateTime reportDate, Sprint sprint, string reportName)
        {
            ArgumentNullException.ThrowIfNull(sprint);

            _report.Header = new Header { creationDate = reportDate, sprintName = sprint.Name, reportName = reportName };
        }

        public Report GetReport(Format format)
        {
            _report.Format = format;
            return _report;
        }
    }
}
