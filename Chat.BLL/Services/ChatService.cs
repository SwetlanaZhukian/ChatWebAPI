using Chat.BLL.Interfaces;
using Chat.DAL.EF;
using Chat.DAL.Interfaces;
using Chat.Models.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Chat.BLL.DTO;
using System.Globalization;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Chat.ViewModels;
using Chat.BLL.Infrastructure;

namespace Chat.BLL.Services
{
    public class ChatService : IChatService
    {
        IUnitOfWork db { get; set; }
        private UserManager<User> userManager;
        private ChatContext context;
        private readonly IBlackListService blackListService;
        private readonly IFileManager fileManager;

        public ChatService(IUnitOfWork _db, ChatContext _context, UserManager<User> _userManager,IBlackListService _blackListService, IHostingEnvironment appEnvironment, IFileManager _fileManager)
        {
            db = _db;
            context = _context;
            userManager = _userManager;
            blackListService = _blackListService;
            fileManager = _fileManager;
            
        }

        public async Task<IEnumerable<Dialog>> GetAllDialogsAsync(string userId)
        {
            return await db.Dialogs.FindByCondition(p => p.UserId == userId);
        }

        public async Task<List<MessageViewModel>> GetAllMessagesForDialogAsync(string user1Id, string user2Id)
        {
            try
            {
                var user = await userManager.FindByIdAsync(user1Id);
                var dialoglist = user.UserDialogs.Union(user.ContactDialogs);
                var dialog = dialoglist.SingleOrDefault(p => (p.UserId == user1Id && p.ContactId == user2Id) || (p.UserId == user2Id && p.ContactId == user1Id));
                var messages = dialog.Messages;
                var allMessages = new List<MessageViewModel>();
                foreach (var message in messages)
                {
                    var model = new MessageViewModel(message);
                    allMessages.Add(model);
                }
                return allMessages;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<bool> IsExistDialog(string user1Id, string user2Id)
        {
            var dialogs = await GetAllDialogList(user1Id);
            bool result = dialogs.Any(p => (p.UserId == user1Id && p.ContactId == user2Id) || (p.UserId == user2Id && p.ContactId == user1Id));
            return result;

        }

        public async Task<DialogViewModel> GetDialogAsync(string user1Id, string user2Id)
        {
            try
            {
                var user = await userManager.FindByIdAsync(user1Id);
                var dialoglist = user.UserDialogs.Union(user.ContactDialogs);
                var dialog = dialoglist.SingleOrDefault(p => (p.UserId == user1Id && p.ContactId == user2Id) || (p.UserId == user2Id && p.ContactId == user1Id));
               
                    if (dialog != null)
                {
                    var dialogs = new DialogViewModel(dialog, user);
                    dialogs.Messages = await GetAllMessagesForDialogAsync(user1Id, user2Id);
                    bool hasInBlock = blackListService.HasUserInBlock(user1Id, user2Id);
                    bool hasInBlockContact = blackListService.HasUserInBlock(user2Id, user1Id);
                    if (hasInBlock || hasInBlockContact)
                    {
                        dialogs.InBlock = true;
                    }
                    return dialogs;
                }
                else
                { 
                var user1 = await userManager.FindByIdAsync(user1Id);
                var user2 = await userManager.FindByIdAsync(user2Id);
                var dialog1 = new Dialog
                {
                    UserId = user1Id,
                    ContactId = user2Id
                };
                await db.Dialogs.Create(dialog);
                    var dialogs = new DialogViewModel(dialog, user);
                    bool hasInBlock = blackListService.HasUserInBlock(user1Id, user2Id);
                    bool hasInBlockContact = blackListService.HasUserInBlock(user2Id, user1Id);
                    if (hasInBlock || hasInBlockContact)
                    {
                        dialogs.InBlock = true;
                    }
                    return dialogs;
                    }
            }
            catch (Exception ex)
            {
                throw ex;
            }


             }

        public async Task CreateNewDialogAsync(string user1Id, string user2Id)
        {
            var user1 = await userManager.FindByIdAsync(user1Id);
            var user2 = await userManager.FindByIdAsync(user2Id);
            var dialog = new Dialog
            {
                UserId = user1Id,
                ContactId = user2Id
            };
            await db.Dialogs.Create(dialog);
            
                await db.Dialogs.SaveAsync();
           
        }

        public async Task<IEnumerable<Dialog>> GetAllDialogList(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            var dialoglist = user.UserDialogs.Union(user.ContactDialogs);
            return dialoglist;
        }

        public async Task<IEnumerable<DialogViewModel>> GetAllUserDialogs(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            var dialoglist = user.UserDialogs.Union(user.ContactDialogs);
            var allDialogs = new List<DialogViewModel>();
            foreach (var dialog in dialoglist)
            {
                if (dialog.Messages.Count != 0)
                {
                    var model = new DialogViewModel(dialog, user);
                    bool online =IsOnline(model.ContactId);
                    if (online)
                    {
                        model.IsOnline = true;
                    }
                    else
                    {
                        model.IsOnline = false;
                    }
                    allDialogs.Add(model);
                }
            }
            return allDialogs;
        }

        public async Task<MessageViewModel> AddNewMessage(string senderId, PostMessageViewModel postMessage, DateTime time)
        {
            try
            {
                User user = await userManager.FindByIdAsync(senderId);
                var dialog = new Dialog();
                if (user.UserDialogs.Any(c => c.ContactId == postMessage.ReceiverId || c.UserId == postMessage.ReceiverId))
                {
                    var usersDialog = user.UserDialogs.Single(c => c.ContactId == postMessage.ReceiverId || c.UserId == postMessage.ReceiverId);
                    dialog = usersDialog;
                }
                if (user.ContactDialogs.Any(c => c.ContactId == postMessage.ReceiverId || c.UserId== postMessage.ReceiverId))
                {
                    var interlocutorsDialog = user.ContactDialogs.Single(c => c.ContactId == postMessage.ReceiverId || c.UserId == postMessage.ReceiverId);
                    dialog = interlocutorsDialog;
                }

                
                var newMessage = new Message
                {
                    DialogId = dialog.DialogId,
                    UserId = senderId,
                    Content = postMessage.Text,
                    Date= time

                };
                await db.Messages.Create(newMessage);
                 await db.Messages.SaveAsync();
                if (postMessage.Attachment != null)
                {
                    var files = await fileManager.UploadMessagesFiles(dialog.DialogId, newMessage.MessageId, postMessage.Attachment);
                }
                var messageVM = new MessageViewModel(newMessage);
                return messageVM;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        

       
        public bool IsOnline(string id)
        {
            if (UserIds.usersList.Any(c => c.userId == id))
            {
                return true;
            }
            else return false;
        }
        public async Task<Dialog> FindDialog(string user1Id, string user2Id)
        {
            var user = await userManager.FindByIdAsync(user1Id);
            var dialoglist = user.UserDialogs.Union(user.ContactDialogs);
            var dialog = dialoglist.SingleOrDefault(p => (p.UserId == user1Id && p.ContactId == user2Id) || (p.UserId == user2Id && p.ContactId == user1Id));
            return dialog;
        }
        public async Task DeleteDialog(Dialog dialog)
        {
            await db.Dialogs.Delete(dialog);
        }
    }
}
