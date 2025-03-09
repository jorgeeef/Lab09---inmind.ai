﻿namespace ChatService1;

public class ChatMessage
{
    public int Id { get; set; }
    public string ChatId { get; set; }
    public string User { get; set; }
    public string Message { get; set; }
    public DateTime Timestamp { get; set; }
}