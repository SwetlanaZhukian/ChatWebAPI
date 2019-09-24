using Chat.Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Chat.BLL.Interfaces
{
   public interface IBlackListService
    {
        Task<BlackList> AddUserInBlock(string id, string blockUserId);
        bool HasUserInBlock(string id, string blockUserId);
        Task DeleteUserFromBlock(BlackList user);
        Task<BlackList> GetUser(int id);
        Task<BlackList> FindUser(string id, string blockUserId);
        Task<IEnumerable<BlackList>> GetAll();
        Task<IEnumerable<BlackList>> GetAllBlockUsersInUser(string id);
    }
}
