using System;
using Domain.Developers;
using Domain.Notifications.External;

namespace Domain.Notifications.Adapters
{
    /// <summary>
    /// Adapter pattern (GoF, structural) - object-adapter.
    ///
    /// 'Target'  : INotificatorService (de interface die het domein nodig heeft).
    /// 'Adaptee' : ISmtpServer (externe library met een afwijkende interface).
    /// 'Adapter' : deze klasse - vertaalt SendNotification(message, developer) naar
    ///             de SendMail(recipient, subject, htmlBody)-call van de SMTP-library.
    ///
    /// Hier wordt dus écht geadapteerd: de incompatibele signatuur van de externe
    /// library wordt omgezet naar de domein-interface (Dependency Inversion Principle).
    /// </summary>
    public class EmailServiceAdapter : INotificatorService
    {
        private readonly ISmtpServer _smtpServer;

        public EmailServiceAdapter() : this(new SmtpServer())
        {
        }

        public EmailServiceAdapter(ISmtpServer smtpServer)
        {
            _smtpServer = smtpServer ?? throw new ArgumentNullException(nameof(smtpServer));
        }

        public void SendNotification(string message, Developer developer)
        {
            ArgumentNullException.ThrowIfNull(developer);

            // Vertaalslag: domein-aanroep -> externe SMTP-signatuur.
            _smtpServer.SendMail(
                recipient: $"{developer.Name}@avans-devops.local",
                subject: "Avans DevOps notification",
                htmlBody: message);
        }
    }
}
