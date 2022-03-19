using Inbox0.Core.Models.Database;

namespace Inbox0.Web.Services.Mail
{
    public interface IMailService
    {
        public Task<List<InboxConversation>> SyncAndGetAccountConversations(MailAccount account);
    }
}
