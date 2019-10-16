using Chat.DAL.EF;
using Chat.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Chat.DAL.Repositories
{
  public  class RepositoryBase<T>:IRepositoryBase<T> where T:class
    {
        protected ChatContext db { get; set; }
        public RepositoryBase(ChatContext _db)
        {
            db = _db;
        }

        public async Task<IEnumerable<T>> FindAll()
        {
            return await db.Set<T>().ToListAsync();
        }

        public async Task<IEnumerable<T>> FindByCondition(Expression<Func<T, bool>> expression)
        {
            return await db.Set<T>().Where(expression).ToListAsync();
        }
        public async   Task<T> GetByCondition(Expression<Func<T, bool>> expression)
        {
            return await db.Set<T>().SingleOrDefaultAsync(expression);
        }
        public async Task<T> Get(int id)
        {
            return await db.Set<T>().FindAsync(id);
        }

        public async Task<T> Get(string id)
        {
            return await db.Set<T>().FindAsync(id);
        }

        public async Task Create(T item)
        {
            await db.Set<T>().AddAsync(item);

        }

        public async Task<T> Update(T item,object key)
        {
            if (item == null)
                return null;
            T exist = await db.Set<T>().FindAsync(key);
            if (exist != null)
            {
                db.Entry(exist).CurrentValues.SetValues(item);
                await db.SaveChangesAsync();
            }
            return exist;
          
        }
        public async Task Edit(T item)
        {
            db.Entry(item).State = EntityState.Modified;
            await SaveAsync();
        }
        public async Task<int> Delete(T item)
        {
            db.Set<T>().Remove(item);
            return await db.SaveChangesAsync();
        }
        

        public async Task SaveAsync()
        {
            await db.SaveChangesAsync();
        }

        
    }
}
