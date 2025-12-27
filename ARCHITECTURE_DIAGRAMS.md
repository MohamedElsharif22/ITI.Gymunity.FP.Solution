# Admin Portal Notifications & Chat - Architecture Diagrams

## System Architecture Diagram

```
┌────────────────────────────────────────────────────────────────┐
│                       Admin Portal (UI)                        │
│  ┌──────────────────────────────────────────────────────────┐  │
│  │                   Razor Pages Views                      │  │
│  │  ┌─────────────────────────────────────────────────────┐ │  │
│  │  │ Navigation/Layout                                  │ │  │
│  │  │ ├─ _NotificationChatWidget (Real-Time Badge)       │ │  │
│  │  │ │  ├─ Notification Bell (Click for Dropdown)       │ │  │
│  │  │ │  └─ Chat Icon (Click for Messages)               │ │  │
│  │  │ └─ Main Content Area                               │ │  │
│  │  │    ├─ /admin/notifications                         │ │  │
│  │  │    └─ /admin/chats                                 │ │  │
│  │  └─────────────────────────────────────────────────────┘ │  │
│  │                                                           │  │
│  │  ┌─────────────────────────────────────────────────────┐ │  │
│  │  │        JavaScript (SignalR Client)                  │ │  │
│  │  │  ┌────────────────────────────────────────────────┐ │ │  │
│  │  │  │ AdminNotificationHub Connection                │ │ │  │
│  │  │  │ ├─ Monitor: GetUnreadCount()                   │ │ │  │
│  │  │  │ ├─ Receive: NotificationBroadcast              │ │ │  │
│  │  │  │ └─ Receive: UrgentAlert                        │ │ │  │
│  │  │  ├────────────────────────────────────────────────┤ │ │  │
│  │  │  │ ChatHub Connection                             │ │ │  │
│  │  │  │ ├─ Invoke: SendMessage()                       │ │ │  │
│  │  │  │ ├─ Invoke: JoinThread()                        │ │ │  │
│  │  │  │ └─ Receive: MessageReceived                    │ │ │  │
│  │  │  └────────────────────────────────────────────────┘ │ │  │
│  │  └─────────────────────────────────────────────────────┘ │  │
│  └──────────────────────────────────────────────────────────┘  │
└───────────────────┬──────────────────────────────────────┬────┘
                    │ WebSocket                            │
                    │ (SignalR)                            │
                    │                                      │
┌───────────────────▼──────────────────────────────────────▼────┐
│              Backend API (Hubs & Controllers)                  │
├────────────────────────────────────────────────────────────────┤
│                                                                │
│  ┌─────────────────────────────────────────────────────────┐  │
│  │                  SignalR Hubs                           │  │
│  │  ┌────────────────────┐   ┌────────────────────────┐   │  │
│  │  │ Admin              │   │ Chat                   │   │  │
│  │  │ NotificationHub    │   │ Hub                    │   │  │
│  │  │ ├─ Broadcast       │   │ ├─ SendMessage         │   │  │
│  │  │ ├─ UrgentAlert     │   │ ├─ JoinThread          │   │  │
│  │  │ └─ Status Tracking │   │ ├─ MarkAsRead          │   │  │
│  │  │                    │   │ └─ Typing Indicators   │   │  │
│  │  └────────────────────┘   └────────────────────────┘   │  │
│  └─────────────────────────────────────────────────────────┘  │
│                            ▲                                   │
│                            │ Uses Services                     │
│  ┌─────────────────────────▼─────────────────────────────┐  │
│  │            Application Services Layer                  │  │
│  │  ┌───────────────────────────────────────────────────┐ │  │
│  │  │ INotificationService                             │ │  │
│  │  │ ├─ CreateNotificationAsync()                     │ │  │
│  │  │ ├─ GetUserNotificationsAsync()                  │ │  │
│  │  │ ├─ MarkNotificationAsReadAsync()                │ │  │
│  │  │ └─ GetUnreadNotificationCountAsync()            │ │  │
│  │  └───────────────────────────────────────────────────┘ │  │
│  │  ┌───────────────────────────────────────────────────┐ │  │
│  │  │ IChatService                                      │ │  │
│  │  │ ├─ SendMessageAsync()                           │ │  │
│  │  │ ├─ GetMessageThreadAsync()                      │ │  │
│  │  │ ├─ MarkMessageAsReadAsync()                     │ │  │
│  │  │ └─ GetUserChatsAsync()                          │ │  │
│  │  └───────────────────────────────────────────────────┘ │  │
│  │  ┌───────────────────────────────────────────────────┐ │  │
│  │  │ ISignalRConnectionManager                         │ │  │
│  │  │ ├─ AddConnection()                              │ │  │
│  │  │ ├─ RemoveConnection()                           │ │  │
│  │  │ └─ GetConnectionId()                            │ │  │
│  │  └───────────────────────────────────────────────────┘ │  │
│  └─────────────────────────────────────────────────────────┘  │
│                            ▲                                   │
│                            │ Uses Repositories               │
│  ┌─────────────────────────▼─────────────────────────────┐  │
│  │              Controllers Layer                         │  │
│  │  ┌───────────────────────────────────────────────────┐ │  │
│  │  │ NotificationsController                           │ │  │
│  │  │ ├─ GET /admin/notifications                      │ │  │
│  │  │ ├─ POST /mark-as-read/{id}                       │ │  │
│  │  │ ├─ POST /mark-all-as-read                        │ │  │
│  │  │ └─ GET /unread-count                             │ │  │
│  │  └───────────────────────────────────────────────────┘ │  │
│  │  ┌───────────────────────────────────────────────────┐ │  │
│  │  │ ChatsController                                   │ │  │
│  │  │ ├─ GET /admin/chats                              │ │  │
│  │  │ ├─ GET /thread/{id}                              │ │  │
│  │  │ ├─ POST /send-message                            │ │  │
│  │  │ └─ POST /mark-as-read/{id}                       │ │  │
│  │  └───────────────────────────────────────────────────┘ │  │
│  └─────────────────────────────────────────────────────────┘  │
└───────────────────┬──────────────────────────────────────┬────┘
                    │ Read/Write                           │
                    │ Data                                 │
┌───────────────────▼──────────────────────────────────────▼────┐
│                    SQL Database                               │
├────────────────────────────────────────────────────────────────┤
│  ┌─────────────────┐  ┌─────────────────┐  ┌──────────────┐  │
│  │ Notifications   │  │ MessageThreads  │  │  Messages    │  │
│  │ ├─ Id           │  │ ├─ Id           │  │ ├─ Id        │  │
│  │ ├─ UserId       │  │ ├─ ClientId     │  │ ├─ ThreadId  │  │
│  │ ├─ Title        │  │ ├─ TrainerId    │  │ ├─ SenderId  │  │
│  │ ├─ Message      │  │ ├─ LastMsg...   │  │ ├─ Content   │  │
│  │ ├─ Type         │  │ ├─ IsPriority   │  │ ├─ Type      │  │
│  │ ├─ CreatedAt    │  │ └─ CreatedAt    │  │ ├─ CreatedAt │  │
│  │ └─ IsRead       │  │                 │  │ └─ IsRead    │  │
│  └─────────────────┘  └─────────────────┘  └──────────────┘  │
│                                                                │
│  Indexes:                                                      │
│  ├─ Notifications(UserId, IsRead, CreatedAt)                  │
│  ├─ MessageThreads(ClientId, TrainerId, LastMessageAt)        │
│  └─ Messages(ThreadId, CreatedAt)                             │
└────────────────────────────────────────────────────────────────┘
```

