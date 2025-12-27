# Admin Portal Notifications & Chat - Quick Reference

## URL Routes

### Notifications
- **Main Page**: `/admin/notifications`
- **Mark as Read (Single)**: `POST /admin/notifications/mark-as-read/{id}`
- **Mark as Read (All)**: `POST /admin/notifications/mark-all-as-read`
- **Unread Count**: `GET /admin/notifications/unread-count`
- **Unread List**: `GET /admin/notifications/unread`

### Messages
- **Main Page**: `/admin/chats`
- **Get Thread**: `GET /admin/chats/thread/{threadId}`
- **Send Message**: `POST /admin/chats/send-message`
- **Mark Message Read**: `POST /admin/chats/mark-as-read/{messageId}`
- **Mark Thread Read**: `POST /admin/chats/mark-thread-as-read/{threadId}`
- **Unread Count**: `GET /admin/chats/unread-count`

## SignalR Hubs

### Admin Notification Hub (`/hubs/admin-notifications`)

**Server Methods (Client can invoke):**
```javascript
await connection.invoke("GetAdminNotifications");
await connection.invoke("GetUnreadCount");
await connection.invoke("BroadcastAdminNotification", title, message, type);
await connection.invoke("SendUrgentAlert", targetAdminId, title, message);
await connection.invoke("MarkAsRead", notificationId);
```

**Client Events (Server sends):**
```javascript
connection.on("AdminNotifications", (notifications) => {});
connection.on("UnreadCount", (count) => {});
connection.on("NotificationBroadcast", (notification) => {});
connection.on("UrgentAlert", (alert) => {});
connection.on("NotificationRead", (notificationId) => {});
connection.on("AdminOnline", (userId) => {});
connection.on("AdminOffline", (userId) => {});
```

### Chat Hub (`/hubs/chat`)

**Server Methods:**
```javascript
await connection.invoke("SendMessage", threadId, request);
await connection.invoke("JoinThread", threadId);
await connection.invoke("LeaveThread", threadId);
await connection.invoke("MarkMessageAsRead", messageId, threadId);
await connection.invoke("MarkThreadAsRead", threadId);
await connection.invoke("UserTyping", threadId);
await connection.invoke("UserStoppedTyping", threadId);
```

**Client Events:**
```javascript
connection.on("MessageReceived", (message) => {});
connection.on("MessageMarkedAsRead", (messageId) => {});
connection.on("ThreadMarkedAsRead", (threadId, userId) => {});
connection.on("UserOnline", (userId) => {});
connection.on("UserOffline", (userId) => {});
connection.on("UserTyping", (userId) => {});
connection.on("UserStoppedTyping", (userId) => {});
```

## API Response Formats

### Notification Object
```json
{
  "id": 123,
  "title": "New Message",
  "message": "You have a new message from John",
  "type": "Message",
  "relatedEntityId": "thread-456",
  "createdAt": "2024-01-15T10:30:00Z",
  "isRead": false
}
```

### Chat Message Object
```json
{
  "id": 789,
  "threadId": 456,
  "senderId": "user-123",
  "senderName": "John Doe",
  "senderProfilePhoto": "https://...",
  "content": "Hello, how are you?",
  "mediaUrl": null,
  "type": 0,
  "createdAt": "2024-01-15T10:30:00Z",
  "isRead": true
}
```

### Chat Thread Object
```json
{
  "id": 456,
  "clientId": "client-123",
  "trainerId": "trainer-456",
  "lastMessageAt": "2024-01-15T10:30:00Z",
  "isPriority": false,
  "unreadCount": 3,
  "messages": [
    { /* message objects */ }
  ]
}
```

## Code Examples

### Create Notification from Backend
```csharp
[Inject] INotificationService notificationService { get; set; }

var notification = await notificationService.CreateNotificationAsync(
    userId: adminId,
    title: "New Request",
    message: "A new trainer approval request is pending",
    type: (int)NotificationType.TrainerApproval,
    relatedEntityId: trainerId
);
```

### Send Message from Backend
```csharp
[Inject] IChatService chatService { get; set; }

var message = await chatService.SendMessageAsync(
    threadId: 456,
    senderId: adminId,
    request: new SendMessageRequest
    {
        Content = "Message content",
        Type = 0
    }
);
```

### Get Admin Unread Count
```csharp
var count = await notificationService.GetUnreadNotificationCountAsync(adminId);
```

### Mark All as Read
```csharp
await notificationService.MarkAllNotificationsAsReadAsync(adminId);
```

### Connect to SignalR Hub (JavaScript)
```javascript
const connection = new signalR.HubConnectionBuilder()
    .withUrl("/hubs/admin-notifications", {
        accessTokenFactory: () => localStorage.getItem('token')
    })
    .withAutomaticReconnect()
    .build();

connection.start().catch(err => console.error(err));
```

