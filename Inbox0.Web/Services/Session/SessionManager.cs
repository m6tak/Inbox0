using Inbox0.Core.Models.Database;
using Inbox0.Web.Services.EF;
using Inbox0.Web.Services.Session;
using Microsoft.EntityFrameworkCore;

namespace Inbox0.Web.Services.Session
{
    public class SessionManager : ISessionManager
    {
        private readonly AppDbContext _context;

        public SessionManager(AppDbContext context)
        {
            _context = context;
        }

        public AppUser? GetUser(HttpContext httpContext, bool includeMailAccounts = false)
        {
            var uid = httpContext.Session.GetString("uid");
            if (uid is null) return null;

            var users = _context.Users.Where(x => x.Id == uid);

            if (includeMailAccounts) users = users.Include(x => x.MailAccounts).ThenInclude(x => x.Inboxes);

            return users.FirstOrDefault(x => x.Id == uid);
        }
    }
}
