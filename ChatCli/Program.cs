using Grpc.Net.Client;
using Microsoft.AspNetCore.SignalR.Client;

class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("Enter your name:");
        var user = Console.ReadLine();

        Console.WriteLine("Enter chat ID:");
        var chatId = Console.ReadLine();

        var chatHubClient = new HubConnectionBuilder()
            .WithUrl("https://localhost:5001/chatHub")
            .Build();

        chatHubClient.On<string, string>("ReceiveMessage", (sender, message) =>
        {
            Console.WriteLine($"{sender}: {message}");
        });

        await chatHubClient.StartAsync();
        await chatHubClient.InvokeAsync("JoinChat", chatId);

        var channel = GrpcChannel.ForAddress("https://localhost:5001");
        var client = new ChatHistory.ChatHistoryClient(channel);

        var history = await client.GetChatHistoryAsync(new ChatHistoryRequest { ChatId = chatId });
        Console.WriteLine("Chat History:");
        foreach (var message in history.Messages)
        {
            Console.WriteLine($"{message.User}: {message.Message} ({message.Timestamp})");
        }

        while (true)
        {
            Console.WriteLine("Enter a message (or 'exit' to quit):");
            var message = Console.ReadLine();

            if (message?.ToLower() == "exit")
                break;

            await chatHubClient.InvokeAsync("SendMessage", chatId, user, message);
        }
    }
}