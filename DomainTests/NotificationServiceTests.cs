using Domain.MessageServices;
using Domain.Notifications.ExternalMessageServices;
using Moq;
using System;

namespace DomainTests
{
    public class NotificationServiceTests
    {
        [Fact]
        public void A_Developer_Uses_The_Notificator_Service()
        {
            var service = new Mock<INotificatorService>();
            var developer = TestHelpers.CreateDeveloper("Dev", Role.Developer, service.Object);

            developer.SendNotification("Hello");

            service.Verify(s => s.SendNotification("Hello", developer), Times.Once);
        }

        [Fact]
        public void A_Developer_Uses_The_Notificator_Service_Via_Mock()
        {
            var service = new Mock<INotificatorService>();
            var developer = TestHelpers.CreateDeveloper("Dev", Role.Developer, service.Object);

            developer.SendNotification("Hello");

            service.Verify(s => s.SendNotification("Hello", developer), Times.Once);
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

    }
}
