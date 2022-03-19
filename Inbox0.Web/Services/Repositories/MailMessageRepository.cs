using Inbox0.Core.Models.Database;
using Inbox0.Web.Services.EF;
using Microsoft.EntityFrameworkCore;

namespace Inbox0.Web.Services.Repositories.Base
{
    public class MailMessageRepository : GenericRepository<InboxMessage>
    {
        public MailMessageRepository(AppDbContext context) : base(context)
        {
        }

        public IEnumerable<InboxMessage> GetByConversationId(string covnersationId)
        {
            return _context.InboxMessages.Where(x => x.ConversationId == covnersationId).ToList();
        }
    }
}
