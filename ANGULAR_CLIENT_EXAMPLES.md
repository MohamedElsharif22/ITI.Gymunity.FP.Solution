# SignalR Client Implementation Examples

## Angular/TypeScript Client Examples

### Installation

```bash
npm install @microsoft/signalr
```

### 1. Chat Service

**chat.service.ts**

```typescript
import * as signalR from "@microsoft/signalr";
import { Injectable } from "@angular/core";
import { BehaviorSubject, Observable } from "rxjs";

export interface Message {
  id: number;
  threadId: number;
  senderId: string;
  senderName: string;
  senderProfilePhoto: string;
  content: string;
  mediaUrl?: string;
  type: number;
  createdAt: Date;
  isRead: boolean;
}

export interface SendMessageRequest {
  content: string;
  mediaUrl?: string;
  type: number;
}

@Injectable({
  providedIn: "root"
})
export class ChatService {
  private connection: signalR.HubConnection;
  private messageSubject = new BehaviorSubject<Message | null>(null);
  private messagesSubject = new BehaviorSubject<Message[]>([]);
  private connectionStatusSubject = new BehaviorSubject<string>("disconnected");
  private typingUsersSubject = new BehaviorSubject<Set<string>>(new Set());
  
  public message$ = this.messageSubject.asObservable();
  public messages$ = this.messagesSubject.asObservable();
  public connectionStatus$ = this.connectionStatusSubject.asObservable();
  public typingUsers$ = this.typingUsersSubject.asObservable();

  constructor(private authService: AuthService) {
    this.initializeConnection();
  }

  private initializeConnection(): void {
    this.connection = new signalR.HubConnectionBuilder()
      .withUrl("https://localhost:5131/hubs/chat", {
        accessTokenFactory: () => this.authService.getToken() ?? "",
        skipNegotiation: true,
        transport: signalR.HttpTransportType.WebSockets
      })
      .withAutomaticReconnect([0, 500, 1000, 2000, 5000, 10000])
      .withHubProtocol(new signalR.JsonHubProtocol())
      .configureLogging(signalR.LogLevel.Information)
      .build();

    this.setupEventListeners();
  }

  private setupEventListeners(): void {
    // Connection events
    this.connection.onreconnecting(() => {
      this.connectionStatusSubject.next("reconnecting");
      console.log("Attempting to reconnect...");
    });

    this.connection.onreconnected(() => {
      this.connectionStatusSubject.next("connected");
      console.log("Reconnected successfully");
    });

    this.connection.onclose((error) => {
      this.connectionStatusSubject.next("disconnected");
      console.log("Connection closed:", error);
    });

    // Message events
    this.connection.on("MessageReceived", (message: Message) => {
      console.log("Message received:", message);
      this.messageSubject.next(message);
      
      // Add to messages list
      const currentMessages = this.messagesSubject.value;
      this.messagesSubject.next([...currentMessages, message]);
    });

    // Status events
    this.connection.on("UserOnline", (userId: string) => {
      console.log(`User ${userId} is online`);
    });

    this.connection.on("UserOffline", (userId: string) => {
      console.log(`User ${userId} is offline`);
    });

    this.connection.on("UserJoinedThread", (userId: string) => {
      console.log(`User ${userId} joined thread`);
    });

    this.connection.on("UserLeftThread", (userId: string) => {
      console.log(`User ${userId} left thread`);
    });

    // Read receipt events
    this.connection.on("MessageMarkedAsRead", (messageId: number) => {
      const messages = this.messagesSubject.value;
      const message = messages.find(m => m.id === messageId);
      if (message) {
        message.isRead = true;
        this.messagesSubject.next([...messages]);
      }
    });

    this.connection.on("ThreadMarkedAsRead", (threadId: number, userId: string) => {
      console.log(`Thread ${threadId} marked as read by ${userId}`);
    });

    // Typing indicators
    this.connection.on("UserTyping", (userId: string) => {
      const typingUsers = this.typingUsersSubject.value;
      typingUsers.add(userId);
      this.typingUsersSubject.next(new Set(typingUsers));
    });

    this.connection.on("UserStoppedTyping", (userId: string) => {
      const typingUsers = this.typingUsersSubject.value;
      typingUsers.delete(userId);
      this.typingUsersSubject.next(new Set(typingUsers));
    });

    // Error event
    this.connection.on("Error", (message: string) => {
      console.error("Hub error:", message);
    });
  }

  public async connect(): Promise<void> {
    try {
      await this.connection.start();
      this.connectionStatusSubject.next("connected");
      console.log("SignalR connected");
    } catch (error) {
      console.error("Connection error:", error);
      setTimeout(() => this.connect(), 5000);
    }
  }

  public async disconnect(): Promise<void> {
    await this.connection.stop();
    this.connectionStatusSubject.next("disconnected");
  }

  public async joinThread(threadId: number): Promise<void> {
    return this.connection.invoke("JoinThread", threadId);
  }

  public async leaveThread(threadId: number): Promise<void> {
    return this.connection.invoke("LeaveThread", threadId);
  }

  public async sendMessage(threadId: number, request: SendMessageRequest): Promise<void> {
    return this.connection.invoke("SendMessage", threadId, request);
  }

  public async markMessageAsRead(messageId: number, threadId: number): Promise<void> {
    return this.connection.invoke("MarkMessageAsRead", messageId, threadId);
  }

  public async markThreadAsRead(threadId: number): Promise<void> {
    return this.connection.invoke("MarkThreadAsRead", threadId);
  }

  public async notifyTyping(threadId: number): Promise<void> {
    return this.connection.invoke("UserTyping", threadId);
  }

  public async notifyStoppedTyping(threadId: number): Promise<void> {
    return this.connection.invoke("UserStoppedTyping", threadId);
  }

  public getConnectionStatus(): string {
    return this.connectionStatusSubject.value;
  }

  public isConnected(): boolean {
    return this.connection?.state === signalR.HubConnectionState.Connected;
  }
}
```

