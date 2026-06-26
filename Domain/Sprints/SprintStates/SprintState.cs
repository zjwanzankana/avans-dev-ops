using Domain.Backlogs;
using Domain.Developers;
using System;

namespace Domain.Sprints.SprintStates
{
    /// <summary>
    /// State pattern (GoF, behavioral) - abstracte 'State' voor de sprint-levenscyclus.
    ///
    /// Net als bij het backlog item is elke pijl uit de toestandsdiagram één expliciete
    /// methode (Start, Finish, Close, Cancel, Retry). De basisklasse weigert standaard;
    /// een concrete state staat enkel zijn eigen uitgaande transities toe. Zo is er géén
    /// generieke Next/Previous die de uitbreidbaarheid (OCP) zou breken.
    /// </summary>
    public abstract class SprintState
    {
        private readonly Sprint _sprint;

        protected SprintState(Sprint sprint)
        {
            _sprint = sprint;
        }

        protected Sprint Sprint => _sprint;

        public abstract ESprintStates GetSprintState();

        // ---- Configuratie (alleen toegestaan in de Scheduled-fase) ------------
        public virtual void SetName(string name)
            => throw new InvalidOperationException($"Can't set name in {GetSprintState()} state");

        public virtual void SetStartDate(DateTime startDate)
            => throw new InvalidOperationException($"Can't set start date in {GetSprintState()} state");

        public virtual void SetEndDate(DateTime endDate)
            => throw new InvalidOperationException($"Can't set end date in {GetSprintState()} state");

        public virtual void AddDeveloper(Developer developer)
            => throw new InvalidOperationException($"Can't add developer in {GetSprintState()} state");

        public virtual void AddToSprintBacklog(BacklogItem backlogItem)
            => throw new InvalidOperationException($"Can't add backlog item in {GetSprintState()} state");

        // ---- Transities uit de STD (per default geweigerd) --------------------

        /// <summary>Scheduled -> InProgress.</summary>
        public virtual void Start() => ThrowInvalid(nameof(Start));

        /// <summary>InProgress -> Finished. Triggert de sprint-type-specifieke afronding (OnFinish).</summary>
        public virtual void Finish() => ThrowInvalid(nameof(Finish));

        /// <summary>Finished -> Closed. Voorwaarde verschilt per sprinttype (review geupload / pipeline geslaagd).</summary>
        public virtual void Close() => ThrowInvalid(nameof(Close));

        /// <summary>Finished -> Cancelled (release geannuleerd); bericht naar PO en scrum master.</summary>
        public virtual void Cancel() => ThrowInvalid(nameof(Cancel));

        /// <summary>Finished -> Finished: probeer een gefaalde release opnieuw.</summary>
        public virtual void Retry() => ThrowInvalid(nameof(Retry));

        private void ThrowInvalid(string transition)
        {
            throw new InvalidOperationException(
                $"Transition '{transition}' is not allowed from state '{GetSprintState()}'.");
        }
    }
}
