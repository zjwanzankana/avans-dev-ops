using Domain.Developers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Notifications.ExternalMessageServices
{
    public class EmailMessageService : INotificatorService
    {

        public void SendNotification(string message, Developer developer)
        {
            Console.WriteLine($"Email with message: {message} send to {developer.GetName()}");
        }
    }
}
