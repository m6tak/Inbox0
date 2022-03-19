using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inbox0.Core.Models.Database.Relations
{
    public static class ModelBuilderExtensions
    {
        public static void CreateInbox0Relations(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Inbox>().HasKey(e => e.Id);
            modelBuilder.Entity<InboxMessage>().HasKey(e => e.Id);
            modelBuilder.Entity<AppUser>().HasKey(e => e.Id);
            modelBuilder.Entity<InboxConversation>().HasKey(e => e.Id);
            modelBuilder.Entity<MailAccount>().HasKey(e => e.Id);

            modelBuilder.Entity<InboxConversation>().Ignore(x => x.IsNew);

            modelBuilder.Entity<InboxConversation>()
                .HasOne(x => x.Inbox)
                .WithMany(x => x.Conversations)
                .HasForeignKey(x => x.InboxId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<InboxMessage>()
                .HasOne(x => x.Conversation)
                .WithMany(x => x.Messages)
                .HasForeignKey(x => x.ConversationId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<MailAccount>()
                .HasMany(x => x.Inboxes)
                .WithOne(x => x.Owner)
                .HasForeignKey(x => x.OwnerId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<AppUser>()
                .HasMany(x => x.MailAccounts)
                .WithOne(x => x.Owner)
                .HasForeignKey(x => x.OwnerId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Inbox>()
                .HasMany(x => x.Messages)
                .WithOne(x => x.Inbox)
                .HasForeignKey(x => x.InboxId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
