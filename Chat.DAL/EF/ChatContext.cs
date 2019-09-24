using System;
using System.Collections.Generic;
using System.Text;
using Chat.Models.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Proxies;


namespace Chat.DAL.EF
{
   public class ChatContext: IdentityDbContext<User>
    {
        public ChatContext(DbContextOptions<ChatContext> options)
          : base(options)
        {


        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserDialog>()
                .HasKey(x => new { x.UserId, x.DialogId });

            modelBuilder.Entity<Contact>()
              .HasOne(a => a.User)
              .WithMany(b => b.Contacts)
              .HasForeignKey(c => c.UserId);

            modelBuilder.Entity<Contact>()
            .HasOne(a => a.Friend)
            .WithMany(b => b.UserInContacts)
            .HasForeignKey(c => c.FriendId);

            modelBuilder.Entity<BlackList>()
            .HasOne(a => a.User)
            .WithMany(b => b.ContactBlackLists)
            .HasForeignKey(c => c.UserId);

            modelBuilder.Entity<BlackList>()
            .HasOne(a => a.ContactBlock)
            .WithMany(b => b.UserInBlackLists)
            .HasForeignKey(c => c.ContactBlockId);

            modelBuilder.Entity<Dialog>()
           .HasOne(a => a.User)
           .WithMany(b => b.UserDialogs)
           .HasForeignKey(c => c.UserId);

            modelBuilder.Entity<Dialog>()
            .HasOne(a => a.Contact)
            .WithMany(b => b.ContactDialogs)
            .HasForeignKey(c => c.ContactId);

            modelBuilder.Entity<File>()
               .HasOne(a => a.Message)
               .WithMany(b => b.Files)
               .HasForeignKey(c => c.MessageId);
        }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Dialog> Dialogs { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<File> Files { get; set; }
        public  DbSet<BlackList> BlackLists { get; set; }


    }
}
