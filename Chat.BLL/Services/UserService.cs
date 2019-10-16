using Chat.BLL.DTO;
using Chat.BLL.Interfaces;
using Chat.Models.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using System.Linq;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Chat.DAL.Interfaces;

namespace Chat.BLL.Services
{
   public class UserService:IUserService
    {
        private UserManager<User> userManager;
        private readonly IMapper mapper;
        IUnitOfWork db { get; set; }
        public UserService(UserManager<User> _userManager,IMapper _mapper, IUnitOfWork _db)
        {
            userManager = _userManager;
            mapper = _mapper;
            db = _db;
        }
       
        public async Task <IEnumerable<User>> Search(string id,string str)
        {
          return  await userManager.Users.Where(h => h.Email.Contains(str) || h.PhoneNumber.Contains(str)||h.Name.Contains(str)).Where(p => p.Id != id).ToListAsync();
            
        }
        public async Task SetLastTimeOnline(string id)
        {
            User user = await userManager.FindByIdAsync(id);
            user.LastTimeOnline = DateTime.Now;
            await db.Users.Edit(user);
        }

    }
}
