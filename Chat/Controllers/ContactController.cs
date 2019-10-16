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
    [Route("api/contact")]
    public class ContactController : Controller
    {
        private UserManager<User> userManager;
        private readonly IContactService contactService;
        private readonly IChatService chatService;
        private readonly IMapper mapper;
        private readonly IBlackListService blackListService;
        public ContactController(UserManager<User> _userManager, IChatService _chatService, IMapper _mapper, IContactService _contactService,IBlackListService _blackListService)
        {
            userManager = _userManager;
            mapper = _mapper;
            contactService = _contactService;
            blackListService = _blackListService;
            chatService = _chatService;


        }
        [HttpGet]
       [Route("add/{id}")]
        public async Task<IActionResult> Add(string id)
        {
            string Id = User.Claims.First(c => c.Type == "Id").Value;
            bool hasContact = contactService.HasContact(Id, id);
            if (hasContact)
            {
                return BadRequest();
            }
            else
            {
                var contact = await contactService.AddContact(Id, id);

                return Ok(contact);
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> ContactDelete(string id)
        {
            try
            {
                string Id = User.Claims.First(c => c.Type == "Id").Value;
                var contact = await contactService.FindContact(Id,id);
                if (contact==null)
                {
                    return BadRequest();
                }
                await contactService.DeleteContact(contact);
                return NoContent();
            }
            catch(Exception ex)
            {
                throw ex;
            }

        }
        [HttpGet]
        [Route("profile/{id}")]
        public async Task<object> GetContactProfile(string id)
        {
            string Id = User.Claims.First(c => c.Type == "Id").Value;
            var user = await userManager.FindByIdAsync(id);
            if (user==null)
            {
                return NotFound();
            }
            var userprofile= mapper.Map<ContactViewModel>(user);

            if (String.IsNullOrEmpty(userprofile.Avatar))
            {

                userprofile.Avatar = "\\Resources\\Images\\default.jpg";
            }
            bool hasContact = contactService.HasContact(Id, user.Id);
            bool hasInBlock =  blackListService.HasUserInBlock(Id, user.Id);
            if (hasContact)
                {
                    userprofile.UserInContact = true;
                }
            if(hasInBlock)
            {
                userprofile.Status = true;
            }
            return Ok(userprofile);
        }
    
    [HttpGet]
    [Route("contact")]
    public async Task<object> GetContact()
    {
        string Id = User.Claims.First(c => c.Type == "Id").Value;
        var contacts = await contactService.GetAllContactsInUser(Id);
        var users = mapper.Map<IEnumerable<Contact>, List<ContactViewModel>>(contacts);
            foreach (var user in users)
            {
                if (String.IsNullOrEmpty(user.Avatar))
                user.Avatar = "\\Resources\\Images\\default.jpg";

                bool hasContact = contactService.HasContact(Id, user.Id);
                bool hasInBlock = blackListService.HasUserInBlock(Id, user.UserId);
                // bool hasInBlockContact = blackListService.HasUserInBlock(user.UserId, Id);
                bool online = chatService.IsOnline(user.UserId);
                if (online)
                {
                    user.IsOnline = true;
                }
                else
                {
                    user.IsOnline = false;
                }
                if (hasContact)
                {
                    user.UserInContact = true;
                }
                if (hasInBlock)
                {
                    user.Status = true;
                }
            }

        return Ok(users);

        }
        
        [HttpPut("{id}")]
        public async Task<ActionResult> EditContact( string id,[FromBody] EditViewModel model)
        {
            try
            {
                string userId = User.Claims.First(c => c.Type == "Id").Value;
                var user = await userManager.FindByIdAsync(userId);
                var contact = await contactService.FindContact(userId, id);
                contact.NickName = model.Name;
                if (!ModelState.IsValid)
                {
                    return BadRequest("Invalid model object");
                }
                await contactService.Edit(contact);
                return NoContent();

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

    }


}