### 2. Notification Service

**notification.service.ts**

```typescript
import * as signalR from "@microsoft/signalr";
import { Injectable } from "@angular/core";
import { BehaviorSubject, Observable } from "rxjs";

export interface Notification {
  id: number;
  title: string;
  message: string;
  type: number;
  relatedEntityId?: string;
  createdAt: Date;
  isRead: boolean;
}

@Injectable({
  providedIn: "root"
})
export class NotificationService {
  private connection: signalR.HubConnection;
  private notificationSubject = new BehaviorSubject<Notification | null>(null);
  private notificationsSubject = new BehaviorSubject<Notification[]>([]);
  private unreadCountSubject = new BehaviorSubject<number>(0);
  private connectionStatusSubject = new BehaviorSubject<string>("disconnected");

  public notification$ = this.notificationSubject.asObservable();
  public notifications$ = this.notificationsSubject.asObservable();
  public unreadCount$ = this.unreadCountSubject.asObservable();
  public connectionStatus$ = this.connectionStatusSubject.asObservable();

  constructor(private authService: AuthService) {
    this.initializeConnection();
  }

  private initializeConnection(): void {
    this.connection = new signalR.HubConnectionBuilder()
      .withUrl("https://localhost:5131/hubs/notifications", {
        accessTokenFactory: () => this.authService.getToken() ?? "",
        skipNegotiation: true,
        transport: signalR.HttpTransportType.WebSockets
      })
      .withAutomaticReconnect([0, 500, 1000, 2000, 5000, 10000])
      .withHubProtocol(new signalR.JsonHubProtocol())
      .configureLogging(signalR.LogLevel.Information)
      .build();

    this.setupEventListeners();
  }

  private setupEventListeners(): void {
    this.connection.onreconnecting(() => {
      this.connectionStatusSubject.next("reconnecting");
    });

    this.connection.onreconnected(() => {
      this.connectionStatusSubject.next("connected");
      this.getUnreadNotifications(); // Refresh on reconnect
    });

    this.connection.onclose(() => {
      this.connectionStatusSubject.next("disconnected");
    });

    // Notification events
    this.connection.on("NewNotification", (notification: Notification) => {
      console.log("New notification:", notification);
      this.notificationSubject.next(notification);
      
      const notifications = this.notificationsSubject.value;
      this.notificationsSubject.next([notification, ...notifications]);
      
      // Increment unread count
      this.unreadCountSubject.next(this.unreadCountSubject.value + 1);
    });

    this.connection.on("UnreadNotifications", (notifications: Notification[]) => {
      this.notificationsSubject.next(notifications);
      this.unreadCountSubject.next(notifications.length);
    });

    this.connection.on("UnreadNotificationCount", (count: number) => {
      this.unreadCountSubject.next(count);
    });

    this.connection.on("NotificationMarkedAsRead", (notificationId: number) => {
      const notifications = this.notificationsSubject.value;
      const notification = notifications.find(n => n.id === notificationId);
      if (notification) {
        notification.isRead = true;
        this.notificationsSubject.next([...notifications]);
        this.unreadCountSubject.next(Math.max(0, this.unreadCountSubject.value - 1));
      }
    });

    this.connection.on("AllNotificationsMarkedAsRead", () => {
      const notifications = this.notificationsSubject.value;
      notifications.forEach(n => n.isRead = true);
      this.notificationsSubject.next([...notifications]);
      this.unreadCountSubject.next(0);
    });

    this.connection.on("Error", (message: string) => {
      console.error("Notification hub error:", message);
    });
  }

  public async connect(): Promise<void> {
    try {
      await this.connection.start();
      this.connectionStatusSubject.next("connected");
      console.log("Notification hub connected");
      await this.getUnreadNotifications();
    } catch (error) {
      console.error("Notification connection error:", error);
      setTimeout(() => this.connect(), 5000);
    }
  }

  public async disconnect(): Promise<void> {
    await this.connection.stop();
    this.connectionStatusSubject.next("disconnected");
  }

  public async getUnreadNotifications(): Promise<void> {
    return this.connection.invoke("GetUnreadNotifications");
  }

  public async getUnreadCount(): Promise<void> {
    return this.connection.invoke("GetUnreadNotificationCount");
  }

  public async markAsRead(notificationId: number): Promise<void> {
    return this.connection.invoke("MarkNotificationAsRead", notificationId);
  }

  public async markAllAsRead(): Promise<void> {
    return this.connection.invoke("MarkAllNotificationsAsRead");
  }

  public isConnected(): boolean {
    return this.connection?.state === signalR.HubConnectionState.Connected;
  }
}
```

