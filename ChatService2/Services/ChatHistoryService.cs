using ChatService1;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;

namespace ChatService2.Services;

public class ChatHistoryService: ChatHistory.ChatHistoryBase
{
    private readonly ChtDbContext _context;

    public ChatHistoryService(ChtDbContext context)
    {
        _context = context;
    }

    public override async Task<ChatHistoryResponse> GetChatHistory(ChatHistoryRequest request, ServerCallContext context)
    {
        var messages = await _context.ChatMessages
            .Where(m => m.ChatId == request.ChatId)
            .OrderBy(m => m.Timestamp)
            .Select(m => new ChatMessage
            {
                User = m.User,
                Message = m.Message,
                Timestamp = m.Timestamp.ToString("o")
            })
            .ToListAsync();

        var response = new ChatHistoryResponse();
        response.Messages.AddRange(messages);
        return response;
    }
}