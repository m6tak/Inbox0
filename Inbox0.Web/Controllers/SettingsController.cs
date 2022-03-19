using AutoMapper;
using Inbox0.Core.Models.Database;
using Inbox0.Core.Tools.General;
using Inbox0.Web.Models.Forms;
using Inbox0.Web.Models.ViewModels;
using Inbox0.Web.Services.EF;
using Inbox0.Web.Services.Repositories;
using Inbox0.Web.Services.Session;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Inbox0.Web.Controllers
{
    [Authorize]
    public class SettingsController : Controller
    {
        private readonly ISessionManager _session;
        private readonly MailAccountRepository _mailRepository;
        private readonly IMapper _mapper;

        public SettingsController(ISessionManager session, MailAccountRepository mailRepository, IMapper mapper)
        {
            _session = session;
            _mailRepository = mailRepository;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            var user = _session.GetUser(HttpContext, includeMailAccounts: true);
            if (user is null) return RedirectToAction("Login", "Home");

            var vm = new SettingsViewModel
            {
                User = user,
            };

            return View(vm);
        }

        [HttpPost]
        public IActionResult AddMailAccount(EmailForm form)
        {
            var user = _session.GetUser(HttpContext);
            if (user is null) return RedirectToAction("Login", "Home");

            var newAccount = _mapper.Map<MailAccount>(form);
            newAccount.Id = Id.New;
            newAccount.OwnerId = user.Id;

            _mailRepository.Add(newAccount);

            return RedirectToAction("Index");
        }

        [HttpDelete]
        public IActionResult DeleteMailAccount([FromQuery] string id)
        {
            var user = _session.GetUser(HttpContext);
            if (user is null) return RedirectToAction("Login", "Home");

            var accountToDelete = _mailRepository.GetById(id);
            _mailRepository.Delete(accountToDelete);

            return new OkResult();
        }
    }
}
