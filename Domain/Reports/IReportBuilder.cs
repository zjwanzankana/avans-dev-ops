using Domain.Sprints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Reports
{
    /// <summary>
    /// Builder interface voor het stapsgewijs construeren van rapporten.
    /// Implementeert het Builder design pattern.
    /// </summary>
    public interface IReportBuilder
    {
        /// <summary>
        /// Bouwt de header van het rapport met metadata.
        /// </summary>
        void BuildHeader(DateTime reportDate, Sprint sprint, string reportName);

        /// <summary>
        /// Bouwt de body van het rapport met de hoofdinhoud.
        /// </summary>
        void BuildBody(string content);

        /// <summary>
        /// Bouwt de footer van het rapport.
        /// </summary>
        void BuildFooter();

        /// <summary>
        /// Retourneert het voltooide rapport in het gespecificeerde formaat.
        /// </summary>
        /// <param name="format">Het gewenste output formaat (PDF, PNG, etc.)</param>
        Report GetReport(Format format);
    }
}
