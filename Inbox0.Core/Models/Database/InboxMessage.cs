using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inbox0.Core.Models.Database
{
    public class InboxMessage : IEntity
    {
        public string Id { get; set; }
        public string ExternalId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public string Message { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string ConversationId { get; set; }
        public string InboxId { get; set; }
        public string Title { get; set; }
        public bool IsRead { get; set; }
        public string? ReplyToExternalId { get; set; }
        public virtual InboxConversation Conversation { get; set; }
        public virtual Inbox Inbox { get; set; }
    }
}
