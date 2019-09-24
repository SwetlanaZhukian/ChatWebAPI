using Chat.BLL.DTO;
using Chat.Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Chat.BLL.Interfaces
{
  public  interface IChatService
    {
        Task<IEnumerable<Dialog>> GetAllDialogsAsync(string userId);
        Task<List<MessageViewModel>> GetAllMessagesForDialogAsync(string user1Id, string user2Id);
        //Task AddMessageAsync(string user1Id, string user2Id, string message, DateTime date);
        Task<bool> IsExistDialog(string user1Id, string user2Id);
        Task<DialogViewModel> GetDialogAsync(string user1Id, string user2Id);
        Task CreateNewDialogAsync(string user1Id, string user2Id);
        Task<IEnumerable<Dialog>> GetAllDialogList(string userId);
        Task<IEnumerable<DialogViewModel>> GetAllUserDialogs(string userId);
        //Task<List<DialogViewModel>> GetAllDialogs(string userId);
        //Task<List<MessageViewModel>> GetAllDialogMessages(string userId, string friendId);
        Task<MessageViewModel> AddNewMessage(string senderId, PostMessageViewModel postMessage, DateTime time);
        //Task<bool> IsDialogExists(string senderId, string recevierId);
        //Task<DialogViewModel> GetDialog(string userId, string friendId);
        //Task CreateDialog(string userId, string friendId);
        bool IsOnline(string id);

    }
}
