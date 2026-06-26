using System;

namespace Domain.Sprints.SprintStates
{
    /// <summary>
    /// State pattern - ConcreteState 'Finished'. 'De tijd is op'.
    /// Vanaf hier splitst de flow zich (polymorf, geen type-switch):
    ///  - ReviewSprint: Close() mag pas als de scrum master de review heeft geupload.
    ///  - ReleaseSprint: Close() mag als de pipeline slaagde; bij een fout kan de scrum
    ///    master Retry() of Cancel() kiezen.
    /// </summary>
    internal sealed class FinishedState : SprintState
    {
        public FinishedState(Sprint sprint) : base(sprint)
        {
        }

        public override ESprintStates GetSprintState() => ESprintStates.Finished;

        public override void Close()
        {
            if (!Sprint.CanClose())
            {
                throw new InvalidOperationException(
                    "Sprint can't be closed yet (review not uploaded or release pipeline not finished successfully).");
            }

            Sprint.ChangeState(new ClosedState(Sprint));
        }

        public override void Retry()
        {
            // Opnieuw proberen (bv. een server was tijdelijk onbereikbaar).
            Sprint.OnFinish();
        }

        public override void Cancel()
        {
            Sprint.NotifyStakeholders($"Sprint '{Sprint.Name}' has been cancelled.");
            Sprint.ChangeState(new CancelledState(Sprint));
        }
    }
}
