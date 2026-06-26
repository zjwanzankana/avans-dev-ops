using System;

namespace Domain.Notifications.External
{
    /// <summary>Stub-implementatie van een externe SMTP-library (infrastructure ring).</summary>
    public class SmtpServer : ISmtpServer
    {
        public void SendMail(string recipient, string subject, string htmlBody)
        {
            Console.WriteLine($"[SMTP] To: {recipient} | Subject: {subject} | Body: {htmlBody}");
        }
    }
}
