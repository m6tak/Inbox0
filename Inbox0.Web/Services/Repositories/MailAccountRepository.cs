using Inbox0.Core.Models.Database;
using Inbox0.Core.Tools.General;
using Inbox0.Web.Services.EF;
using Inbox0.Web.Services.Repositories.Base;

namespace Inbox0.Web.Services.Repositories
{
    public class MailAccountRepository : GenericRepository<MailAccount>
    {
        private readonly IGenericRepository<Inbox> _inboxesRepository;
        public MailAccountRepository(AppDbContext context, IGenericRepository<Inbox> inboxesRepository) : base(context)
        {
            _inboxesRepository = inboxesRepository;
        }

        new public void Add(MailAccount account)
        {
            base.Add(account);
            _inboxesRepository.Add(new Inbox
            {
                Id = Id.New,
                Name = "Main",
                OwnerId = account.Id
            });
        }
    }
}