### 3. Component Example: Chat Thread

**chat-thread.component.ts**

```typescript
import { Component, OnInit, OnDestroy } from "@angular/core";
import { Subject } from "rxjs";
import { takeUntil, debounceTime } from "rxjs/operators";
import { ChatService, Message, SendMessageRequest } from "../services/chat.service";
import { ActivatedRoute } from "@angular/router";

@Component({
  selector: "app-chat-thread",
  templateUrl: "./chat-thread.component.html",
  styleUrls: ["./chat-thread.component.scss"]
})
export class ChatThreadComponent implements OnInit, OnDestroy {
  threadId: number;
  messages: Message[] = [];
  messageText: string = "";
  typingUsers: Set<string> = new Set();
  isConnected: boolean = false;
  isLoading: boolean = false;
  
  private destroy$ = new Subject<void>();
  private typing$ = new Subject<void>();

  constructor(
    private chatService: ChatService,
    private route: ActivatedRoute
  ) {
    this.threadId = parseInt(this.route.snapshot.paramMap.get("threadId") || "0");
  }

  ngOnInit(): void {
    this.chatService.connectionStatus$
      .pipe(takeUntil(this.destroy$))
      .subscribe(status => {
        this.isConnected = status === "connected";
        if (this.isConnected) {
          this.joinThread();
        }
      });

    this.chatService.messages$
      .pipe(takeUntil(this.destroy$))
      .subscribe(message => {
        if (message && message.threadId === this.threadId) {
          this.messages.push(message);
          this.scrollToBottom();
        }
      });

    this.chatService.typingUsers$
      .pipe(takeUntil(this.destroy$))
      .subscribe(users => {
        this.typingUsers = users;
      });

    // Debounce typing indicator
    this.typing$
      .pipe(
        debounceTime(3000),
        takeUntil(this.destroy$)
      )
      .subscribe(() => {
        this.chatService.notifyStoppedTyping(this.threadId);
      });
  }

  async joinThread(): Promise<void> {
    try {
      await this.chatService.joinThread(this.threadId);
      console.log("Joined thread:", this.threadId);
    } catch (error) {
      console.error("Error joining thread:", error);
    }
  }

  async sendMessage(): Promise<void> {
    if (!this.messageText.trim()) return;

    const request: SendMessageRequest = {
      content: this.messageText,
      type: 1 // MessageType.Text
    };

    try {
      await this.chatService.sendMessage(this.threadId, request);
      this.messageText = "";
      await this.chatService.notifyStoppedTyping(this.threadId);
    } catch (error) {
      console.error("Error sending message:", error);
    }
  }

  onMessageTextChange(): void {
    this.typing$.next();
    this.chatService.notifyTyping(this.threadId);
  }

  async markMessageAsRead(message: Message): Promise<void> {
    if (!message.isRead) {
      try {
        await this.chatService.markMessageAsRead(message.id, this.threadId);
      } catch (error) {
        console.error("Error marking message as read:", error);
      }
    }
  }

  private scrollToBottom(): void {
    setTimeout(() => {
      const messagesDiv = document.getElementById("messages-container");
      if (messagesDiv) {
        messagesDiv.scrollTop = messagesDiv.scrollHeight;
      }
    });
  }

  ngOnDestroy(): void {
    this.chatService.leaveThread(this.threadId);
    this.destroy$.next();
    this.destroy$.complete();
  }
}
```

