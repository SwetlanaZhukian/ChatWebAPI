using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Chat.BLL.Interfaces;
using Chat.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Chat.ViewModels;
using Chat.BLL.DTO;
using System.IO;
using System.Net.Http.Headers;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Chat.Controllers
{
    [Route("api/user")]
    public class UserController : Controller
    {
        private UserManager<User> userManager;
        private readonly IUserService userService;
        private readonly IContactService contactService;
        private readonly IMapper mapper;
        private readonly IFileManager fileManager;
        private readonly IBlackListService blackListService;
        public UserController(UserManager<User> _userManager,IUserService _userService,IMapper _mapper,IFileManager _fileManager,IContactService _contactService, IBlackListService _blackListService)
        {
            userManager = _userManager;
            userService = _userService;
            mapper = _mapper;
            fileManager = _fileManager;
            contactService = _contactService;
            blackListService = _blackListService;

        }
        [HttpGet]
        [Authorize]
        [Route("profile")]
        public async Task<object> GetUserProfile()
        {
            string userId = User.Claims.First(c => c.Type == "Id").Value;
            var user = await userManager.FindByIdAsync(userId);
             if (String.IsNullOrEmpty(user.Avatar))
            user.Avatar = "\\Resources\\Images\\default.jpg";
            return mapper.Map<ProfileViewModel>(user);
        }

        [HttpPut] 
        public async Task<ActionResult> EditUser( [FromBody] EditViewModel model)
        {
            try
            {
               
                string userId = User.Claims.First(c => c.Type == "Id").Value;
               var user = await userManager.FindByIdAsync(userId);
                user = mapper.Map<EditViewModel,User>(model,user);
                if (!ModelState.IsValid)
                {
                    return BadRequest("Invalid model object");
                }
                await userManager.UpdateAsync(user);
                return NoContent();
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
           
        }
        
        [HttpPost]
        [Route("avatar")]
        public async Task<IActionResult> Upload()
        {
            try
            {
                var file = Request.Form.Files[0];
                string userId = User.Claims.First(c => c.Type == "Id").Value;
                var user = await userManager.FindByIdAsync(userId);
                if (file.ContentType == "image/jpeg" || file.ContentType == "image/pjpeg" || file.ContentType == "image/png")
                {
                    var dbPath = fileManager.SaveImage(file);
                    user.Avatar = dbPath;
                    await userManager.UpdateAsync(user);
                    return Ok(new { dbPath });
                }
                else return BadRequest();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpGet]
        [Route("search")]
        public async Task<IActionResult> Search( string str)
        {
            string Id = User.Claims.First(c => c.Type == "Id").Value;
            if (String.IsNullOrEmpty(str))
            {
                return NotFound();
            }
            var search = await userService.Search(Id, str);
            var users = mapper.Map<IEnumerable<User>, List<ContactViewModel>>(search); 
            foreach(var user in users)
            {
                if (String.IsNullOrEmpty(user.Avatar))
                    user.Avatar = "\\Resources\\Images\\default.jpg";
                bool hasContact = contactService.HasContact(Id, user.Id);
                bool hasInBlock = blackListService.HasUserInBlock(Id, user.Id); 
                if (hasContact)
                {
                    user.UserInContact = true;
                }
                if(hasInBlock)
                {
                    user.Status = true;
                }
            }
           
            return Ok(users);

        }
    }
}
