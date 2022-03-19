using Inbox0.Core.Helpers.Security;
using Inbox0.Core.Models.Database;
using Inbox0.Core.Tools.General;
using Inbox0.Core.Tools.Messenging.Email;
using Inbox0.Web.Models;
using Inbox0.Web.Models.Forms;
using Inbox0.Web.Models.ViewModels;
using Inbox0.Web.Services.EF;
using Inbox0.Web.Services.Mail;
using Inbox0.Web.Services.Repositories;
using Inbox0.Web.Services.Repositories.Base;
using Inbox0.Web.Services.Session;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace Inbox0.Web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ISessionManager _session;

        public HomeController(ISessionManager session,AppDbContext context)
        {
            _session = session;
            _context = context;
        }

        public IActionResult Index()
        {
            var user = _session.GetUser(HttpContext, includeMailAccounts: true);
            if (user is null) return RedirectToAction("Login");

            return RedirectToAction("Inbox", "Mail");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            HttpContext.Session.Clear();
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string login, string password)
        {
            try
            {
                var user = _context.Users.FirstOrDefault(x => x.Name == login);
                if (user is null) throw new Exception();

                if (!PasswordHasher.Verify(user.Password, password)) throw new UnauthorizedAccessException();

                var claims = new List<Claim> { new Claim("Authorized", user.Name) };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(principal);

                HttpContext.Session.SetString("uid", user.Id);

                return RedirectToAction("Inbox", "Mail");
            }
            catch
            {
                ViewBag.Error = "Nieprawidłowe dane logowania";
                return View();
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult Register(RegisterForm form)
        {
            try
            {
                if (form.Password != form.PasswordRepeat) throw new Exception("Podane hasła są różne");
                if (!PasswordPolicy.VerifyPassword(form.Password)) throw new Exception("Hasło nie spełnia wymagań");
                if (_context.Users.Any(u => u.Name == form.Name)) throw new Exception("Konto z tym adresem email już istnieje");

                var newUser = new AppUser
                {
                    Id = Id.New,
                    Name = form.Name,
                    Password = PasswordHasher.Hash(form.Password),
                };


                _context.Users.Add(newUser);
                _context.SaveChanges();

                return RedirectToAction("Login");
            }
            catch (Exception e)
            {
                ViewBag.Error = e.Message;
                return View();
            }
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}