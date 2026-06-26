using Domain.Developers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Notifications
{
    /// <summary>
    /// Adapter pattern (GoF, structural) - 'Target'.
    ///
    /// Dit is de interface die het domein nodig heeft: SendNotification(message, Developer).
    /// De concrete adapters (EmailServiceAdapter, SlackServiceAdapter) implementeren deze
    /// Target en vertalen de aanroep naar de afwijkende signatuur van de externe libraries
    /// (de 'Adaptees' ISmtpServer.SendMail(...) en ISlackApiClient.PostMessage(...)).
    ///
    /// Het domein hangt dus alleen af van deze abstractie (Dependency Inversion); een nieuw
    /// medium toevoegen = één nieuwe adapter, zonder bestaande code te wijzigen (OCP).
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