## Request/Response Flow Diagram

### Notification Creation Flow
```
Admin Portal
    │
    ├─► Show notification badge
    │    ▲
    │    │
    └────┤
         │
         ▼
    Backend Service
         │
         ├─► INotificationService.CreateNotificationAsync()
         │
         ├─► Save to Notification table
         │
         ├─► SignalR Broadcast to AdminNotificationHub
         │
         └─► All connected admins receive:
              ├─ NotificationBroadcast event
              ├─ UnreadCount update
              └─ Badge animation
```

### Chat Message Flow
```
Admin Portal                Backend                Database
    │                          │                       │
    ├─ User types message      │                       │
    │                          │                       │
    ├─ Clicks Send             │                       │
    │                          │                       │
    └─► ChatHub.SendMessage() ─┤                       │
         (threadId, content)    │                       │
                                ├─► IChatService      │
                                │   SendMessageAsync() │
                                │                      │
                                └──────► Save Message ──────┐
                                         table entry        │
                                         │◄──────────────────┘
                                         │
                                         ├─► SignalR Broadcast
                                         │   MessageReceived event
                                         │
                                         └─► Update thread's
                                             LastMessageAt
                                         
Admin receives:
    ▼
├─ MessageReceived event
├─ Update UI with new message
├─ Scroll to bottom
└─ Unread badge updates for other users
```

