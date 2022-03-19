using Inbox0.Core.Models.Database;
using Inbox0.Core.Tools.General;
using MailKit;
using MailKit.Net.Imap;
using MailKit.Search;

namespace Inbox0.Core.Tools.Messenging.Email
{
    public class StandardIncomingMailClient : IIncomingMailClient
    {
        private bool _authorized = false;
        private MailAccount? _account = null;
        public void Authorize(MailAccount account)
        {
            _account = account;
            _authorized = true;
        }

        public async Task<List<InboxMessage>> FetchMessages(EmailFilterModel filters)
        {
            if (!_authorized || _account is null) return new List<InboxMessage>();

            try
            {
                using var client = new ImapClient();
                client.Connect(_account.IncomingServer, _account.IncomingPort);
                client.Authenticate(_account.EmailAddress, _account.IncomingPassword);
                var inbox = client.Inbox;
                await inbox.OpenAsync(FolderAccess.ReadOnly);

                var minDate = new DateTimeOffset(filters.DateFrom ?? DateTime.MinValue);
                var maxDate = new DateTimeOffset(filters.DateTo ?? DateTime.MaxValue);
                var from = filters.FilterFrom;
                var subject = filters.FilterSubject!;
                var limit = filters.Limit;

                var messages = inbox.OrderByDescending(x => x.Date)
                    .Where(x => x.Date > minDate && x.Date < maxDate && x.Subject.Contains(subject) && x.From.ToString().Contains(from))
                    .Take(limit)
                    .ToList();

                var inboxMessages = messages.Select(x => new InboxMessage
                {
                    Id = Id.New,
                    ExternalId = x.MessageId,
                    CreatedAt = x.Date.DateTime,
                    DeliveryDate = null,
                    From = x.From.ToString(),
                    To = x.To.ToString(),
                    IsRead = false,
                    Message = x.HtmlBody,
                    Title = x.Subject,
                    ReplyToExternalId = x.InReplyTo,
                });

                return inboxMessages.OrderBy(x => x.CreatedAt).ToList();
            }
            catch (Exception)
            {
                return new List<InboxMessage>();
            }
        }

        public async Task<Tuple<uint, List<InboxMessage>>> FetchNewMessages()
        {
            if (!_authorized || _account is null) return new (0, new List<InboxMessage>());

            try
            {
                using var client = new ImapClient();
                client.Connect(_account.IncomingServer, _account.IncomingPort, MailKit.Security.SecureSocketOptions.None);
                client.Authenticate(_account.EmailAddress, _account.IncomingPassword);
                var inbox = client.Inbox;
                await inbox.OpenAsync(FolderAccess.ReadOnly);

                var range = new UniqueIdRange(new UniqueId(_account.LastUid == 0 ? 1 : _account.LastUid + 1), UniqueId.MaxValue);

                var uids = inbox.Search(range, SearchQuery.All);
                uids.Remove(new UniqueId(_account.LastUid));

                var messages = uids.Select(x => inbox.GetMessage(x)).Select(x => new InboxMessage
                {
                    Id = Id.New,
                    ExternalId = x.MessageId,
                    CreatedAt = x.Date.DateTime,
                    DeliveryDate = null,
                    From = x.From.ToString(),
                    To = x.To.ToString(),
                    IsRead = false,
                    Message = x.HtmlBody,
                    Title = x.Subject,
                    ReplyToExternalId = x.InReplyTo,
                });

                return new (uids.OrderBy(x => x.Id).Last().Id, messages.OrderBy(x => x.CreatedAt).ToList());
            }
            catch (Exception)
            {
                return new(0, new List<InboxMessage>());
            }
        }
    }
}
