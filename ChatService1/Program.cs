using ChatService1.Clients;
using ChatService1.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSignalR();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapHub<ChatHub>("/chatHub");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

Console.WriteLine("Enter your User ID:");
var userId = Console.ReadLine();

Console.WriteLine("Enter the URL of the chat service (e.g., http://localhost:5000):");
var serviceUrl = Console.ReadLine();

var chatClient = new ChatClient(serviceUrl, userId);

Console.WriteLine("Enter the User ID of the person you want to chat with:");
var targetUserId = Console.ReadLine();

while (true)
{
    Console.WriteLine("Enter your message (or type 'exit' to quit):");
    var message = Console.ReadLine();

    if (message?.ToLower() == "exit")
        break;

    await chatClient.SendMessage(targetUserId, message);
}