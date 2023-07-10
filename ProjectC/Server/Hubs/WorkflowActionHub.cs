using Microsoft.AspNetCore.SignalR;
using ProjectC.Shared.Models;

namespace ProjectC.Server.Hubs
{
    public class WorkflowActionHub : Hub
    {
        public async Task NotifyWorkflowActionEventCaught(RequestEventDto webhookEventDto)
        {
            await Clients.All.SendAsync("WorkflowActionEventCaught", webhookEventDto);
        }
    }
}
