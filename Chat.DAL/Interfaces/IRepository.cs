using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Chat.DAL.Interfaces
{
  public  interface IRepository<T> where T : class
    {
        Task<T> Get( int id);
        Task<IEnumerable<T>> GetAll();
        Task Update(T item);
        Task Create(T item);
        Task Delete(T item);
        Task Save();
        bool HasContact(string id, string contactId);
        Task<T> ContactInUser(string id, string contactId);
       
    }
}
