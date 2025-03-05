using Microsoft.AspNetCore.SignalR.Client;

namespace ChatService1.Clients;

public class ChatClient
{
    private HubConnection _connection;

    public ChatClient(string serviceUrl, string userId)
    {
        _connection = new HubConnectionBuilder()
            .WithUrl($"{serviceUrl}/chatHub")
            .Build();

        _connection.On<string>("ReceiveMessage", (message) =>
        {
            Console.WriteLine($"Received: {message}");
        });

        _connection.StartAsync().Wait();
        Console.WriteLine("Connected to the chat hub.");
    }

    public async Task SendMessage(string userId, string message)
    {
        await _connection.InvokeAsync("SendMessage", userId, message);
    }
}