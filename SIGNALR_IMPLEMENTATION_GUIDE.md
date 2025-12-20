# SignalR Real-Time Communication Implementation Guide

## Overview

This guide describes the implementation of **SignalR** for real-time notifications and chatting in the Gymunity application using a clean architecture pattern (without CQRS).

## Architecture

### Project Structure

```
Domain Layer (ITI.Gymunity.FP.Domain)
├── Models
│   ├── Messaging
│   │   ├── Message.cs
│   │   └── MessageThread.cs
│   ├── Notification.cs
│   └── Enums
│       ├── NotificationType.cs
│       └── MessageType.cs (existing)
└── Repositories
    └── IRepository<T>

Application Layer (ITI.Gymunity.FP.Application)
├── Contracts/Services
│   ├── IChatService.cs
│   ├── INotificationService.cs
│   └── ISignalRConnectionManager.cs
├── DTOs
│   ├── Messaging
│   │   ├── SendMessageRequest.cs
│   │   └── MessageResponse.cs
│   └── Notifications
│       └── NotificationResponse.cs
└── Mapping
    └── MappingProfile.cs

Infrastructure Layer (ITI.Gymunity.FP.Infrastructure)
└── Services
    ├── Chat
    │   └── ChatService.cs
    ├── Notifications
    │   └── NotificationService.cs
    └── SignalR
        └── SignalRConnectionManager.cs

API Layer (ITI.Gymunity.FP.APIs)
├── Hubs
│   ├── ChatHub.cs
│   └── NotificationHub.cs
├── Areas/Client
│   ├── ChatController.cs
│   └── NotificationsController.cs
└── Program.cs
```

## Components

### 1. **Domain Models**

#### Message.cs
- Represents a single message in a conversation
- Contains: content, sender, thread reference, media URL, read status

#### MessageThread.cs
- Represents a conversation between a client and trainer
- Manages participants and message history

#### Notification.cs
- Represents system notifications
- Tracks type, read status, and related entity

#### NotificationType.cs
Enum values:
- NewMessage (1)
- MessageReply (2)
- SubscriptionConfirmed (3)
- SubscriptionExpired (4)
- PackageUpdate (5)
- ProgramUpdate (6)
- WorkoutReminder (7)
- PaymentSuccessful (8)
- PaymentFailed (9)
- TrainerApproved (10)
- TrainerRejected (11)
- SystemNotification (12)

### 2. **Application Layer**

#### IChatService
```csharp
Task<MessageResponse> SendMessageAsync(int threadId, string senderId, SendMessageRequest request);
Task<IEnumerable<MessageResponse>> GetMessageThreadAsync(int threadId);
Task<IEnumerable<object>> GetUserChatsAsync(string userId);
Task<bool> MarkMessageAsReadAsync(long messageId);
Task<bool> MarkThreadAsReadAsync(int threadId, string userId);
```

#### INotificationService
```csharp
Task<NotificationResponse> CreateNotificationAsync(string userId, string title, string message, int type, string? relatedEntityId = null);
Task<IEnumerable<NotificationResponse>> GetUserNotificationsAsync(string userId, bool unreadOnly = false);
Task<bool> MarkNotificationAsReadAsync(int notificationId);
Task<bool> MarkAllNotificationsAsReadAsync(string userId);
Task<int> GetUnreadNotificationCountAsync(string userId);
```

#### ISignalRConnectionManager
```csharp
void AddConnection(string userId, string connectionId);
void RemoveConnection(string connectionId);
string? GetConnectionId(string userId);
IEnumerable<string> GetAllConnections(string userId);
void AddUserToGroup(string userId, string groupName);
void RemoveUserFromGroup(string userId, string groupName);
```

### 3. **Infrastructure Services**

#### ChatService
Implements `IChatService`
- Manages message persistence and retrieval
- Handles message thread operations
- Tracks read status

#### NotificationService
Implements `INotificationService`
- Creates and manages notifications
- Tracks unread notifications
- Marks notifications as read

#### SignalRConnectionManager
Implements `ISignalRConnectionManager`
- Thread-safe connection tracking
- Supports multiple connections per user
- Manages user groups

### 4. **SignalR Hubs**

#### ChatHub
Provides real-time messaging functionality:

**Methods (Server → Client):**
- `SendMessage(int threadId, SendMessageRequest request)` - Send a message
- `JoinThread(int threadId)` - Join a conversation thread
- `LeaveThread(int threadId)` - Leave a conversation thread
- `MarkMessageAsRead(long messageId, int threadId)` - Mark single message as read
- `MarkThreadAsRead(int threadId)` - Mark all messages in thread as read
- `UserTyping(int threadId)` - Notify others that user is typing
- `UserStoppedTyping(int threadId)` - Notify that user stopped typing

