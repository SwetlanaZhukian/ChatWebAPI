using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Chat.DAL.Interfaces
{
   public interface IRepositoryBase<T> 
    {
      Task <IEnumerable<T>> FindAll();
      Task <IEnumerable<T>> FindByCondition(Expression<Func<T, bool>> expression);
      Task<T> GetByCondition(Expression<Func<T, bool>> expression);
      Task<T> Get(int id);
      Task<T> Get(string id);
      Task Create(T item);
      Task<T> Update(T item, object key);
      Task<int> Delete(T item);
        Task Edit(T item);
     
        Task SaveAsync();

    }
}
