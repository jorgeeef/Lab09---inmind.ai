using System;
using System.Threading.Tasks;
using Grpc.Net.Client;
using Microsoft.AspNetCore.SignalR.Client;


namespace ChatCli
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Enter your name:");
            var user = Console.ReadLine();

            Console.WriteLine("Enter chat ID:");
            var chatId = Console.ReadLine();

            // Create SignalR connection
            var chatHubClient = new HubConnectionBuilder()
                .WithUrl("https://localhost:5001/chatHub")
                .Build();

            // Handle incoming messages
            chatHubClient.On<string, string>("ReceiveMessage", (sender, message) =>
            {
                Console.WriteLine($"{sender}: {message}");
            });

            try
            {
                await chatHubClient.StartAsync();
                Console.WriteLine("Connected to SignalR hub.");
                
                await chatHubClient.InvokeAsync("JoinChat", chatId);
                Console.WriteLine($"Joined chat group: {chatId}");
                
                using var channel = GrpcChannel.ForAddress("https://localhost:5001");
                var client = new ChatHistory.ChatHistoryClient(channel);
                
                var history = await client.GetChatHistoryAsync(new ChatHistoryRequest { ChatId = chatId });
                Console.WriteLine("Chat History:");
                foreach (var message in history.Messages)
                {
                    Console.WriteLine($"{message.User}: {message.Message} ({message.Timestamp})");
                }

                // Main chat loop
                while (true)
                {
                    Console.WriteLine("Enter a message (or 'exit' to quit):");
                    var message = Console.ReadLine();

                    if (message?.ToLower() == "exit")
                        break;

                    // Send message via SignalR
                    await chatHubClient.InvokeAsync("SendMessage", chatId, user, message);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
            finally
            {
                await chatHubClient.DisposeAsync();
                Console.WriteLine("Disconnected from SignalR hub.");
            }
        }
    }
}