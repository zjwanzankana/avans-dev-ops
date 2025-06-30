using Domain.Backlogs.BacklogItemStates;
using Domain.Developers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Notifications
{
    public class Notificator : IBacklogObserver
    {
        private BacklogItemState _backlogItemState;
        private Developer _developer;

        //to check if it is running with tests
        public int messagesSend = 0;

        public Notificator(Developer developer)
        {
            //_backlogItemState = backlogItemState;
            _developer = developer;
        }

        public BacklogItemState GetBacklogItemState()
        {
            return _backlogItemState;
        }

        public void Update(BacklogItemState backlogItemState)
        {
            this._backlogItemState = backlogItemState;

            messagesSend++;
            _developer.SendNotification($"Hey {_developer} your backlogItem has changed fase to {backlogItemState.GetType()}");
        }
    }
}