## Component Relationship Diagram

```
┌─────────────────────────────────────────────────────────────┐
│                    Admin Portal Views                       │
└────┬───────────────────────────────────────────────┬────────┘
     │                                               │
     ▼                                               ▼
┌────────────────────────┐                  ┌─────────────────────┐
│ Notifications/Index    │                  │  Chats/Index        │
│ ├─ NotificationsControl│                  │ ├─ ChatsController  │
│ ├─ ViewModel           │                  │ ├─ ViewModel        │
│ └─ Partial Views       │                  │ └─ Partial Views    │
└────┬───────────────────┘                  └────┬────────────────┘
     │                                           │
     ├─────────┬───────────────┬────────────────┤
     │         │               │                │
     ▼         ▼               ▼                ▼
    Model    Controller     Service          SignalR Hub
     │         │               │                │
     ▼         ▼               ▼                ▼
┌─────────────────────────────────────────────────────────────┐
│            Shared _NotificationChatWidget                   │
│  ├─ Real-time Badge Updates                                 │
│  ├─ Dropdown Preview                                        │
│  └─ SignalR Event Handlers                                  │
└─────────────────────────────────────────────────────────────┘
             │ SignalR WebSocket
             ▼
┌─────────────────────────────────────────────────────────────┐
│                  API Hubs (Backend)                         │
│  ├─ AdminNotificationHub                                    │
│  └─ ChatHub                                                 │
└────┬───────────────────────────────────────────┬────────────┘
     │                                           │
     ▼                                           ▼
┌──────────────────────┐            ┌────────────────────────┐
│ NotificationService  │            │   ChatService          │
│ ├─ Create            │            │ ├─ SendMessage         │
│ ├─ GetByUser         │            │ ├─ GetThread           │
│ ├─ MarkAsRead        │            │ ├─ MarkAsRead          │
│ └─ GetUnreadCount    │            │ └─ GetUserChats        │
└──────────┬───────────┘            └────┬───────────────────┘
           │                             │
           └──────────┬──────────────────┘
                      ▼
          ┌────────────────────────┐
          │  Unit of Work Pattern  │
          │  Repository<T>         │
          │  ├─ Notification       │
          │  ├─ MessageThread      │
          │  ├─ Message            │
          │  └─ AppUser            │
          └──────────┬─────────────┘
                     ▼
          ┌──────────────────────┐
          │   SQL Database       │
          │  ├─ Notifications    │
          │  ├─ MessageThreads   │
          │  └─ Messages         │
          └──────────────────────┘
```

## Data Flow Diagram

