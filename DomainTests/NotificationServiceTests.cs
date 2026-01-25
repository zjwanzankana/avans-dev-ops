using Domain.MessageServices;
using Domain.Notifications.ExternalMessageServices;
using System;

namespace DomainTests
{
    public class NotificationServiceTests
    {
        [Fact]
        public void A_Developer_Uses_The_Notificator_Service()
        {
            var service = new RecordingNotificatorService();
            var developer = TestHelpers.CreateDeveloper("Dev", Role.Developer, service);

            developer.SendNotification("Hello");

            Assert.Equal(1, service.MessagesSent);
        }

        [Fact]
        public void Notification_Services_Require_A_Developer()
        {
            var email = new EmailMessageService();
            var slack = new SlackMessageService();
            var google = new GoogleMessageService();

            Assert.Throws<ArgumentNullException>(() => email.SendNotification("msg", null));
            Assert.Throws<ArgumentNullException>(() => slack.SendNotification("msg", null));
            Assert.Throws<ArgumentNullException>(() => google.SendNotification("msg", null));
        }

        private sealed class RecordingNotificatorService : INotificatorService
        {
            public int MessagesSent { get; private set; }

            public void SendNotification(string message, Developer developer)
            {
                MessagesSent++;
            }
        }
    }
}
