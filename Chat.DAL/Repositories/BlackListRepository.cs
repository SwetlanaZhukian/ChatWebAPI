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
   public class BlackListRepository: IRepository<BlackList>
    {
        private ChatContext db;

        public BlackListRepository(ChatContext context)
        {
            this.db = context;
        }
        public async Task<IEnumerable<BlackList>> GetAll()
        {
            return await db.BlackLists.ToListAsync();
        }

        public async Task<BlackList> Get(int id)
        {
            return await db.BlackLists.FindAsync(id);
        }

        public async Task Create(BlackList users)
        {
            await db.BlackLists.AddAsync(users);
        }
        public async Task Update(BlackList users)
        {
            db.Entry(users).State = EntityState.Modified;
            await Save();
        }


        public async Task Delete(BlackList user)
        {
            db.BlackLists.Remove(user);
            await Save();
        }
        public async Task Save()
        {
            await db.SaveChangesAsync();
        }
        public bool HasContact(string id, string contactId)
        {
            return db.BlackLists.Any(p =>p.UserId == id && p.ContactBlockId == contactId);

        }
        public async Task<BlackList> ContactInUser(string id, string contactId)
        {
            BlackList user = await db.BlackLists.SingleOrDefaultAsync(p => p.UserId == id && p.ContactBlockId == contactId);
            return user;
        }

    }
}
