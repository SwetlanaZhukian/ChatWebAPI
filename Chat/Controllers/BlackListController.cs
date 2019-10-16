using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Chat.BLL.Interfaces;
using Chat.Models.Entities;
using Chat.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Chat.Controllers
{
    [Route("api/blacklist")]
    public class BlackListController : Controller
    {
        private UserManager<User> userManager;
        private readonly IContactService contactService;
        private readonly IChatService chatService;
        private readonly IMapper mapper;
        private readonly IBlackListService blackListService;
        public BlackListController(UserManager<User> _userManager, IChatService _chatService, IMapper _mapper, IContactService _contactService, IBlackListService _blackListService)
        {
            userManager = _userManager;
            mapper = _mapper;
            contactService = _contactService;
            blackListService = _blackListService;
            chatService = _chatService;
        }
        [HttpGet]
        [Route("add/{id}")]
        public async Task<IActionResult> AddInBlock(string id)
        {
            string Id = User.Claims.First(c => c.Type == "Id").Value;
            bool hasUserInBlock =  blackListService.HasUserInBlock(Id, id);
            bool hasUserInContact = contactService.HasContact(Id, id);
            var contact = await contactService.FindContact(Id, id);
            if (hasUserInBlock)
            {
                return BadRequest();
            }          
            else
            {
                if (hasUserInContact)
                {
                    await contactService.DeleteContact(contact);
                }
                var user = await blackListService.AddUserInBlock(Id, id);

                return Ok(user);
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserFromBlackList(string id)
        {
            try
            {
                string Id = User.Claims.First(c => c.Type == "Id").Value;
                var user = await blackListService.FindUser(Id, id);
                if (user == null)
                {
                    return BadRequest();
                }
                await blackListService.DeleteUserFromBlock(user);
                return NoContent();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        [HttpGet]
        [Route("blockusers")]
        public async Task<object> GetAllBlockUsers()
        {
            string Id = User.Claims.First(c => c.Type == "Id").Value;
            var users = await blackListService.GetAllBlockUsersInUser(Id);
            var blockusers = mapper.Map<IEnumerable<BlackList>, List<ContactViewModel>>(users);
            foreach (var user in blockusers)
            {
                if (String.IsNullOrEmpty(user.Avatar))
                    user.Avatar = "\\Resources\\Images\\default.jpg";
                bool hasUserInBlock = blackListService.HasUserInBlock(Id, user.UserId);
                bool online = chatService.IsOnline(user.UserId);
                if (online)
                {
                    user.IsOnline = true;
                }
                else
                {
                    user.IsOnline = false;
                }
                if (hasUserInBlock)
                {
                    user.Status = true;
                }
            }

            return Ok(blockusers);

        }

    }
}