### Notification Data Flow
```
User Action (Trainer applies)
        │
        ▼
Create Notification
        │
        ├─► Notification Object
        │   ├─ UserId: "admin-123"
        │   ├─ Title: "New Application"
        │   ├─ Message: "Trainer John applied"
        │   ├─ Type: TrainerApply
        │   └─ CreatedAt: Now
        │
        ▼
NotificationService.CreateAsync()
        │
        ├─► Save to Database
        │   └─ INSERT INTO Notifications...
        │
        ├─► Return NotificationResponse
        │   └─ Notification DTO
        │
        ▼
SignalR AdminNotificationHub
        │
        ├─► Query NotificationService.GetUnreadCount()
        │
        ├─► Broadcast to all admins
        │   ├─ UnreadCount event
        │   └─ NotificationBroadcast event
        │
        ▼
Admin Portal JavaScript
        │
        ├─► Receive SignalR events
        │
        ├─► Update Badge
        │   └─ Show count "1"
        │
        ├─► Update Dropdown
        │   └─ Add notification preview
        │
        └─► Toast/Alert
            └─ Visual/Audio notification
```

### Chat Message Data Flow
```
Admin Types & Sends Message
        │
        ▼
Message Object
    {
        threadId: 456,
        content: "Approved",
        type: 0
    }
        │
        ▼
ChatsController.SendMessage()
        │
        ├─► Validate User
        │   └─ Extract from Claims
        │
        ├─► Call ChatService
        │
        ▼
ChatService.SendMessageAsync()
        │
        ├─► Create Message object
        │   ├─ ThreadId: 456
        │   ├─ SenderId: "admin-123"
        │   ├─ Content: "Approved"
        │   ├─ CreatedAt: Now
        │   └─ IsRead: false
        │
        ├─► Update MessageThread
        │   └─ LastMessageAt = Now
        │
        ├─► Save to Database
        │   └─ INSERT INTO Messages...
        │
        ▼
Return MessageResponse
        │
        ├─ Include Sender Info
        ├─ Include Timestamp
        └─ Include Message ID
        │
        ▼
SignalR ChatHub
        │
        ├─► Broadcast to Group
        │   └─ "thread-456"
        │
        ├─► Send MessageReceived event
        │   └─ Include all message data
        │
        ▼
All Admin Portals in Thread
        │
        ├─► Receive MessageReceived event
        │
        ├─► Update Message List
        │   ├─ Add new message to DOM
        │   ├─ Animate in
        │   └─ Scroll to bottom
        │
        ├─► Update Unread Badge
        │   └─ Increment counter
        │
        └─► Play notification sound
            └─ Optional
```

## Real-Time Event Sequence

### Notification Scenario
```
Timeline:
─────────────────────────────────────────────────────────────

T=0s:     Admin Portal Loads
          └─ Establish WebSocket to /hubs/admin-notifications

T=1s:     Backend creates notification
          └─ await notificationService.CreateNotificationAsync(...)

T=1.1s:   SignalR broadcasts to all connected admins
          └─ Clients.All.SendAsync("NotificationBroadcast", ...)

T=1.2s:   Admin Portal receives event
          └─ connection.on("NotificationBroadcast", ...)

T=1.3s:   JavaScript updates UI
          ├─ Update badge count
          ├─ Add to dropdown list
          └─ Show animation

T=1.5s:   Admin sees notification
          └─ Clicks on bell icon

T=2s:     Admin reads notification
          └─ Click mark as read button

T=2.1s:   POST /admin/notifications/mark-as-read/123
          └─ ChatsController.MarkAsRead(123)

T=2.2s:   Backend updates database
          └─ UPDATE Notifications SET IsRead=1 WHERE Id=123

T=2.3s:   SignalR broadcasts read status
          └─ Clients.All.SendAsync("NotificationRead", 123)

T=2.4s:   UI updates
          └─ Remove notification item or gray out
```

