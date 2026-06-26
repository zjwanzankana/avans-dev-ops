using System;

namespace Domain.Notifications.External
{
    /// <summary>Stub-implementatie van de externe Slack-client (infrastructure ring).</summary>
    public class SlackApiClient : ISlackApiClient
    {
        public void PostMessage(string channelId, string markdownText)
        {
            Console.WriteLine($"[Slack] {channelId}: {markdownText}");
        }
    }
}
