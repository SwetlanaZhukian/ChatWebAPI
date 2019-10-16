using Chat.BLL.DTO;
using Chat.Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Chat.BLL.Interfaces

{public interface IUserService
    {
        Task<IEnumerable<User>> Search(string id, string str);
        Task SetLastTimeOnline(string id);
    }
}
