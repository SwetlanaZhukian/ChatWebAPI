using Chat.DAL.EF;
using Chat.DAL.Interfaces;
using Chat.Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chat.DAL.Repositories
{
   public class EFUnitOfWork:IUnitOfWork
    {
        private ChatContext db;
        private RepositoryBase<Message> messageRepository;
        private RepositoryBase<Dialog> dialogRepository;
        private RepositoryBase<File> fileRepository;
        private RepositoryBase<User> userRepository;
        private ContactRepository contactRepository;
        private BlackListRepository blackListRepository;
       


        public EFUnitOfWork(ChatContext _db)
        {
            db = _db;
        }
        public IRepository<Contact> Contacts
        {
            get
            {
                if (contactRepository == null)
                    contactRepository = new ContactRepository(db);
                return contactRepository;
            }
        }

        //IDialogRepository<Dialog> Dialogs1 {
        //    get
        //    {
        //        if (dRepository == null)
        //            dRepository = new DialogRepository(db);
        //        return dRepository;
        //    }
        //}
        public IRepositoryBase<File> Files
        {
            get
            {
                if (fileRepository == null)
                    fileRepository = new RepositoryBase<File>(db);
                return fileRepository;
            }
        }
        public IRepository<BlackList> BlackLists
        {
            get
            {
                if (blackListRepository == null)
                    blackListRepository = new BlackListRepository(db);
                return blackListRepository;
            }
        }
        
        public IRepositoryBase<Message> Messages
        {
            get
            {
                if (messageRepository == null)
                    messageRepository = new RepositoryBase<Message>(db);
                return messageRepository;
            }
        }
        public IRepositoryBase<User> Users
        {
            get
            {
                if (userRepository == null)
                    userRepository = new RepositoryBase<User>(db);
                return userRepository;
            }
        }
        public IRepositoryBase<Dialog> Dialogs
        {
            get
            {
                if (dialogRepository == null)
                    dialogRepository = new RepositoryBase<Dialog>(db);
                return dialogRepository;
            }
        }

        

        public void Save()
        {
            db.SaveChanges();
        }

        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    db.Dispose();
                }
                this.disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
