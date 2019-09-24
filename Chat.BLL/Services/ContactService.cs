using Chat.BLL.Interfaces;
using Chat.DAL.EF;
using Chat.DAL.Interfaces;
using Chat.Models.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.BLL.Services
{
   public class ContactService:IContactService
    {
        IUnitOfWork db { get; set; }
        private UserManager<User> userManager;
        private ChatContext context;
        public ContactService(IUnitOfWork _db, ChatContext _context, UserManager<User> _userManager)
        {
            db = _db;
            context = _context;
            userManager = _userManager;
        }
        public bool HasContact(string id, string UserContactId)
        {
         var contact= db.Contacts.HasContact(id, UserContactId);
            return contact;
        }
        public async Task<Contact> AddContact(string id,string UserContactId)
        {
            try
            {
                //var user1 = await userManager.FindByIdAsync(UserContactId);
                //var user2 = await userManager.FindByIdAsync(id);

                var contact = new Contact
                {
                    UserId = id,
                    FriendId = UserContactId,  

                };
                var usercontact = new Contact
                {
                    UserId = UserContactId,
                    FriendId = id,

                };

                await db.Contacts.Create(contact);
                await db.Contacts.Save();
                return contact;
            }
            catch(Exception ex)
            {
                throw ex;
            }
            
       }
        public async Task DeleteContact(Contact contact)
        {
            await db.Contacts.Delete(contact);
         }
        public async Task<Contact> GetContact(int id)
        {
            return await db.Contacts.Get(id);
        }
        public async Task<Contact> FindContact(string id, string UserContactId)
        {
            return await db.Contacts.ContactInUser(id, UserContactId);
        }
        public async Task<IEnumerable<Contact>> GetAll()
        {
            return await db.Contacts.GetAll();
        }
        public async Task<IEnumerable<Contact>> GetAllContactsInUser(string id)
        {
           var contacts = await GetAll();
            var contactsInUser = contacts.Where(p => p.UserId == id);
            return contactsInUser;
        }
        public async Task Edit(Contact contacts)
        {
          await db.Contacts.Update(contacts);
        }
    }
}
