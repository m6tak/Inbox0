using Inbox0.Core.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inbox0.Core.Tools.Messenging.Email
{
    public interface IOutgoingMailClient
    {
        public Task<InboxMessage?> SendAsync(EmailMessage message);
    }

    public class EmailMessage
    {
        public string To { get; init; }
        public string HtmlContent { get; init; }
        public string Subject { get; set; }
    }
}
