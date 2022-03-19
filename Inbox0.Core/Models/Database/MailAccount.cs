using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inbox0.Core.Models.Database
{
    public class MailAccount : IEntity
    {
        public string Id { get; set; }
        public string OwnerId { get; set; }
        public string Name { get; set; }
        public string EmailAddress { get; set; }
        public string IncomingServer { get; set; }
        public string IncomingProtocol { get; set; }
        public int IncomingPort { get; set; }
        public string IncomingPassword { get; set; }
        public string OutgoingServer { get; set; }
        public string OutgoingProtocol { get; set; }
        public int OutgoingPort { get; set; }
        public string OutgoingPassword { get; set; }
        public uint LastUid { get; set; }
        public virtual AppUser Owner { get; set; }
        public virtual List<Inbox> Inboxes { get; set; }
    }
}
