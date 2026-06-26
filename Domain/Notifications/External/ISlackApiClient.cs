namespace Domain.Notifications.External
{
    /// <summary>
    /// Stub voor de externe Slack web-API. Incompatibele signatuur: Slack denkt in
    /// channels en markdown-tekst, niet in onze (message, Developer)-aanroep.
    /// </summary>
    public interface ISlackApiClient
    {
        void PostMessage(string channelId, string markdownText);
    }
}