**Client Events:**
- `MessageReceived(MessageResponse)` - New message received
- `UserOnline(string userId)` - User came online
- `UserOffline(string userId)` - User went offline
- `UserJoinedThread(string userId)` - User joined thread
- `UserLeftThread(string userId)` - User left thread
- `MessageMarkedAsRead(long messageId)` - Message marked as read
- `ThreadMarkedAsRead(int threadId, string userId)` - Thread marked as read
- `UserTyping(string userId)` - User is typing
- `UserStoppedTyping(string userId)` - User stopped typing
- `Error(string message)` - Error occurred

#### NotificationHub
Provides real-time notification functionality:

**Methods (Server → Client):**
- `GetUnreadNotifications()` - Get unread notifications
- `GetUnreadNotificationCount()` - Get unread count
- `MarkNotificationAsRead(int notificationId)` - Mark as read
- `MarkAllNotificationsAsRead()` - Mark all as read

**Client Events:**
- `UnreadNotifications(IEnumerable<NotificationResponse>)` - Receive unread notifications
- `UnreadNotificationCount(int)` - Receive unread count
- `NotificationMarkedAsRead(int notificationId)` - Notification marked as read
- `AllNotificationsMarkedAsRead()` - All notifications marked as read
- `NewNotification(NotificationResponse)` - New notification arrived
- `Error(string message)` - Error occurred

### 5. **REST API Controllers**

#### ChatController (`/api/chat`)
- `GET /threads` - Get all chat threads
- `GET /threads/{threadId}/messages` - Get messages in thread
- `POST /threads/{threadId}/messages` - Send message
- `PUT /messages/{messageId}/read` - Mark message as read
- `PUT /threads/{threadId}/read` - Mark thread as read

#### NotificationsController (`/api/notifications`)
- `GET` - Get all notifications
- `GET /unread-count` - Get unread count
- `PUT /{notificationId}/read` - Mark as read
- `PUT /read-all` - Mark all as read

## Usage Guide

### Client Implementation (Angular/TypeScript Example)

#### Installation
```bash
npm install @microsoft/signalr
```

#### Chat Connection
```typescript
import * as signalR from "@microsoft/signalr";

// Create connection
const connection = new signalR.HubConnectionBuilder()
  .withUrl("https://localhost:5131/hubs/chat", {
    accessTokenFactory: () => this.getToken(),
    skipNegotiation: true,
    transport: signalR.HttpTransportType.WebSockets
  })
  .withAutomaticReconnect()
  .withHubProtocol(new signalR.JsonHubProtocol())
  .build();

// Start connection
await connection.start();

// Join thread
await connection.invoke("JoinThread", threadId);

// Send message
await connection.invoke("SendMessage", threadId, {
  content: "Hello",
  type: 1, // MessageType.Text
  mediaUrl: null
});

// Listen for incoming messages
connection.on("MessageReceived", (message) => {
  console.log("New message:", message);
});

// Typing indicator
await connection.invoke("UserTyping", threadId);
// ... after 3 seconds
await connection.invoke("UserStoppedTyping", threadId);

// Mark as read
await connection.invoke("MarkMessageAsRead", messageId, threadId);

// Leave thread
await connection.invoke("LeaveThread", threadId);
```

#### Notification Connection
```typescript
// Create connection
const notificationConnection = new signalR.HubConnectionBuilder()
  .withUrl("https://localhost:5131/hubs/notifications", {
    accessTokenFactory: () => this.getToken(),
    skipNegotiation: true,
    transport: signalR.HttpTransportType.WebSockets
  })
  .withAutomaticReconnect()
  .build();

// Start connection
await notificationConnection.start();

// Get unread notifications
await notificationConnection.invoke("GetUnreadNotifications");

// Listen for notifications
notificationConnection.on("UnreadNotifications", (notifications) => {
  console.log("Unread notifications:", notifications);
});

notificationConnection.on("NewNotification", (notification) => {
  console.log("New notification:", notification);
});

// Mark as read
await notificationConnection.invoke("MarkNotificationAsRead", notificationId);
```

### Server Implementation (C# Example)

#### Sending Notifications to Clients
```csharp
public class SomeService
{
    private readonly INotificationService _notificationService;
    private readonly IHubContext<NotificationHub> _hubContext;

    public async Task SendNotificationToUserAsync(string userId, string title, string message, int type)
    {
        // Create notification in database
        var notification = await _notificationService.CreateNotificationAsync(
            userId, 
            title, 
            message, 
            type
        );

        // Send to connected client via SignalR
        await _hubContext.Clients
            .User(userId)
            .SendAsync("NewNotification", notification);
    }
}
```

