using Inbox0.Core.Models.Database;
using Inbox0.Core.Tools.General;
using Inbox0.Web.Models;
using Inbox0.Web.Models.ViewModels;
using Inbox0.Web.Services.EF;
using Inbox0.Web.Services.Mail;
using Inbox0.Web.Services.Repositories;
using Inbox0.Web.Services.Repositories.Base;
using Inbox0.Web.Services.Session;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Inbox0.Web.Controllers
{

    public class MailController : Controller
    {
        private readonly ISessionManager _session;
        private readonly IMailService _mailService;
        private readonly IGenericRepository<Inbox> _inboxRepo;
        private readonly MailConversationRepository _conversationRepo;
        private readonly MailMessageRepository _messageRepo;

        public MailController(IMailService mailService, ISessionManager session, IGenericRepository<Inbox> inboxRepo, MailConversationRepository conversationRepo, MailMessageRepository messageRepo)
        {
            _mailService = mailService;
            _session = session;
            _inboxRepo = inboxRepo;
            _conversationRepo = conversationRepo;
            _messageRepo = messageRepo;
        }

        public async Task<IActionResult> Inbox()
        {
            var user = _session.GetUser(HttpContext, includeMailAccounts: true);
            if (user is null) return RedirectToAction("Login", "Home");

            var userDefaultAccount = user.MailAccounts.FirstOrDefault();
            if (userDefaultAccount is null) return View(new InboxViewModel { User = user, NoMailAccount = true });

            var conversations = await _mailService.SyncAndGetAccountConversations(userDefaultAccount);

            var vm = new InboxViewModel
            {
                User = user,
                SelectedMailAccountId = userDefaultAccount.Id,
                LoadedConversations = conversations
            };

            return View(vm);
        }

        [HttpPost]
        public IActionResult Inbox([FromBody]SelectInboxRequest req)
        {
            var user = _session.GetUser(HttpContext, includeMailAccounts: true);
            if (user is null) return RedirectToAction("Login", "Home");

            var selectedAccount = user.MailAccounts.Where(x => x.Id == req.mailId).First();
            var conversations = _conversationRepo.GetByInboxId(req.inboxId).ToList();

            var vm = new InboxViewModel
            {
                User = user,
                SelectedMailAccountId = selectedAccount.Id,
                LoadedConversations = conversations
            };

            return PartialView("_InboxConversationTable", vm);
        }

        [HttpPost]
        public IActionResult OpenConversation([FromBody]OpenConversationRequest req)
        {
            var user = _session.GetUser(HttpContext);
            if (user is null) return RedirectToAction("Login", "Home");

            var conversation = _conversationRepo.GetById(req.conversationId);
            var messages = _messageRepo.GetByConversationId(req.conversationId);

            var vm = new InboxConversationViewModel
            {
                User = user,
                From = conversation.From,
                Subject = conversation.Subject,
                Messages = messages.Select(x => new MessageViewModel(x.Title, x.Message, x.From, x.To)).ToList()
            };

            return PartialView("_InboxConversation", vm);
        }

        [HttpGet]
        public IActionResult MoveToArchive(string mailId, string ids)
        {
            var user = _session.GetUser(HttpContext, includeMailAccounts: true);
            if (user is null) return RedirectToAction("Login", "Home");

            var userArchiveInbox = user.MailAccounts.First(x => x.Id == mailId).Inboxes.FirstOrDefault(x => x.Name == "Archive");

            if(userArchiveInbox is null)
            {
                var archiveInbox = new Inbox
                {
                    Id = Id.New,
                    OwnerId = user.MailAccounts.First(x => x.Id == mailId).Id,
                    Name = "Archive"
                };

                _inboxRepo.Add(archiveInbox);

                userArchiveInbox = archiveInbox;
            }

            var conversations = _conversationRepo.GetManyById(ids.Split('|')).ToList();

            conversations.ForEach(x =>
            {
                x.InboxId = userArchiveInbox.Id;
                _conversationRepo.Update(x);
            });

            return new OkResult();
        }
    }
}
