using Chat.BLL.Interfaces;
using Chat.DAL.EF;
using Chat.DAL.Interfaces;
using Chat.Models.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chat.BLL.Services
{
    public class BlackListService:IBlackListService
    {
        IUnitOfWork db { get; set; }
        private UserManager<User> userManager;
        private ChatContext context;
        public BlackListService(IUnitOfWork _db, ChatContext _context, UserManager<User> _userManager)
        {
            db = _db;
            context = _context;
            userManager = _userManager;
        }
        public  bool HasUserInBlock(string id, string blockUserId)
        {
            //var user = await userManager.FindByIdAsync(id);
            //var list = user.UserInBlackLists.Union(user.ContactBlackLists);
            //var userInList = list.Any(p => (p.UserId == id && p.ContactBlockId == blockUserId) || (p.UserId == blockUserId && p.ContactBlockId == id));
           var user = db.BlackLists.HasContact(id, blockUserId);
            return user;
        }
        public async Task<BlackList> AddUserInBlock(string id, string blockUserId)
        { 
            try
            {
                var userInBlock = new BlackList
                {
                    UserId = id,
                    ContactBlockId = blockUserId,

                };
                var currentUserInBlock = new BlackList
                {
                    UserId = blockUserId,
                    ContactBlockId = id,

                };

                await db.BlackLists.Create(userInBlock);
                await db.Contacts.Save();
                return userInBlock;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public async Task DeleteUserFromBlock(BlackList user)
        {
            await db.BlackLists.Delete(user);
        }
        public async Task<BlackList> GetUser(int id)
        {
            return await db.BlackLists.Get(id);
        }
        public async Task<BlackList> FindUser(string id, string blockUserId)
        {
            return await db.BlackLists.ContactInUser(id, blockUserId);
        }
        public async Task<IEnumerable<BlackList>> GetAll()
        {
            return await db.BlackLists.GetAll();
        }
        public async Task<IEnumerable<BlackList>> GetAllBlockUsersInUser(string id)
        {
            var users = await GetAll();
            var blockUsers = users.Where(p => p.UserId == id);
            return blockUsers;
        }
    }
}
