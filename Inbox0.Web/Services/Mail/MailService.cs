using Inbox0.Core.Models.Database;
using Inbox0.Core.Tools.General;
using Inbox0.Core.Tools.Messenging.Email;
using Inbox0.Web.Services.EF;
using Inbox0.Web.Services.Repositories;
using Inbox0.Web.Services.Repositories.Base;

namespace Inbox0.Web.Services.Mail
{
    public class MailService : IMailService
    {
        private readonly AppDbContext _context;
        private readonly StandardIncomingMailClient _incomingMails;
        private readonly MailConversationRepository _conversationRepo;
        private readonly MailMessageRepository _messageRepo;

        public MailService(MailMessageRepository messageRepo, MailConversationRepository conversationRepo, IIncomingMailClient incomingMails, AppDbContext context)
        {
            _messageRepo = messageRepo;
            _conversationRepo = conversationRepo;
            _incomingMails = (StandardIncomingMailClient)incomingMails;
            _context = context;
        }

        public async Task<List<InboxConversation>> SyncAndGetAccountConversations(MailAccount account)
        {
            var defaultInbox = account.Inboxes.First(x => x.Name == "Main");

            _incomingMails.Authorize(account);
            var (newLastUid, fetchedMessages) = await _incomingMails.FetchNewMessages();

            var existingConversations = _conversationRepo.GetByUserId(account.OwnerId);
            var fetchedConversations = fetchedMessages.GroupBy(x => x.Title).ToList();

            foreach (var conversation in fetchedConversations)
            {
                var dbConversation = existingConversations.FirstOrDefault(x => x.Subject == conversation.Key) ?? new InboxConversation
                {
                    Id = Id.New,
                    IsNew = true,
                    From = conversation.First().From,
                    InboxId = defaultInbox.Id,
                    LastMessageDate = conversation.OrderBy(x => x.CreatedAt).Last().CreatedAt,
                    MessageCount = conversation.Count(),
                    Subject = conversation.Key
                };

                if (dbConversation.IsNew) _conversationRepo.Add(dbConversation);

                foreach (var msg in conversation)
                {
                    msg.ConversationId = dbConversation.Id;
                    msg.InboxId = defaultInbox.Id;
                }

                _messageRepo.AddRange(conversation.ToList());
            }

            if (newLastUid != 0) account.LastUid = newLastUid;
            _context.Entry(account).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();

            return _conversationRepo.GetByInbox(defaultInbox).ToList();
        }
    }
}
