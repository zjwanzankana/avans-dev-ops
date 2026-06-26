using System;
using Domain.Developers;
using Domain.Notifications.External;

namespace Domain.Notifications.Adapters
{
    /// <summary>
    /// Adapter pattern (GoF, structural) - object-adapter.
    /// Target: INotificatorService, Adaptee: ISlackApiClient.
    /// Vertaalt de domein-aanroep naar de channel/markdown-signatuur van Slack.
    /// </summary>
    public class SlackServiceAdapter : INotificatorService
    {
        private readonly ISlackApiClient _slackApiClient;

        public SlackServiceAdapter() : this(new SlackApiClient())
        {
        }

        public SlackServiceAdapter(ISlackApiClient slackApiClient)
        {
            _slackApiClient = slackApiClient ?? throw new ArgumentNullException(nameof(slackApiClient));
        }

        public void SendNotification(string message, Developer developer)
        {
            ArgumentNullException.ThrowIfNull(developer);

            // Vertaalslag: domein-aanroep -> Slack-signatuur (channel + markdown).
            _slackApiClient.PostMessage(
                channelId: $"@{developer.Name}",
                markdownText: message);
        }
    }
}
