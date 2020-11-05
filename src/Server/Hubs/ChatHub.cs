using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace Datalk.Server.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(string chatroomId, string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", chatroomId, user, message);
        }
    }
}