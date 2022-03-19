using Inbox0.Core.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inbox0.Core.Tools.Messenging.Email
{
    public interface IIncomingMailClient
    {
        public Task<List<InboxMessage>> FetchMessages(EmailFilterModel filters);
    }

    public class EmailFilterModel
    {
        public DateTime? DateFrom { get; init; }
        public DateTime? DateTo { get; init; }
        public string? FilterSubject { get; init; } = "";
        public string? FilterFrom { get; init; } = "";
        public int Limit { get; init; } = 100;
    }
}
