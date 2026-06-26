namespace Domain.Notifications.External
{
    /// <summary>
    /// Stub voor een externe e-mail/SMTP-library. Let op de INCOMPATIBELE signatuur:
    /// deze 'Adaptee' kent geen Developer en geen generieke message, maar werkt met
    /// een ontvangeradres, onderwerp en HTML-body. De Adapter overbrugt dit verschil.
    /// </summary>
    public interface ISmtpServer
    {
        void SendMail(string recipient, string subject, string htmlBody);
    }
}
