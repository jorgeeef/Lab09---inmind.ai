using Microsoft.AspNetCore.SignalR;

namespace ChatService1.Hubs;

public class ChatHub : Hub
{
    private readonly ChtDbContext _context;

    public ChatHub(ChtDbContext context)
    {
        _context = context;
    }

    public async Task SendMessage(string chatId, string user, string message)
    {
        await Clients.Group(chatId).SendAsync("ReceiveMessage", user, message);

        // Save message to the database
        var chatMessage = new ChatMessage
        {
            ChatId = chatId,
            User = user,
            Message = message,
            Timestamp = DateTime.UtcNow
        };
        _context.ChatMessages.Add(chatMessage);
        await _context.SaveChangesAsync();
    }
}