**chat-thread.component.html**

```html
<div class="chat-container">
  <div class="chat-header">
    <h2>Chat</h2>
    <span *ngIf="isConnected" class="status connected">● Connected</span>
    <span *ngIf="!isConnected" class="status disconnected">● Disconnected</span>
  </div>

  <div id="messages-container" class="messages-list">
    <div *ngFor="let message of messages" class="message" [class.own]="isOwnMessage(message)">
      <div class="message-avatar">
        <img [src]="message.senderProfilePhoto" [alt]="message.senderName" />
      </div>
      <div class="message-content">
        <div class="message-header">
          <span class="sender-name">{{ message.senderName }}</span>
          <span class="timestamp">{{ message.createdAt | date: 'short' }}</span>
        </div>
        <p class="message-text">{{ message.content }}</p>
        <span *ngIf="message.isRead" class="read-receipt">✓✓</span>
      </div>
    </div>

    <div *ngIf="typingUsers.size > 0" class="typing-indicator">
      <span>{{ Array.from(typingUsers).join(', ') }} is typing</span>
      <div class="dots">
        <span></span><span></span><span></span>
      </div>
    </div>
  </div>

  <div class="message-input-container">
    <textarea
      [(ngModel)]="messageText"
      (input)="onMessageTextChange()"
      placeholder="Type a message..."
      [disabled]="!isConnected"
    ></textarea>
    <button (click)="sendMessage()" [disabled]="!messageText.trim() || !isConnected">
      Send
    </button>
  </div>
</div>
```

### 4. Component Example: Notifications

**notifications-panel.component.ts**

```typescript
import { Component, OnInit, OnDestroy } from "@angular/core";
import { Subject } from "rxjs";
import { takeUntil } from "rxjs/operators";
import { NotificationService, Notification } from "../services/notification.service";

@Component({
  selector: "app-notifications-panel",
  templateUrl: "./notifications-panel.component.html",
  styleUrls: ["./notifications-panel.component.scss"]
})
export class NotificationsPanelComponent implements OnInit, OnDestroy {
  notifications: Notification[] = [];
  unreadCount: number = 0;
  isConnected: boolean = false;
  isOpen: boolean = false;

  private destroy$ = new Subject<void>();

  constructor(private notificationService: NotificationService) {}

  ngOnInit(): void {
    this.notificationService.connectionStatus$
      .pipe(takeUntil(this.destroy$))
      .subscribe(status => {
        this.isConnected = status === "connected";
      });

    this.notificationService.notifications$
      .pipe(takeUntil(this.destroy$))
      .subscribe(notifications => {
        this.notifications = notifications;
      });

    this.notificationService.unreadCount$
      .pipe(takeUntil(this.destroy$))
      .subscribe(count => {
        this.unreadCount = count;
      });
  }

  togglePanel(): void {
    this.isOpen = !this.isOpen;
  }

  async markAsRead(notification: Notification): Promise<void> {
    if (!notification.isRead) {
      try {
        await this.notificationService.markAsRead(notification.id);
      } catch (error) {
        console.error("Error marking notification as read:", error);
      }
    }
  }

  async markAllAsRead(): Promise<void> {
    try {
      await this.notificationService.markAllAsRead();
    } catch (error) {
      console.error("Error marking all as read:", error);
    }
  }

  getNotificationIcon(type: number): string {
    const icons: { [key: number]: string } = {
      1: "💬", // NewMessage
      2: "↩️", // MessageReply
      3: "✓", // SubscriptionConfirmed
      4: "⏰", // SubscriptionExpired
      5: "📦", // PackageUpdate
      6: "🏋️", // ProgramUpdate
      7: "⏲️", // WorkoutReminder
      8: "✅", // PaymentSuccessful
      9: "❌", // PaymentFailed
      10: "👍", // TrainerApproved
      11: "👎", // TrainerRejected
      12: "ℹ️" // SystemNotification
    };
    return icons[type] || "🔔";
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }
}
```

