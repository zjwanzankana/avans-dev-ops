namespace Domain.Sprints.SprintStates
{
    /// <summary>State pattern - ConcreteState 'Cancelled'. Eindtoestand: release geannuleerd.</summary>
    internal sealed class CancelledState : SprintState
    {
        public CancelledState(Sprint sprint) : base(sprint)
        {
        }

        public override ESprintStates GetSprintState() => ESprintStates.Cancelled;
    }
}
