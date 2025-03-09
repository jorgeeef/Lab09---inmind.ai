using Grpc.Core;

namespace ChatService1.Services;

public class ChatHistoryService: ChatHistory.ChatHistoryBase
{
    private static readonly Dictionary<string, List<ChatMessage>> ChatMessages = new();

    public override Task<ChatHistoryResponse> GetChatHistory(ChatHistoryRequest request, ServerCallContext context)
    {
        var response = new ChatHistoryResponse();

        if (ChatMessages.TryGetValue(request.ChatId, out var messages))
        {
            response.Messages.AddRange(messages);
        }

        return Task.FromResult(response);
    }

    public static void AddMessage(string chatId, string user, string message)
    {
        if (!ChatMessages.ContainsKey(chatId))
        {
            ChatMessages[chatId] = new List<ChatMessage>();
        }

        ChatMessages[chatId].Add(new ChatMessage
        {
            User = user,
            Message = message,
            Timestamp = DateTime.UtcNow.ToString("o")
        });
    }
}