**notifications-panel.component.html**

```html
<div class="notifications-panel">
  <button class="notifications-button" (click)="togglePanel()">
    🔔
    <span *ngIf="unreadCount > 0" class="badge">{{ unreadCount }}</span>
  </button>

  <div *ngIf="isOpen" class="notifications-dropdown">
    <div class="notifications-header">
      <h3>Notifications</h3>
      <button *ngIf="unreadCount > 0" (click)="markAllAsRead()" class="mark-all-btn">
        Mark all as read
      </button>
    </div>

    <div *ngIf="notifications.length === 0" class="empty-state">
      No notifications
    </div>

    <div *ngFor="let notification of notifications" class="notification-item" [class.unread]="!notification.isRead">
      <span class="icon">{{ getNotificationIcon(notification.type) }}</span>
      <div class="notification-content">
        <h4>{{ notification.title }}</h4>
        <p>{{ notification.message }}</p>
        <small>{{ notification.createdAt | date: 'short' }}</small>
      </div>
      <button
        *ngIf="!notification.isRead"
        (click)="markAsRead(notification)"
        class="close-btn"
        title="Mark as read"
      >
        ×
      </button>
    </div>
  </div>
</div>
```

### 5. App Initialization

**app.component.ts**

```typescript
import { Component, OnInit } from "@angular/core";
import { ChatService } from "./services/chat.service";
import { NotificationService } from "./services/notification.service";

@Component({
  selector: "app-root",
  templateUrl: "./app.component.html",
  styleUrls: ["./app.component.scss"]
})
export class AppComponent implements OnInit {
  constructor(
    private chatService: ChatService,
    private notificationService: NotificationService
  ) {}

  async ngOnInit(): Promise<void> {
    // Connect to both hubs on app initialization
    await Promise.all([
      this.chatService.connect(),
      this.notificationService.connect()
    ]);
  }
}
```

### 6. Module Configuration

**app.module.ts**

```typescript
import { NgModule } from "@angular/core";
import { BrowserModule } from "@angular/platform-browser";
import { FormsModule } from "@angular/forms";
import { HttpClientModule } from "@angular/common/http";

import { AppComponent } from "./app.component";
import { ChatThreadComponent } from "./components/chat-thread/chat-thread.component";
import { NotificationsPanelComponent } from "./components/notifications-panel/notifications-panel.component";

import { ChatService } from "./services/chat.service";
import { NotificationService } from "./services/notification.service";
import { AuthService } from "./services/auth.service";

@NgModule({
  declarations: [
    AppComponent,
    ChatThreadComponent,
    NotificationsPanelComponent
  ],
  imports: [
    BrowserModule,
    FormsModule,
    HttpClientModule
  ],
  providers: [
    ChatService,
    NotificationService,
    AuthService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
```

## Testing in Development

### 1. Open Browser DevTools
- F12 or Right-click → Inspect

### 2. Check Network Tab
- Look for WebSocket connections:
  - `/hubs/chat`
  - `/hubs/notifications`

### 3. Check Console
- Look for connection logs
- Monitor errors

### 4. Test Endpoints
Open browser console and run:

```javascript
// Send test notification
fetch('https://localhost:5131/api/chat/threads', {
  method: 'GET',
  headers: {
    'Authorization': `Bearer ${localStorage.getItem('token')}`
  }
}).then(r => r.json()).then(console.log);
```

## Performance Tips

1. **Unsubscribe properly** - Always use `takeUntil` with destroy subject
2. **Debounce typing** - Don't send every keystroke
3. **Lazy load messages** - Implement pagination for old messages
4. **Cache** - Store recent chats locally
5. **Memory management** - Limit stored messages to latest 100

## Error Handling

Always wrap SignalR calls in try-catch:

```typescript
try {
  await this.chatService.sendMessage(threadId, request);
} catch (error) {
  console.error("Failed to send message:", error);
  // Show user-friendly error message
}
```

## References

- [Microsoft SignalR Client](https://docs.microsoft.com/en-us/javascript/api/@microsoft/signalr/)
- [Angular with SignalR](https://docs.microsoft.com/en-us/aspnet/core/signalr/java-client)
- [RxJS Operators](https://rxjs.dev/api)
