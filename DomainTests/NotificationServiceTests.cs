using Domain.Notifications.Adapters;
using Domain.Notifications.External;
using Moq;
using System;

namespace DomainTests
{
    public class NotificationServiceTests
    {
        // FR_N1 - Developer gebruikt de (gekozen) notificatie-strategie.
        [Fact]
        public void A_Developer_Uses_The_Notificator_Service()
        {
            var service = new Mock<INotificatorService>();
            var developer = TestHelpers.CreateDeveloper("Dev", Role.Developer, service.Object);

            developer.SendNotification("Hello");

            service.Verify(s => s.SendNotification("Hello", developer), Times.Once);
        }

        // Adapter pattern: de e-mail-adapter vertaalt onze aanroep naar de SMTP-signatuur.
        [Fact]
        public void Email_Adapter_Translates_To_Smtp_Server_Call()
        {
            var smtp = new Mock<ISmtpServer>();
            var adapter = new EmailServiceAdapter(smtp.Object);
            var developer = TestHelpers.CreateDeveloper("Dev", Role.Developer, adapter);

            developer.SendNotification("Ready for testing");

            smtp.Verify(s => s.SendMail(
                It.Is<string>(r => r.Contains("Dev")),
                It.IsAny<string>(),
                "Ready for testing"), Times.Once);
        }

        // Adapter pattern: de Slack-adapter vertaalt onze aanroep naar channel + markdown.
        [Fact]
        public void Slack_Adapter_Translates_To_Slack_Api_Call()
        {
            var slack = new Mock<ISlackApiClient>();
            var adapter = new SlackServiceAdapter(slack.Object);
            var developer = TestHelpers.CreateDeveloper("Dev", Role.Developer, adapter);

            developer.SendNotification("Build failed");

            slack.Verify(s => s.PostMessage("@Dev", "Build failed"), Times.Once);
        }

        [Fact]
        public void Notification_Adapters_Require_A_Developer()
        {
            var email = new EmailServiceAdapter();
            var slack = new SlackServiceAdapter();

            Assert.Throws<ArgumentNullException>(() => email.SendNotification("msg", null));
            Assert.Throws<ArgumentNullException>(() => slack.SendNotification("msg", null));
        }

        [Fact]
        public void Adapters_Require_An_External_Client()
        {
            Assert.Throws<ArgumentNullException>(() => new EmailServiceAdapter(null));
            Assert.Throws<ArgumentNullException>(() => new SlackServiceAdapter(null));
        }
    }
}
