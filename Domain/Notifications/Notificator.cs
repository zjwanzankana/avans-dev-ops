using Domain.Backlogs.BacklogItemStates;
using Domain.Developers;
using System;

namespace Domain.Notifications
{
    public class Notificator : IBacklogObserver
    {
        private BacklogItemState _backlogItemState;
        private readonly Developer _developer;

        public Notificator(Developer developer)
        {
            //_backlogItemState = backlogItemState;
            _developer = developer;
        }

        public BacklogItemState BacklogItemState => _backlogItemState;

        // to check if it is running with tests
        public int MessagesSent { get; private set; }

        public void Update(BacklogItemState backlogItem)
        {
            ArgumentNullException.ThrowIfNull(backlogItem);

            _backlogItemState = backlogItem;

            MessagesSent++;
            _developer.SendNotification($"Hey {_developer} your backlogItem has changed fase to {backlogItem.GetType()}");
        }
    }
}
