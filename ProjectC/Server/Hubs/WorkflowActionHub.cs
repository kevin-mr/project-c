using Microsoft.AspNetCore.SignalR;
using ProjectC.Shared.Models;

namespace ProjectC.Server.Hubs
{
    public class WorkflowActionHub : Hub
    {
        private static List<string> ConnectionIds = new List<string>();

        public override async Task OnConnectedAsync()
        {
            ConnectionIds.Add(Context.ConnectionId);
            await base.OnConnectedAsync();
        }

        public async Task NotifyWorkflowActionEventCaught(RequestEventDto webhookEventDto)
        {
            await Clients.All.SendAsync("WorkflowActionEventCaught", webhookEventDto);
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            ConnectionIds.Remove(Context.ConnectionId);
            await base.OnDisconnectedAsync(exception);
        }
    }
}
