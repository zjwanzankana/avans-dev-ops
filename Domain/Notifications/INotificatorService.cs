using Domain.Developers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Notifications
{
    /// <summary>
    /// Strategy interface voor verschillende notificatie services.
    /// Implementeert het Strategy design pattern voor notificatie mechanismes.
    /// </summary>
    public interface INotificatorService
    {
        /// <summary>
        /// Verstuurt een notificatie naar een developer via het gekozen medium.
        /// </summary>
        /// <param name="message">Het bericht dat verstuurd moet worden</param>
        /// <param name="developer">De developer die het bericht ontvangt</param>
        void SendNotification(string message, Developer developer);
    }
}
