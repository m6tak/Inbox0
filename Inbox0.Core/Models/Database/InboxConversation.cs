using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inbox0.Core.Models.Database
{
    public class InboxConversation : IEntity
    {
        public string Id { get; set; }
        public string Subject { get; set; }
        public string InboxId { get; set; }
        public DateTime LastMessageDate { get; set; }
        public int MessageCount { get; set; }
        public string From { get; set; }
        public virtual List<InboxMessage> Messages { get; set; }
        public virtual Inbox Inbox { get; set; }

        public bool IsNew { get; init; } = false;
    }
}
