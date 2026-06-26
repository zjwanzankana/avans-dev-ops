namespace Domain.Sprints.SprintStates
{
    /// <summary>State pattern - ConcreteState 'InProgress'. Het team werkt de sprint uit.</summary>
    internal sealed class InProgressState : SprintState
    {
        public InProgressState(Sprint sprint) : base(sprint)
        {
        }

        public override ESprintStates GetSprintState() => ESprintStates.InProgress;

        public override void Finish()
        {
            Sprint.ChangeState(new FinishedState(Sprint));

            // Polymorfe afronding i.p.v. een GetType()-switch: ReleaseSprint draait
            // zijn pipeline, ReviewSprint doet (standaard) niets.
            Sprint.OnFinish();
        }
    }
}
