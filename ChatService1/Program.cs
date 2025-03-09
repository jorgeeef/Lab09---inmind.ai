using ChatService1;
using ChatService1.Clients;
using ChatService1.Hubs;
using ChatService2.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddGrpc();
builder.Services.AddSignalR();
builder.Services.AddDbContext<ChtDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseEndpoints(endpoints =>
{
    endpoints.MapGrpcService<ChatHistoryService>();
    endpoints.MapHub<ChatHub>("/chatHub");
    endpoints.MapControllers();
});

app.MapHub<ChatHub>("/chatHub");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

