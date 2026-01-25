using Domain.Notifications;
using Domain.Notifications.ExternalMessageServices;
using System;

namespace Domain.Developers
{
    public class Developer
    {
        private string _name;
        private readonly Role _role;
        private readonly INotificatorService _notificatorService;
        
        public Developer(string name, Role role, INotificatorService notificatorService)
        {
            this._name = name;
            this._role = role;
            this._notificatorService = notificatorService ?? throw new ArgumentNullException(nameof(notificatorService));
        }

        public Role Role => _role;

        public void SendNotification(string message)
        {
            this._notificatorService.SendNotification(message, this);
        }

        public string Name => _name;
    }
}
