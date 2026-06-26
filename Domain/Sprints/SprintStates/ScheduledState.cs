using Domain.Backlogs;
using Domain.Developers;
using System;

namespace Domain.Sprints.SprintStates
{
    /// <summary>State pattern - ConcreteState 'Scheduled'. Enige fase waarin de sprint geconfigureerd mag worden.</summary>
    public class ScheduledState : SprintState
    {
        public ScheduledState(Sprint sprint) : base(sprint)
        {
        }

        public override ESprintStates GetSprintState() => ESprintStates.Scheduled;

        public override void SetName(string name) => Sprint.SetName(name);

        public override void SetStartDate(DateTime startDate) => Sprint.SetStartDate(startDate);

        public override void SetEndDate(DateTime endDate) => Sprint.SetEndDate(endDate);

        public override void AddDeveloper(Developer developer) => Sprint.AddDeveloper(developer);

        public override void AddToSprintBacklog(BacklogItem backlogItem) => Sprint.AddToSprintBacklog(backlogItem);

        public override void Start() => Sprint.ChangeState(new InProgressState(Sprint));
    }
}
