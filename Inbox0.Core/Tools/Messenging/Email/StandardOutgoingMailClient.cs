using Inbox0.Core.Models.Database;
using Inbox0.Core.Tools.General;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inbox0.Core.Tools.Messenging.Email
{
    public class StandardOutgoingMailClient : IOutgoingMailClient
    {
        private bool _authorized = false;
        private MailAccount? _account = null;
        public void Authorize(MailAccount account)
        {
            _account = account;   
        }

        public async Task<InboxMessage?> SendAsync(EmailMessage message)
        {
            if (!_authorized || _account is null) return null;

            try
            {
                var email = new MimeMessage();

                email.From.Add(MailboxAddress.Parse(_account.EmailAddress));
                message.To.Split(',').ToList().ForEach(x => email.To.Add(MailboxAddress.Parse(x)));

                email.Subject = message.Subject;
                email.Body = new TextPart(TextFormat.Html) { Text = message.HtmlContent };

                using var smtp = new SmtpClient();
                smtp.Connect(_account.OutgoingServer, _account.OutgoingPort, SecureSocketOptions.StartTls);
                smtp.Authenticate(_account.EmailAddress, _account.OutgoingPassword);
                var result = await smtp.SendAsync(email);

                smtp.Disconnect(true);

                return new InboxMessage
                {
                    Message = message.HtmlContent,
                    Title = message.Subject,
                    From = _account.EmailAddress,
                    To = message.To,
                    CreatedAt = DateTime.UtcNow,
                    IsRead = true,
                    DeliveryDate = null,
                    Id = Id.New
                };
            }
            catch(Exception e)
            {
                return null;
            }
        }
    }
}
