using Domain.Sprints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Reports.ReportBuilders
{
    public class DeploymentReportBuilder : IReportBuilder
    {
        private readonly Report _report;

        public DeploymentReportBuilder()
        {
            _report = new Report();
        }

        public void BuildBody(string content)
        {
            this._report.Body = new Body {Content = content };
        }

        public void BuildFooter()
        {
            this._report.Footer = new Footer { companyname = "Avans", companyLogo = "AvansLogo"};
        }

        public void BuildHeader(DateTime date, Sprint sprint, string reportName)
        {
            this._report.Header = new Header {companyname = "Avans", companyLogo = "AvansLogo", creationDate =  date, reportName = reportName, sprintName = sprint.GetName()};
        }

        public Report GetReport(Format format)
        {
            this._report.Format = format;
            return this._report;
        }
    }
}
