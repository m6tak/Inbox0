using Inbox0.Core.Models.Database;

namespace Inbox0.Web.Services.Session
{
    public interface ISessionManager
    {
        public AppUser? GetUser(HttpContext httpContext, bool includeMailAccounts = false);
    }
}
