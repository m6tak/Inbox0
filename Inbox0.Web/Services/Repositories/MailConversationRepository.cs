using Inbox0.Core.Models.Database;
using Inbox0.Web.Services.EF;
using Inbox0.Web.Services.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace Inbox0.Web.Services.Repositories
{
    public class MailConversationRepository : GenericRepository<InboxConversation>
    {
        public MailConversationRepository(AppDbContext context) : base(context)
        {
        }

        public IEnumerable<InboxConversation> GetByInboxId(string inboxId, int limit = 100)
        {
            return _context.InboxConversations
                .Where(x => x.InboxId == inboxId)
                .OrderByDescending(x => x.LastMessageDate)
                .Take(limit).ToList();
        }

        public IEnumerable<InboxConversation> GetByInbox(Inbox inbox, int limit = 100)
        {
            return GetByInboxId(inbox.Id, limit);
        }

        public IEnumerable<InboxConversation> GetByUserId(string userId)
        {
            var querySet = _context.Users.Where(x => x.Id == userId)
                .Include(x => x.MailAccounts)
                    .ThenInclude(x => x.Inboxes)
                        .ThenInclude(x => x.Conversations);

            return querySet.SelectMany(x => x.MailAccounts).SelectMany(x => x.Inboxes).SelectMany(x => x.Conversations).ToList();
        }

        public IEnumerable<InboxConversation> GetManyById(IEnumerable<string> ids)
        {
            return _context.InboxConversations.Where(x => ids.Contains(x.Id)).ToList();
        }
    }
}
