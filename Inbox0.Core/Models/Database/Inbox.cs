using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inbox0.Core.Models.Database
{
    public class Inbox : IEntity
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string OwnerId { get; set; }
        public virtual MailAccount Owner { get; set; }
        public virtual List<InboxMessage> Messages { get; set; }
        public virtual List<InboxConversation> Conversations { get; set; }
    }
}
