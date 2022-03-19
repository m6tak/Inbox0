using Inbox0.Core.Models.Database;
using Inbox0.Core.Models.Database.Relations;
using Microsoft.EntityFrameworkCore;

namespace Inbox0.Web.Services.EF
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.CreateInbox0Relations();
        }

        public virtual DbSet<Inbox> Inboxes { get; set; }
        public virtual DbSet<InboxMessage> InboxMessages { get; set; }
        public virtual DbSet<AppUser> Users { get; set; }
        public virtual DbSet<MailAccount> MailAccounts { get; set; }
        public virtual DbSet<InboxConversation> InboxConversations { get; set; }
    }
}
