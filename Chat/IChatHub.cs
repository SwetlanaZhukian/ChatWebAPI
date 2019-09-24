using Chat.BLL.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chat
{
  public  interface IChatHub
    {
        Task OnConnectedAsync();
        void UpdateList(string callerId);
        void FindCallerReceiverByIds(string receiverId, string id, out UserIds caller, out UserIds receiver);
        void Disconnect(string id);
        Task OnDisconnectedAsync(Exception exception);
    }
}
