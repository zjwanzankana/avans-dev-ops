using Domain.Sprints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            _report.Footer = new Footer {companyname = "Student Report", companyLogo = "" };
        }

        public void BuildHeader(DateTime date, Sprint sprint, string reportName)
        {
            _report.Header = new Header { creationDate = date, sprintName = sprint.GetName(), reportName = reportName };
        }

        public Report GetReport(Format format)
        {
            _report.Format = format;
            return _report;
        }
    }
}
