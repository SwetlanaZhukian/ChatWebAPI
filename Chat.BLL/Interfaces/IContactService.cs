using Chat.Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Chat.BLL.Interfaces
{
   public interface IContactService
    {
        Task<Contact> AddContact(string id,string UserContactId);
        bool HasContact(string id, string contactId);
        Task DeleteContact(Contact contact);
        Task<Contact> GetContact(int id);
        Task<Contact> FindContact(string id, string UserContactId);
        Task<IEnumerable<Contact>> GetAll();
        Task<IEnumerable<Contact>> GetAllContactsInUser(string id);
        Task Edit(Contact contacts);
    }
}
