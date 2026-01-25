using Domain.Sprints;
using System;

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
            this._report.Footer = new Footer { Companyname = "Avans", CompanyLogo = "AvansLogo"};
        }

        public void BuildHeader(DateTime reportDate, Sprint sprint, string reportName)
        {
            ArgumentNullException.ThrowIfNull(sprint);

            this._report.Header = new Header
            {
                companyname = "Avans",
                companyLogo = "AvansLogo",
                creationDate = reportDate,
                reportName = reportName,
                sprintName = sprint.Name
            };
        }

        public Report GetReport(Format format)
        {
            this._report.Format = format;
            return this._report;
        }
    }
}