### Chat Message Scenario
```
Timeline:
─────────────────────────────────────────────────────────────

T=0s:     Both Admins have chat page open
          ├─ Admin A: Connected to /hubs/chat, joined thread-456
          └─ Admin B: Connected to /hubs/chat, joined thread-456

T=3s:     Admin A types message and clicks Send
          └─ POST /admin/chats/send-message
             {
               threadId: 456,
               content: "Looks good!",
               type: 0
             }

T=3.1s:   ChatsController receives request
          ├─ Validate authorization
          ├─ Extract senderId from claims
          └─ Call chatService.SendMessageAsync()

T=3.2s:   ChatService saves to database
          ├─ INSERT INTO Messages...
          ├─ UPDATE MessageThreads SET LastMessageAt=Now
          └─ Return MessageResponse

T=3.3s:   Controller returns JSON response
          └─ { success: true, message: {...} }

T=3.4s:   JavaScript invokes SignalR method
          └─ chatConnection.invoke("SendMessage", 456, {...})

T=3.5s:   ChatHub.SendMessage() executes
          ├─ Validate user
          ├─ Verify thread access
          ├─ Save to database (already saved)
          └─ Broadcast to group

T=3.6s:   SignalR broadcasts to thread-456 group
          └─ Clients.Group("thread-456")
                .SendAsync("MessageReceived", message)

T=3.7s:   Both Admins receive MessageReceived event
          ├─ Admin A: Updates own message (already showed optimistically)
          └─ Admin B: Receives new message from Admin A

T=3.8s:   UI updates for Admin B
          ├─ Add message to chat list
          ├─ Show sender info
          ├─ Show timestamp
          ├─ Animate message in
          └─ Scroll to bottom

T=4s:     Admin B sees message in real-time
          └─ Appears instantly (no reload needed)

T=4.5s:   Admin B reads message
          └─ Focus on chat area (auto-read) or click read button

T=4.6s:   ChatsController.MarkThreadAsRead(456)
          └─ Update message read status

T=4.7s:   SignalR broadcasts read status
          └─ Clients.Group("thread-456")
                .SendAsync("MessageMarkedAsRead", messageId)

T=4.8s:   Admin A sees read receipt
          └─ Message marked as read in UI
```

## State Management Diagram

```
Admin Portal State
├─ Connection States
│  ├─ AdminNotificationHub
│  │  ├─ Connecting
│  │  ├─ Connected ✓
│  │  ├─ Reconnecting
│  │  └─ Disconnected
│  └─ ChatHub
│     ├─ Connecting
│     ├─ Connected ✓
│     ├─ Reconnecting
│     └─ Disconnected
│
├─ Notification State
│  ├─ Unread Count (0-999+)
│  ├─ Notifications List
│  │  └─ [ ] Notification 1 (unread)
│  │  └─ [✓] Notification 2 (read)
│  └─ Badge Visibility
│     ├─ Hidden (count=0)
│     └─ Visible (count>0)
│
├─ Chat State
│  ├─ Selected Thread ID
│  │  └─ 456
│  ├─ Thread List
│  │  ├─ Thread 456
│  │  │  ├─ Unread: 3
│  │  │  └─ LastMessage: "..."
│  │  └─ Thread 789
│  │     ├─ Unread: 0
│  │     └─ LastMessage: "..."
│  │
│  ├─ Current Thread Messages
│  │  └─ [ Message 1 ]
│  │  └─ [ Message 2 ] <- Scroll position
│  │  └─ [ Message 3 ]
│  │
│  └─ User Status
│     ├─ User 123: online
│     └─ User 456: offline
│
└─ UI State
   ├─ Modal States
   │  ├─ Notifications Dropdown (open/closed)
   │  └─ Chat Panel (open/closed)
   ├─ Loading States
   │  ├─ Loading Notifications (true/false)
   │  └─ Loading Messages (true/false)
   └─ Animations
      ├─ Badge Pulse (true/false)
      └─ Message Entrance (true/false)
```

---

**These diagrams provide a visual reference for understanding the system architecture and data flow.**
