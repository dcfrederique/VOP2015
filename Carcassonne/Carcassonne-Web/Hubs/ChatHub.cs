using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace Carcassonne_Web.Hubs
{
    public class ChatHub : Hub
    {
        public void Connect(int id)
        {
            Groups.Add(Context.ConnectionId, id.ToString());
        }

        public void Send(int id, string name, string message)
        {
            Clients.Group(id.ToString()).broadcastMessage(name, message);
        }

        public void Typing(int id, string name)
        {
            Clients.OthersInGroup(id.ToString()).broadcastTyping(name);
        }

        public void Disconnect(int id)
        {
            Groups.Remove(Context.ConnectionId, id.ToString());
        }

    }
}