using Domain.Notifications;
using Domain.Notifications.ExternalMessageServices;
using System;
using System.Data;

namespace Domain.Developers
{
    public class Developer
    {
        private string _name;
        private readonly Role _role;
        private INotificatorService _notificatorService;
        public Developer(string name, Role role)
        {
            this._name = name;
            this._role = role;
            this._notificatorService = new EmailMessageService();
        }

        public Role GetRole()
        {
            return this._role;
        }

        public void SendNotification(string message)
        {
            this._notificatorService.SendNotification(message, this);
        }

        public void setNotificatorService(INotificatorService notificatorService)
        {
            this._notificatorService = notificatorService;
        }

        public string GetName() {
            return _name;
        }
    }
}
