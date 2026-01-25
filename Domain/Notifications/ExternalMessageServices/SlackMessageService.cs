using Domain.Developers;
using Domain.Notifications;
using System;

namespace Domain.MessageServices
{
    public class SlackMessageService : INotificatorService
    {
        public void SendNotification(string message , Developer developer)
        {
            ArgumentNullException.ThrowIfNull(developer);

            Console.WriteLine($"Slack message: {message} send to {developer.Name}");
        }
    }
}
