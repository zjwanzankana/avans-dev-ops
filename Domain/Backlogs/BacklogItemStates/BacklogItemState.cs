using System;
using Domain.Backlogs;

namespace Domain.Backlogs.BacklogItemStates
{
    /// <summary>
    /// State pattern (GoF, behavioral) - abstracte 'State'.
    ///
    /// Elke overgang uit de toestandsdiagram (STD) is hier één expliciete methode
    /// (BeginWork, SubmitForTesting, StartTesting, ...). De basisklasse weigert
    /// standaard élke overgang met een InvalidOperationException; een concrete
    /// state staat alléén de overgangen toe die in zijn knoop uit de STD vertrekken.
    ///
    /// Hierdoor is er géén generieke Next/Previous meer: het toevoegen van een nieuwe
    /// fase (bv. 'PeerReview') betekent een nieuwe klasse + het overschrijven van de
    /// relevante transitie-methode, zonder bestaande states te wijzigen (OCP).
    /// </summary>
    public abstract class BacklogItemState
    {
        private readonly BacklogItem _backlogItem;

        protected BacklogItemState(BacklogItem backlogItem)
        {
            _backlogItem = backlogItem;
        }

        public BacklogItem BacklogItem => _backlogItem;

        public abstract EBacklogStates GetState();

        // ---- Bewerkingen op het item (per default toegestaan) -----------------
        public virtual void AddActivity(Activity activity) => BacklogItem.AddActivity(activity);

        public virtual void RemoveActivity(Activity activity) => BacklogItem.RemoveActivity(activity);

        public virtual void SetDescription(string description) => BacklogItem.Description = description;

        public virtual void SetEffort(int newEffort) => BacklogItem.Effort = newEffort;

        public virtual void SetName(string newName) => BacklogItem.Name = newName;

        // ---- Transities uit de STD (per default geweigerd) --------------------

        /// <summary>TodoState -> DoingState. Developer moet toegewezen zijn.</summary>
        public virtual void BeginWork() => ThrowInvalid(nameof(BeginWork));

        /// <summary>Doing -> ReadyForTesting. Alle activiteiten moeten done zijn; testers worden genotificeerd.</summary>
        public virtual void SubmitForTesting() => ThrowInvalid(nameof(SubmitForTesting));

        /// <summary>ReadyForTesting -> Testing.</summary>
        public virtual void StartTesting() => ThrowInvalid(nameof(StartTesting));

        /// <summary>Testing -> Tested.</summary>
        public virtual void CompleteTesting() => ThrowInvalid(nameof(CompleteTesting));

        /// <summary>ReadyForTesting/Testing -> TodoState: tester vindt een defect; scrum master krijgt bericht.</summary>
        public virtual void RejectByTester() => ThrowInvalid(nameof(RejectByTester));

        /// <summary>Tested -> Done: (lead) developer keurt af aan de hand van de Definition of Done.</summary>
        public virtual void Approve() => ThrowInvalid(nameof(Approve));

        /// <summary>Tested -> ReadyForTesting: item voldoet niet aan de DoD, tester test opnieuw.</summary>
        public virtual void RejectByLeadDeveloper() => ThrowInvalid(nameof(RejectByLeadDeveloper));

        /// <summary>Done -> TodoState: item wordt heropend (bv. naar aanleiding van een discussie).</summary>
        public virtual void Reopen() => ThrowInvalid(nameof(Reopen));

        private void ThrowInvalid(string transition)
        {
            throw new InvalidOperationException(
                $"Transition '{transition}' is not allowed from state '{GetState()}'.");
        }
    }
}
