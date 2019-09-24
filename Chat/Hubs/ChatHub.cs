using AutoMapper;
using Chat.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Chat.BLL.Interfaces;
using Newtonsoft.Json;
using Chat.BLL.Infrastructure;

namespace Chat.Hubs
{
   
    [Authorize]
    public class ChatHub:Hub, IChatHub
    {
        //private UserManager<User> userManager;
        //private IChatService chatService;
        //private IBlackListService blackListService;
        //readonly IMapper mapper;
        //public ChatHub(UserManager<User> _userManager,IChatService _chatServise,IMapper _mapper,IBlackListService _blackListService)
        //{
        //    userManager = _userManager;
        //    chatService = _chatServise;
        //    mapper = _mapper;
        //    blackListService = _blackListService;
        //}
        //static List<UserIds> usersList = new List<UserIds>();

        //public async override Task OnConnectedAsync()
        //{
        //    string Id = Context.User.Claims.First(c => c.Type == "Id").Value;
        //    UpdateList(Id);
        //    await base.OnConnectedAsync();
        //}

        ////public async Task Send(string message, string receiverId)
        ////{
        ////    UserIds receiver, caller;
        ////    FindCallerReceiverByIds(receiverId, out caller, out receiver);
        ////    await chatService.AddMessageAsync(caller.userId, receiverId, message, DateTime.Now);
        ////    await Clients.Client(caller.connectId).SendAsync("SendMyself", message);
        ////    if (receiver != null)
        ////    {
        ////        await Clients.Client(receiver.connectId).SendAsync("Send", message, caller.userId);

        ////    }

        ////}
        //public async Task SendFaraway(string message, string receiverId)
        //{

        //    UserIds receiver, caller;
        //    FindCallerReceiverByIds(receiverId, out caller, out receiver);
        //    bool dialogExist = await chatService.IsExistDialog(caller.userId, receiverId);
        //    if (dialogExist)
        //    {
        //        if (!String.IsNullOrEmpty(message))
        //        {
        //            await chatService.AddMessageAsync(caller.userId, receiverId, message, DateTime.Now);
        //        if (receiver != null)
        //        {
        //            // await Clients.Client(caller.connectId).SendAsync("SendMyself", message);
        //            await Clients.Client(receiver.connectId).SendAsync("Send", message, caller.userId);
        //        }

        //        await Clients.Client(caller.connectId).SendAsync("SendMyself", message);
        //    }
        //    }
        //    else
        //    {
        //        if (!String.IsNullOrEmpty(message))
        //        {
        //            await chatService.CreateNewDialogAsync(caller.userId, receiverId);
        //            await chatService.AddMessageAsync(caller.userId, receiverId, message, DateTime.Now);
        //            var dialog = await chatService.GetDialogAsync(caller.userId, receiverId);

        //            if (receiver != null)
        //            {
        //                await Clients.Client(caller.connectId).SendAsync("SendMyself", message);
        //                await Clients.Client(receiver.connectId).SendAsync("Send", message, caller.userId);
        //            }
        //        }
        //    }

        //}
        //public override Task OnDisconnectedAsync(Exception exception)
        //{
        //   usersList.Remove(usersList.Find(u => u.connectId == Context.ConnectionId));
        //    return base.OnDisconnectedAsync(exception);
        //}
        //void UpdateList(string callerId)
        //{
        //    var index =usersList.FindIndex(i => i.userId == callerId);
        //    if (index != -1 && usersList[index].connectId != Context.ConnectionId)
        //    {
        //      usersList[index].connectId = Context.ConnectionId;
        //    }
        //    else
        //    {
        //       usersList.Add(new UserIds { connectId = Context.ConnectionId, userId = callerId });
        //    }
        //}
        //void FindCallerReceiverByIds(string receiverId, out UserIds caller, out UserIds receiver)
        //{
        //    receiver = usersList.Find(i => i.userId == receiverId);
        //    caller = usersList.Find(i => i.connectId == Context.ConnectionId);
        //}



        ////void IChatHub.FindCallerReceiverByIds(string receiverId, out UserIds caller, out UserIds receiver)
        ////{
        ////    receiver = UserIds.usersList.Find(i => i.userId == receiverId);
        ////    caller = UserIds.usersList.Find(i => i.connectId == Context.ConnectionId);
        ////}
        private IChatService chatService;
        public ChatHub(IChatService _chatService)
        {
            chatService = _chatService;
        }

        public async override Task OnConnectedAsync()
        {
            var id = Context.User.Claims.First(c => c.Type == "Id").Value;
            UpdateList(id);
            await base.OnConnectedAsync();
        }

        void UpdateList(string callerId)
        {
            var index = UserIds.usersList.FindIndex(i => i.userId == callerId);
            if (index != -1 && UserIds.usersList[index].connectionId != Context.ConnectionId)
            {
                UserIds.usersList[index].connectionId = Context.ConnectionId;
            }
            else
            {
                UserIds.usersList.Add(new UserIds { connectionId = Context.ConnectionId, userId = callerId });
            }
        }
        void FindCallerReceiverByIds(string receiverId, string id, out UserIds caller, out UserIds receiver)
        {
            receiver = UserIds.usersList.Find(i => i.userId == receiverId);
            caller = UserIds.usersList.Find(i => i.userId == id);
        }

        public async override Task OnDisconnectedAsync(Exception exception)
        {
            UserIds.usersList.Remove(UserIds.usersList.Find(c => c.connectionId == Context.ConnectionId));
            await base.OnDisconnectedAsync(exception);
        }
        public void Disconnect(string id)
        {
            if (UserIds.usersList.Any(c => c.userId == id))
            {
                UserIds.usersList.Remove(UserIds.usersList.Find(c => c.userId == id));
            }

        }
        void IChatHub.UpdateList(string callerId)
        {
            throw new NotImplementedException();
        }

        void IChatHub.FindCallerReceiverByIds(string receiverId, string id, out UserIds caller, out UserIds receiver)
        {
            receiver = UserIds.usersList.Find(i => i.userId == receiverId);
            caller = UserIds.usersList.Find(i => i.userId == id);
        }


    }
}
