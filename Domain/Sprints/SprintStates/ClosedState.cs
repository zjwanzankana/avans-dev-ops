namespace Domain.Sprints.SprintStates
{
    /// <summary>State pattern - ConcreteState 'Closed'. Eindtoestand: sprint succesvol afgesloten/gereleased.</summary>
    internal sealed class ClosedState : SprintState
    {
        public ClosedState(Sprint sprint) : base(sprint)
        {
        }

        public override ESprintStates GetSprintState() => ESprintStates.Closed;
    }
}
