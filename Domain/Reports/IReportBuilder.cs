using Domain.Sprints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Reports
{
    public interface IReportBuilder
    {
        void BuildHeader(DateTime date, Sprint sprint, string reportName);

        void BuildBody(string content);

        void BuildFooter();

        Report GetReport(Format format);
    }
}
