using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace Switchboard.Hubs
{
    public class ChannelHub : Hub
    {
        // Add client to selected channel
        public void JoinChannel(string channelID)
        {
            this.Groups.Add(this.Context.ConnectionId, channelID);
        }
    }
}