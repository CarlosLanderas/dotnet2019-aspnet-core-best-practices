using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DotNet2019.Api.Infrastructure.Hubs
{
    public class DiagnosticsHub: Hub
    {
        public override async Task OnConnectedAsync()
        {
            await Clients.All.SendAsync("Send", $"{Context.ConnectionId} joined");
        }

        public override async Task OnDisconnectedAsync(Exception ex)
        {
            await Clients.Others.SendAsync("Send", $"{Context.ConnectionId} left");
        }

        public Task SendRequestTrace(string message)
        {
            return Clients.All.SendAsync("SendRequestTrace", message);
        }

    }
}
