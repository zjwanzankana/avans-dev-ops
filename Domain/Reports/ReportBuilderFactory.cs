using System;
using Domain.Reports.ReportBuilders;

namespace Domain.Reports
{
    /// <summary>
    /// Factory (creational) - hier valt de KEUZE voor het type rapport.
    ///
    /// De client zegt enkel "ik wil een Deployment- of Review-rapport"; de factory beslist
    /// welke ConcreteBuilder (DeploymentReportBuilder / ReviewReportBuilder) wordt
    /// teruggegeven. Daarna verzorgt de Director de bouwstappen. Zo blijven 'welk soort
    /// rapport' (Factory) en 'hoe bouw je het stap voor stap' (Builder) netjes gescheiden.
    /// Een nieuw rapporttype = één extra builder + één enum-waarde (OCP).
    /// </summary>
    public static class ReportBuilderFactory
    {
        public static IReportBuilder GetBuilder(ReportType reportType)
        {
            return reportType switch
            {
                ReportType.Deployment => new DeploymentReportBuilder(),
                ReportType.Review => new ReviewReportBuilder(),
                _ => throw new ArgumentOutOfRangeException(nameof(reportType), reportType, "Unknown report type")
            };
        }
    }
}
