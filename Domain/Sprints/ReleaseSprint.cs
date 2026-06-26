using Domain.Developers;
using Domain.Pipelines;
using Domain.Reports;
using System;
using System.Collections.Generic;

namespace Domain.Sprints
{
    /// <summary>
    /// Een sprint die eindigt met een release: bij het afronden draait de gekoppelde
    /// development pipeline. Pas als die volledig slaagt mag de sprint gesloten worden.
    /// </summary>
    public class ReleaseSprint : Sprint
    {
        public ReleaseSprint(Project project, string name, DateTime startDate, DateTime endDate, Developer scrumMaster, IReadOnlyList<Developer> developers, Pipeline pipeline)
            : base(project, name, startDate, endDate, scrumMaster, developers)
        {
            base.SetPipeline(pipeline);
        }

        protected internal override void OnFinish()
        {
            // Release-pipeline starten (installeren, build, test, ... t/m deployment).
            Pipeline?.Execute();
        }

        protected internal override bool CanClose()
        {
            // Sluiten/gereleased mag alleen als de pipeline succesvol klaar is.
            return Pipeline != null && Pipeline.Status == PipelineJobStatus.FINISHED;
        }

        public Report GenerateDeploymentReport(string content, string name, DateTime date, Format format)
        {
            return ReportBuilderDirector.BuildAvansReport(this, content, name, date, format);
        }
    }
}
