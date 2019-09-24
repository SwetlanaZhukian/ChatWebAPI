using Chat.DAL.EF;
using Chat.DAL.Interfaces;
using Chat.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.DAL.Repositories
{
   public class ContactRepository:IRepository<Contact>
    {
        private ChatContext db;

        public ContactRepository(ChatContext context)
        {
            this.db = context;
        }
        public async Task<IEnumerable<Contact>> GetAll()
        {
            return await db.Contacts.ToListAsync();
        }

        public async Task<Contact> Get(int id)
        {
            return await db.Contacts.FindAsync(id);
        }

        public async Task Create(Contact contacts)
        {
            await db.Contacts.AddAsync(contacts);
        }
        public async Task Update(Contact contacts)
        {
            db.Entry(contacts).State = EntityState.Modified;
            await Save();
        }
        public async Task Delete(Contact contact)
        {
                db.Contacts.Remove(contact);
            await Save();
        }
        public async Task Save()
        {
            await db.SaveChangesAsync();
        }
       public bool HasContact(string id, string contactId)
        {
            return db.Contacts.Any(p => p.UserId == id && p.FriendId == contactId);

        }
       public async Task<Contact> ContactInUser(string id, string contactId)
        {
            Contact contact= await db.Contacts.SingleOrDefaultAsync(p => p.UserId == id && p.FriendId == contactId);
            return contact;
        }


    }
}
