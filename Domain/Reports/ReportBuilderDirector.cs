using Domain.Sprints;
using System;

namespace Domain.Reports
{
    /// <summary>
    /// Builder pattern (creational) - 'Director'. Kent de vaste volgorde van bouwstappen
    /// (header -> body -> footer -> get), maar niet de concrete invulling. De juiste
    /// ConcreteBuilder wordt via de <see cref="ReportBuilderFactory"/> gekozen.
    /// </summary>
    public static class ReportBuilderDirector
    {
        public static Report Build(ReportType reportType, Sprint sprint, string content, string reportName, DateTime reportDate, Format format)
        {
            IReportBuilder builder = ReportBuilderFactory.GetBuilder(reportType);
            builder.BuildHeader(reportDate, sprint, reportName);
            builder.BuildBody(content);
            builder.BuildFooter();
            return builder.GetReport(format);
        }

        public static Report BuildAvansReport(Sprint sprint, string content, string reportName, DateTime reportDate, Format format)
            => Build(ReportType.Deployment, sprint, content, reportName, reportDate, format);

        public static Report BuildStudentReport(Sprint sprint, string content, string reportName, DateTime reportDate, Format format)
            => Build(ReportType.Review, sprint, content, reportName, reportDate, format);
    }
}
