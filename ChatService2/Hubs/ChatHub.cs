﻿using ChatService1.Services;
using Microsoft.AspNetCore.SignalR;

namespace ChatService1.Hubs;

public class ChatHub : Hub
{

    public async Task SendMessage(string chatId, string user, string message)
    {
        await Clients.Group(chatId).SendAsync("ReceiveMessage", user, message);
        ChatHistoryService.AddMessage(chatId, user, message);
    }
    
    public async Task JoinChat(string chatId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, chatId);
    }

    public async Task LeaveChat(string chatId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, chatId);
    }
}