#### Broadcasting Messages in Thread
```csharp
// Inside ChatHub or other service with IHubContext<ChatHub>
await _hubContext.Clients
    .Group($"thread-{threadId}")
    .SendAsync("MessageReceived", messageResponse);
```

## Configuration

### Program.cs Setup
```csharp
// Add SignalR
builder.Services.AddSignalR();

// Configure CORS for WebSocket
builder.Services.AddCors(options =>
{
    options.AddPolicy("wepPolicy", policyBuilder =>
    {
        policyBuilder
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials()
            .SetIsOriginAllowed(origin => true);
    });
});

// Map hubs
app.MapHub<ChatHub>("/hubs/chat");
app.MapHub<NotificationHub>("/hubs/notifications");
```

### Dependency Injection (Infrastructure)
```csharp
public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
{
    // SignalR Services
    services.AddSingleton<ISignalRConnectionManager, SignalRConnectionManager>();
    services.AddScoped<IChatService, ChatService>();
    services.AddScoped<INotificationService, NotificationService>();
    
    return services;
}
```

## Database Considerations

### Required Migrations
Ensure your DbContext includes:

```csharp
public DbSet<Message> Messages { get; set; }
public DbSet<MessageThread> MessageThreads { get; set; }
public DbSet<Notification> Notifications { get; set; }
```

### Create Migration
```bash
dotnet ef migrations add AddSignalRTables -p ITI.Gymunity.FP.Infrastructure
dotnet ef database update
```

## Best Practices

### 1. **Connection Management**
- Always use `AddAutomaticReconnect()` on client
- Implement exponential backoff for reconnections
- Clean up connections on disconnect

### 2. **Security**
- Always use `[Authorize]` attribute on hubs
- Validate user permissions before operations
- Use JWT tokens for authentication

### 3. **Scalability**
- For production: Use Redis backplane
  ```csharp
  builder.Services.AddSignalR()
      .AddStackExchangeRedis("connection-string");
  ```

### 4. **Performance**
- Use groups for broadcast operations
- Implement pagination for message history
- Cache unread counts

### 5. **Error Handling**
- Implement try-catch in hub methods
- Send error messages back to client
- Log all exceptions

## Common Patterns

### Pattern 1: Direct User Notification
```csharp
await _hubContext.Clients
    .User(userId)
    .SendAsync("NewNotification", notification);
```

### Pattern 2: Group Broadcasting
```csharp
await _hubContext.Clients
    .Group($"thread-{threadId}")
    .SendAsync("MessageReceived", message);
```

### Pattern 3: Exclude Sender
```csharp
await _hubContext.Clients
    .GroupExcept($"thread-{threadId}", Context.ConnectionId)
    .SendAsync("MessageReceived", message);
```

## Troubleshooting

### Issue: "WebSocket Connection Failed"
- Check CORS policy allows credentials
- Verify SSL certificate (for production)
- Check firewall rules

### Issue: "Message Not Received"
- Verify user is in group
- Check connection status
- Validate hub method names match client invocation

### Issue: "Connection Drops"
- Implement automatic reconnection
- Check server logs for exceptions
- Verify network stability

## Next Steps

1. **Add Database Indexes**
   ```sql
   CREATE INDEX idx_message_threadid ON Messages(ThreadId);
   CREATE INDEX idx_notification_userid ON Notifications(UserId);
   ```

2. **Implement Offline Message Queue** - Save messages when recipients are offline

3. **Add Message Search** - Full-text search on message content

4. **Implement Presence** - Track user online/offline status

5. **Add File Upload Support** - For media messages

6. **Implement Read Receipts** - Show when messages are read

7. **Add Typing Indicators UI** - Visual feedback for typing

8. **Message Encryption** - End-to-end encryption for sensitive data

## Testing

### Unit Tests
```csharp
[Test]
public async Task SendMessage_ValidRequest_ReturnsMessageResponse()
{
    // Arrange
    var request = new SendMessageRequest { Content = "Test" };
    
    // Act
    var result = await _chatService.SendMessageAsync(1, "userId", request);
    
    // Assert
    Assert.NotNull(result);
    Assert.AreEqual("Test", result.Content);
}
```

### Integration Tests
- Use `TestServer` for hub testing
- Mock `IHubContext<ChatHub>`
- Test connection/disconnection scenarios

## References

- [Microsoft SignalR Documentation](https://docs.microsoft.com/en-us/aspnet/core/signalr/)
- [SignalR Hubs API](https://docs.microsoft.com/en-us/aspnet/core/signalr/hubs)
- [ASP.NET Core Security](https://docs.microsoft.com/en-us/aspnet/core/security/)
