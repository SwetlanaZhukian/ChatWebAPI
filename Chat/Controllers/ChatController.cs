using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Chat.BLL.DTO;
using Chat.BLL.Infrastructure;
using Chat.BLL.Interfaces;
using Chat.Hubs;
using Chat.Models.Entities;
using Chat.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Chat.Controllers
{

    [Route("api/chat")]
    public class ChatController : Controller
    {

        private UserManager<User> userManager;
        private readonly IContactService contactService;
        private readonly IChatService chatService;
        private readonly IMapper mapper;
        private readonly IBlackListService blackListService;
        IHubContext<ChatHub> hubContext;
        IChatHub chatHub;

        public ChatController(UserManager<User> _userManager, IMapper _mapper, IContactService _contactService, IBlackListService _blackListService, IChatService _chatService, IHubContext<ChatHub> _hubContext, IChatHub _chatHub)
        {
            userManager = _userManager;
            mapper = _mapper;
            contactService = _contactService;
            blackListService = _blackListService;
            chatService = _chatService;
            hubContext = _hubContext;
            chatHub = _chatHub;

        }
        //[HttpGet]
        //[Authorize]
        //[Route("dialogs")]
        //public async Task<List<DialogViewModel>> GetAllDialogs()
        //{
        //    try
        //    {
        //        var id = User.Claims.First(c => c.Type == "Id").Value;
        //        return await chatService.GetAllUserDialogs(id);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        [HttpGet]
        [Authorize]
        [Route("{friendId}")]
        public async Task<ActionResult<DialogViewModel>> GetDialog(string friendId)
        {
            try
            {
                var id = User.Claims.First(c => c.Type == "Id").Value;
                if (!(await chatService.IsExistDialog(id, friendId)))
                {
                    await chatService.CreateNewDialogAsync(id, friendId);
                }
                if (await chatService.IsExistDialog(id, friendId))
                {
                    return await chatService.GetDialogAsync(id, friendId);
                }


                else return BadRequest("Add new message to start dialog");

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [Route("dialogs")]
        public async Task<object> GetAllDialogs()
        {
            string Id = User.Claims.First(c => c.Type == "Id").Value;
            var dialogs = await chatService.GetAllUserDialogs(Id);

            return Ok(dialogs);

        }

        [HttpGet("{id}")]
        public async Task<object> GetAllMessages(string id)
        {
            string Id = User.Claims.First(c => c.Type == "Id").Value;
            var messages = await chatService.GetDialogAsync(Id, id);
            return Ok(messages);

        }
        [HttpPost]
        [Authorize]
        [Route("sendMessage")]
        public async Task<IActionResult> SendMessage([FromForm]PostMessageViewModel postMessage)
        {
            try
            {
                var id = User.Claims.First(c => c.Type == "Id").Value;
                if ( blackListService.HasUserInBlock(id, postMessage.ReceiverId))
                {
                    return BadRequest("Unlock this user to send messages");
                }
                if ( blackListService.HasUserInBlock(postMessage.ReceiverId, id))
                {
                    return BadRequest("This user reseieved access to sendind messages");
                }
                if (postMessage.Attachment == null && postMessage.Text == null)
                {
                    return BadRequest("You cannot send the empty message!");
                }
                if (postMessage.Attachment != null)
                {
                    foreach (var file in postMessage.Attachment)
                    {
                        if (file.ContentType == "image/jpg" || file.ContentType == "image/jpeg" || file.ContentType == "audio/mp3" || file.ContentType == "video/mp4" || file.ContentType == "audio/mpeg")
                        {
                            if ((file.ContentType == "image/jpg" || file.ContentType == "image/jpeg") && file.Length >= 2097152)
                            {
                                return BadRequest("Unsupported file length. JPG/JPEG must be before 2MB ");
                            }
                            else if ((file.ContentType == "audio/mp3" || file.ContentType == "audio/mpeg") && file.Length >= 10484760)
                            {
                                return BadRequest("Unsupported file length. Audio files must be before 10MB ");
                            }
                            else if (file.ContentType == "video/mp4" && file.Length >= 31457280)
                            {
                                return BadRequest("Unsupported file length. Video files must be before 10MB ");
                            }
                        }
                        else
                        {
                            return BadRequest("Unsupported file type. Files must be only of 'jpg/jpeg', 'mp3', 'mp4' formats! ");
                        }
                    }
                }

                UserIds receiver, caller;
                chatHub.FindCallerReceiverByIds(postMessage.ReceiverId,id, out caller, out receiver);
                bool dialogExists = await chatService.IsExistDialog(caller.userId, postMessage.ReceiverId);
                if (dialogExists)
                {
                    var message = await chatService.AddNewMessage(caller.userId, postMessage, DateTime.Now);
                    if (chatService.IsOnline(postMessage.ReceiverId))
                    {
                        await hubContext.Clients.Client(receiver.connectionId).SendAsync("Send", message, caller.userId);
                        await hubContext.Clients.Clients(caller.connectionId).SendAsync("SendMyself", message);
                    }
                    else
                    {
                        await hubContext.Clients.Clients(caller.connectionId).SendAsync("SendMyself", message);
                    }
                    return Ok(message);
                }
                else
                {
                    await chatService.CreateNewDialogAsync(caller.userId, postMessage.ReceiverId);
                    var message = await chatService.AddNewMessage(caller.userId, postMessage, DateTime.Now);
                    if (receiver != null)
                    {
                        if (chatService.IsOnline(postMessage.ReceiverId))
                        {
                            await hubContext.Clients.Client(receiver.connectionId).SendAsync("Send", message, caller.userId);
                            await hubContext.Clients.Clients(caller.connectionId).SendAsync("SendMyself", message);
                        }
                        else
                        {
                            await hubContext.Clients.Clients(caller.connectionId).SendAsync("SendMyself", message);
                        }
                    }
                    return Ok(message);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                string Id = User.Claims.First(c => c.Type == "Id").Value;
                var dialog = await chatService.FindDialog(Id, id);
                if (dialog == null)
                {
                    return BadRequest();
                }
                await chatService.DeleteDialog(dialog);
                return NoContent();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }



    }
}
