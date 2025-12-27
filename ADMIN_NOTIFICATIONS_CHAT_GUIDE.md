# Admin Portal Notifications & Chat Implementation Guide

## Overview
This document describes the implementation of real-time notifications and chat functionality for the Admin Portal using SignalR.

## Architecture

### Components

#### 1. **SignalR Hubs**

**AdminNotificationHub** (`ITI.Gymunity.FP.APIs/Hubs/AdminNotificationHub.cs`)
- Real-time notification delivery for admin users
- Methods:
  - `GetAdminNotifications()`: Fetch all notifications for the current admin
  - `GetUnreadCount()`: Get count of unread notifications
  - `BroadcastAdminNotification()`: Send notification to all connected admins
  - `SendUrgentAlert()`: Send urgent alert to specific admin
  - `MarkAsRead()`: Mark notification as read
- Events:
  - `AdminOnline`: Broadcast when admin comes online
  - `AdminOffline`: Broadcast when admin goes offline
  - `NotificationBroadcast`: Receive notification broadcast
  - `UrgentAlert`: Receive urgent alert

**ChatHub** (Previously existing - reused)
- Real-time messaging between admins and users
- Supports message threads, typing indicators, and read status

### Controllers

#### NotificationsController (`ITI.Gymunity.FP.Admin.MVC/Controllers/NotificationsController.cs`)
- **GET `/admin/notifications`**: Display all notifications
- **POST `/admin/notifications/mark-as-read/{id}`**: Mark single notification as read
- **POST `/admin/notifications/mark-all-as-read`**: Mark all notifications as read
- **GET `/admin/notifications/unread-count`**: Get unread notification count
- **GET `/admin/notifications/unread`**: Get unread notifications (AJAX)

#### ChatsController (`ITI.Gymunity.FP.Admin.MVC/Controllers/ChatsController.cs`)
- **GET `/admin/chats`**: Display chat list
- **GET `/admin/chats/thread/{threadId}`**: Load specific chat thread
- **POST `/admin/chats/send-message`**: Send a message
- **POST `/admin/chats/mark-as-read/{messageId}`**: Mark message as read
- **POST `/admin/chats/mark-thread-as-read/{threadId}`**: Mark entire thread as read
- **GET `/admin/chats/unread-count`**: Get total unread messages count

### Views

#### Notifications Views
- **Index.cshtml**: Main notifications page with full notification list
- **_NotificationsPartial.cshtml**: Partial view for notification items

#### Chat Views
- **Index.cshtml**: Main chat page with thread list and message area
- **_ChatThreadPartial.cshtml**: Partial view for individual chat thread

#### Shared Widget
- **_NotificationChatWidget.cshtml**: Reusable widget for navbar integration

### ViewModels

#### Notifications
- **NotificationViewModel**: Single notification data
- **NotificationsListViewModel**: Collection of notifications with counts

#### Chat
- **ChatMessageViewModel**: Single message data
- **ChatThreadViewModel**: Chat thread with all messages
- **ChatListViewModel**: Collection of chat threads

## Installation & Setup

### Step 1: Update Admin MVC Program.cs
The Program.cs has been updated to:
- Add SignalR services
- Configure CORS for SignalR
- Map the notification and chat endpoints

### Step 2: Update Shared Layout
Add the notification widget to `_AdminLayout.cshtml`:

```html
<!-- In navbar where you want notifications/chat icons -->
@Html.PartialAsync("_NotificationChatWidget")
```

### Step 3: Verify Database
Ensure your database has:
- `Notifications` table
- `MessageThreads` table
- `Messages` table

## Usage Guide

### Admin Notifications Page
Navigate to `/admin/notifications` to view all notifications with real-time updates.

**Features:**
- View all notifications (read and unread)
- Mark individual notifications as read
- Mark all notifications as read at once
- Real-time badge showing unread count
- Auto-refresh via SignalR

### Admin Chat Page
Navigate to `/admin/chats` to access messaging.

**Features:**
- View all chat threads with unread counters
- Load and display conversation history
- Send messages in real-time
- Typing indicators (ready to implement)
- Online/offline status indicators
- Read receipts for messages

### Navbar Widget
The widget appears in the admin navbar with:
- Notification bell with unread badge
- Chat icon with unread badge
- Dropdown showing recent notifications
- Quick access to full pages

## Real-time Features

### SignalR Events

#### Admin Notification Hub Events

**Client receives:**
- `AdminOnline`: Admin user came online
- `AdminOffline`: Admin user went offline
- `NotificationBroadcast`: Broadcast notification
- `UrgentAlert`: Urgent alert notification
- `NotificationRead`: Notification marked as read
- `UnreadCount`: Updated unread count

