using Microsoft.AspNetCore.SignalR;

namespace ChatService1.Hubs;

public class ChatHub : Hub
{
    // Method to send a message to a specific user
    public async Task SendMessage(string userId, string message)
    {
        await Clients.User(userId).SendAsync("ReceiveMessage", message);
    }
}