### Listen for Notifications
```javascript
connection.on("NotificationBroadcast", (notification) => {
    console.log("New notification:", notification);
    // Update UI
    addNotificationToUI(notification);
});
```

### Send Message via JavaScript
```javascript
await chatConnection.invoke("SendMessage", threadId, {
    content: "Hello!",
    type: 0
});
```

## CSS Classes for Styling

### Notification Styles
```css
.notification-item        /* Container for notification */
.notification-item.unread /* Unread notification styling */
.notification-item.read   /* Read notification styling */
.notification-badge       /* Unread count badge */
```

### Chat Styles
```css
.message-item             /* Container for message */
.message-item.own         /* Sent message styling */
.message-item.unread      /* Unread message styling */
.online                   /* Online user indicator */
.offline                  /* Offline user indicator */
```

## Common JavaScript Functions

### Update Notification Badge
```javascript
updateNotificationBadge(count) {
    const badge = document.getElementById('notificationBadge');
    if (count > 0) {
        badge.textContent = count > 9 ? '9+' : count;
        badge.style.display = 'block';
    } else {
        badge.style.display = 'none';
    }
}
```

### Load Chat Thread
```javascript
loadThread(threadId) {
    fetch(`/admin/chats/thread/${threadId}`)
        .then(r => r.text())
        .then(html => {
            document.getElementById('chatArea').innerHTML = html;
        });
}
```

### Mark Notification as Read
```javascript
markNotificationAsRead(notificationId) {
    fetch(`/admin/notifications/mark-as-read/${notificationId}`, {
        method: 'POST'
    })
    .then(r => r.json())
    .then(data => {
        if (data.success) {
            // Update UI
        }
    });
}
```

## Environment Variables (Optional)

Add to `appsettings.json` if needed:
```json
{
  "SignalR": {
    "MaximumReceiveMessageSize": 33554432,
    "HandshakeTimeout": 5000,
    "KeepAliveInterval": 15000
  }
}
```

## Database Queries

### Get User Notifications
```sql
SELECT * FROM Notifications 
WHERE UserId = @UserId 
ORDER BY CreatedAt DESC
```

### Get Unread Count
```sql
SELECT COUNT(*) 
FROM Notifications 
WHERE UserId = @UserId AND IsRead = 0
```

### Get Chat Threads for User
```sql
SELECT * FROM MessageThreads
WHERE ClientId = @UserId OR TrainerId = @UserId
ORDER BY LastMessageAt DESC
```

### Get Messages in Thread
```sql
SELECT * FROM Messages
WHERE ThreadId = @ThreadId
ORDER BY CreatedAt ASC
```

## Error Handling

### Common Errors and Solutions

| Error | Cause | Solution |
|-------|-------|----------|
| Hub not connected | Authentication failed | Verify JWT token |
| Messages not syncing | Different thread ID | Ensure JoinThread called |
| Badge not updating | Connection lost | Check network, reconnect |
| 404 on endpoint | Wrong route | Verify `/admin/` prefix |
| CORS error | Wrong origin | Check CORS policy |

## Performance Tips

1. **Pagination**: Load notifications in batches of 20
2. **Caching**: Cache thread list for 30 seconds
3. **Lazy Loading**: Load message history on scroll
4. **Debouncing**: Debounce typing indicators
5. **Compression**: Enable message compression for large files

## Testing with cURL

### Get Unread Count
```bash
curl -H "Authorization: Bearer TOKEN" \
  https://localhost:7001/admin/notifications/unread-count
```

### Mark as Read
```bash
curl -X POST \
  -H "Authorization: Bearer TOKEN" \
  https://localhost:7001/admin/notifications/mark-as-read/123
```

### Send Message
```bash
curl -X POST \
  -H "Authorization: Bearer TOKEN" \
  -H "Content-Type: application/json" \
  -d '{"threadId": 456, "content": "Hello", "type": 0}' \
  https://localhost:7001/admin/chats/send-message
```

## Browser DevTools Debugging

### Check SignalR Connection
```javascript
// In browser console
connection.state // Check connection state (Connected, Connecting, Reconnecting, Disconnected)
connection.invoke("MethodName") // Test method invocation
```

### Monitor Hub Messages
```javascript
// Add logging
connection.on("receiveMessage", msg => {
    console.log("Received:", msg);
});
```

### Check Network Activity
1. Open DevTools → Network tab
2. Filter by `WS` (WebSocket)
3. Look for `/hubs/admin-notifications` and `/hubs/chat` connections
4. Check Message tab for real-time messages

## Related Files
- `ADMIN_INSTALLATION_GUIDE.md` - Setup instructions
- `ADMIN_NOTIFICATIONS_CHAT_GUIDE.md` - Detailed documentation
- `ITI.Gymunity.FP.Admin.MVC/Program.cs` - Configuration
- `ITI.Gymunity.FP.APIs/Program.cs` - Hub mapping

---
**Version**: 1.0 | **Last Updated**: 2024
