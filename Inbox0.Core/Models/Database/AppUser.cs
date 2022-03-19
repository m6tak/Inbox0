using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inbox0.Core.Models.Database
{
    public class AppUser : IEntity
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public virtual List<MailAccount> MailAccounts { get; set; }
    }
}