**Client sends:**
- `GetAdminNotifications()`: Request notifications
- `GetUnreadCount()`: Request unread count
- `BroadcastAdminNotification()`: Send to all admins
- `SendUrgentAlert()`: Send to specific admin
- `MarkAsRead()`: Mark notification read

#### Chat Hub Events

**Client receives:**
- `MessageReceived`: New message in thread
- `MessageMarkedAsRead`: Message read
- `ThreadMarkedAsRead`: Thread read
- `UserOnline`: User came online
- `UserOffline`: User went offline
- `UserTyping`: User is typing
- `UserStoppedTyping`: User stopped typing

**Client sends:**
- `SendMessage()`: Send message
- `JoinThread()`: Join thread group
- `LeaveThread()`: Leave thread group
- `MarkMessageAsRead()`: Mark message read
- `MarkThreadAsRead()`: Mark thread read
- `UserTyping()`: Notify typing
- `UserStoppedTyping()`: Notify stop typing

## Security Considerations

### Authentication
- All hubs are decorated with `[Authorize]` attribute
- User identity is extracted from JWT claims
- SignalR connections require valid authentication

### Authorization
- Users can only see their own notifications and chats
- Admins can broadcast to all admins
- Individual alerts are sent to specific users only

### CORS
- CORS policy `adminSignalRPolicy` allows SignalR connections
- Credentials are allowed for WebSocket connections

## Customization Guide

### Adding New Notification Types
1. Update `NotificationType` enum in Domain models
2. Create service method to trigger notifications
3. Add corresponding UI element in notification display

### Styling
- Notifications and chat use Bootstrap classes
- Custom CSS provided for animations and states
- Modify `_NotificationChatWidget.cshtml` styles as needed

### Adding Typing Indicators
The infrastructure is ready. Add in `_ChatThreadPartial.cshtml`:

```javascript
document.getElementById('messageInput').addEventListener('input', () => {
    chatHubConnection.invoke("UserTyping", currentThreadId);
});
```

## Troubleshooting

### SignalR Connection Issues
1. Check browser console for connection errors
2. Verify authentication token is being sent
3. Ensure SignalR endpoints are mapped in Program.cs
4. Check CORS configuration

### Notifications Not Appearing
1. Verify admin is authenticated
2. Check notification creation in application service
3. Ensure database is properly seeded
4. Check browser network tab for hub requests

### Chat Messages Not Loading
1. Verify chat thread exists in database
2. Check user is part of the thread
3. Ensure messages are associated with thread
4. Check connection to ChatHub

### Badge Not Updating
1. Verify SignalR connection is active
2. Check notification service response
3. Ensure JavaScript fetch calls are completing
4. Check for JavaScript errors in console

## Testing Checklist

- [ ] Admin can view notifications page
- [ ] Notification badge updates in real-time
- [ ] Mark single notification as read works
- [ ] Mark all notifications as read works
- [ ] Admin can view chat page
- [ ] Chat thread list loads correctly
- [ ] Can open individual chat thread
- [ ] Can send messages in thread
- [ ] Messages appear for both users in real-time
- [ ] Messages marked as read
- [ ] Unread badge updates
- [ ] Online/offline status updates
- [ ] Notification widget in navbar appears
- [ ] Dropdown notifications load correctly

## File Structure

```
ITI.Gymunity.FP.Admin.MVC/
├── Controllers/
│   ├── NotificationsController.cs
│   └── ChatsController.cs
├── ViewModels/
│   ├── Notifications/
│   │   ├── NotificationViewModel.cs
│   │   └── NotificationsListViewModel.cs
│   └── Chat/
│       ├── ChatMessageViewModel.cs
│       ├── ChatThreadViewModel.cs
│       └── ChatListViewModel.cs
└── Views/
    ├── Notifications/
    │   ├── Index.cshtml
    │   └── _NotificationsPartial.cshtml
    ├── Chats/
    │   ├── Index.cshtml
    │   └── _ChatThreadPartial.cshtml
    └── Shared/
        └── _NotificationChatWidget.cshtml

ITI.Gymunity.FP.APIs/
└── Hubs/
    └── AdminNotificationHub.cs
```

## Future Enhancements

1. **Typing Indicators**: Show when user is typing
2. **Message Search**: Search through messages
3. **Chat Groups**: Group chats with multiple participants
4. **File Sharing**: Share files in chat
5. **Notification Preferences**: Admin can configure notification settings
6. **Message History**: Archive and search messages
7. **Admin-to-Admin Priority Chats**: Direct admin communication
8. **Notification Templates**: Pre-made notification messages
9. **Scheduled Notifications**: Schedule notifications for later
10. **Notification Categories**: Filter notifications by type

## Support & Maintenance

For issues or questions:
1. Check the troubleshooting section
2. Review SignalR logs in console
3. Verify database connectivity
4. Check application logs
5. Contact development team

---

**Last Updated**: 2024
**Version**: 1.0
