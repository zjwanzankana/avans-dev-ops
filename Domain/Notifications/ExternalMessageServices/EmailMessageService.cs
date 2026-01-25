using Domain.Developers;
using System;

namespace Domain.Notifications.ExternalMessageServices
{
    public class EmailMessageService : INotificatorService
    {

        public void SendNotification(string message, Developer developer)
        {
            ArgumentNullException.ThrowIfNull(developer);

            Console.WriteLine($"Email with message: {message} send to {developer.Name}");
        }
    }
}
