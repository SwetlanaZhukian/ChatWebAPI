using Chat.Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chat.DAL.Interfaces
{
    public interface IUnitOfWork
    {
        IRepository<Contact> Contacts { get; }
        IRepository<BlackList> BlackLists { get; }
        IRepositoryBase<Message> Messages { get; }
        IRepositoryBase<Dialog> Dialogs { get; }
        IRepositoryBase<File> Files { get; }
        void Save();
        void Dispose(bool disposing);
    }
}
