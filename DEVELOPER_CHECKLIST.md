# SignalR Implementation - Developer Checklist

## ✅ Pre-Implementation Setup

- [ ] Review `README_SIGNALR_IMPLEMENTATION.md` for overview
- [ ] Read `SIGNALR_IMPLEMENTATION_GUIDE.md` for technical details
- [ ] Understand clean architecture principles applied
- [ ] Verify .NET 9 SDK installed: `dotnet --version`
- [ ] Verify Entity Framework Core CLI tools: `dotnet ef --version`
- [ ] Have SQL Server access with correct connection string

## ✅ Backend Setup (ASP.NET Core)

### Code Generation (✓ ALREADY DONE)
- [x] Domain models created (Notification, NotificationType)
- [x] Application DTOs created
- [x] Service contracts created
- [x] Infrastructure services implemented
- [x] SignalR Hubs created (ChatHub, NotificationHub)
- [x] REST API Controllers created
- [x] Database configuration added
- [x] DI configuration updated
- [x] Program.cs updated with SignalR

### Build & Verification
- [ ] Run `dotnet build` - verify no compilation errors
- [ ] Check all references resolve correctly
- [ ] Verify no missing NuGet packages

### Database Setup
- [ ] Review `DATABASE_MIGRATION_GUIDE.md`
- [ ] Update `appsettings.json` with correct connection string
- [ ] Create migration: `Add-Migration AddSignalRNotifications`
- [ ] Review generated migration file
- [ ] Apply migration: `Update-Database`
- [ ] Verify tables created in SQL Server:
  - [ ] Notifications table exists
  - [ ] Proper columns and types
  - [ ] Indexes created correctly
  - [ ] Foreign key relationship to AspNetUsers

### Local Testing
- [ ] Start application: `dotnet run --project ITI.Gymunity.FP.APIs`
- [ ] Verify application starts without errors
- [ ] Check API is accessible: `https://localhost:5131/swagger`
- [ ] Test REST endpoints:
  - [ ] GET `/api/notifications` (should work with auth)
  - [ ] GET `/api/chat/threads` (should work with auth)

### SignalR Hub Testing
- [ ] Open browser DevTools (F12)
- [ ] Navigate to application
- [ ] Check Network tab for WebSocket connections:
  - [ ] `/hubs/chat` connection established
  - [ ] `/hubs/notifications` connection established
- [ ] Check Connection Status (should show "connected")
- [ ] Monitor console for any errors

## ✅ Frontend Setup (Angular)

### Environment Setup
- [ ] Node.js/npm installed: `node --version`
- [ ] Angular CLI installed: `ng version`
- [ ] Project exists and builds: `ng build`

### SignalR Package Installation
- [ ] Install SignalR: `npm install @microsoft/signalr`
- [ ] Verify installation: `npm list @microsoft/signalr`
- [ ] Check TypeScript types available

### Service Implementation
- [ ] Create `src/app/services/chat.service.ts`
  - [ ] Import SignalR
  - [ ] Define Message interface
  - [ ] Define SendMessageRequest interface
  - [ ] Implement HubConnection setup
  - [ ] Implement event listeners
  - [ ] Implement connection methods
  - [ ] Implement message methods
  - [ ] Add error handling and logging

- [ ] Create `src/app/services/notification.service.ts`
  - [ ] Import SignalR
  - [ ] Define Notification interface
  - [ ] Implement HubConnection setup
  - [ ] Implement event listeners
  - [ ] Implement notification methods
  - [ ] Add error handling and logging

- [ ] Create `src/app/services/auth.service.ts` (if not exists)
  - [ ] Implement JWT token management
  - [ ] Implement getToken() method
  - [ ] Implement logout cleanup

### Component Implementation
- [ ] Create `src/app/components/chat-thread.component.ts`
  - [ ] Inject ChatService
  - [ ] Bind to message$ observable
  - [ ] Implement sendMessage()
  - [ ] Implement typing indicators
  - [ ] Proper subscription cleanup (unsubscribe)

- [ ] Create `src/app/components/notifications-panel.component.ts`
  - [ ] Inject NotificationService
  - [ ] Bind to notifications$ observable
  - [ ] Bind to unreadCount$ observable
  - [ ] Implement mark as read
  - [ ] Proper subscription cleanup (unsubscribe)

- [ ] Create HTML templates with styling

### Application Initialization
- [ ] Update `app.component.ts`
  - [ ] Inject both services
  - [ ] Call connect() on both services on init
  - [ ] Handle connection errors gracefully

### Module Configuration
- [ ] Update `app.module.ts`
  - [ ] Import services
  - [ ] Add providers
  - [ ] Add necessary imports (FormsModule, etc.)

### Testing
- [ ] Build project: `ng build`
- [ ] Serve application: `ng serve`
- [ ] Open browser to `http://localhost:4200`
- [ ] Check for compilation errors
- [ ] Monitor console for warnings/errors

## ✅ Integration Testing

### Connection Tests
- [ ] Open DevTools Network tab
- [ ] Verify WebSocket connections to hubs
- [ ] Check connection status display
- [ ] Test reconnection (disconnect network briefly)

### Chat Functionality
- [ ] Open chat component
- [ ] Type message
- [ ] Send message via SignalR
- [ ] Message should appear in real-time
- [ ] Check message in database
- [ ] Test with two users (open in separate windows)
- [ ] Verify messages appear for both users
- [ ] Test typing indicators
- [ ] Test read receipts
- [ ] Test online/offline status

### Notification Functionality
- [ ] Create test notification via API:
  ```bash
  POST /api/notifications
  {
    "title": "Test",
    "message": "Test notification",
    "type": 1
  }
  ```
- [ ] Notification appears in real-time on client
- [ ] Unread count increases
- [ ] Mark as read works
- [ ] Mark all as read works
- [ ] Check notifications in database

### Error Handling
- [ ] Disconnect network while connected
- [ ] Verify automatic reconnection
- [ ] Test sending message while disconnected
- [ ] Verify error messages shown to user
- [ ] Check console for detailed error logs

## ✅ Code Quality Checks

### Backend
- [ ] Run code analysis: Check for warnings
- [ ] Verify logging is appropriate
- [ ] Check exception handling
- [ ] Verify no hardcoded values
- [ ] Check configuration uses appsettings
- [ ] Verify authentication/authorization on hubs

### Frontend
- [ ] Check for memory leaks (subscriptions)
- [ ] Verify error handling in UI
- [ ] Check console for warnings
- [ ] Verify TypeScript strict mode
- [ ] Check for proper component cleanup

## ✅ Production Readiness

### Security
- [ ] HTTPS enforced
- [ ] CORS properly configured (not wildcard in prod)
- [ ] JWT token validation working
- [ ] [Authorize] attribute on hubs
- [ ] User isolation verified
- [ ] No sensitive data in logs

### Performance
- [ ] Connection pooling working
- [ ] Queries are indexed (check DB)
- [ ] No N+1 queries
- [ ] Unread counts cached appropriately
- [ ] Connection limits considered

### Monitoring
- [ ] Logging configured
- [ ] Error tracking in place
- [ ] Performance metrics monitored
- [ ] SignalR connection metrics logged
- [ ] Database query performance monitored

### Deployment
- [ ] Connection string configured for target environment
- [ ] Database migrations automated in deployment
- [ ] Environment variables properly set
- [ ] CORS origins correct for production
- [ ] SSL certificates valid

## ✅ Documentation

- [ ] `README_SIGNALR_IMPLEMENTATION.md` - Reviewed ✓
- [ ] `SIGNALR_IMPLEMENTATION_GUIDE.md` - Reviewed ✓
- [ ] `DATABASE_MIGRATION_GUIDE.md` - Reviewed ✓
- [ ] `ANGULAR_CLIENT_EXAMPLES.md` - Reviewed ✓
- [ ] Code comments added for complex logic
- [ ] API documentation updated in Swagger
- [ ] Architecture decisions documented

## ✅ Testing & QA

### Unit Tests (Optional but Recommended)
- [ ] ChatService tests
- [ ] NotificationService tests
- [ ] SignalRConnectionManager tests
- [ ] Mock IUnitOfWork and UserManager
- [ ] Test success paths
- [ ] Test error paths

### Integration Tests (Optional)
- [ ] Test hub methods with test server
- [ ] Mock hub context
- [ ] Verify message flow
- [ ] Test connection management

### Manual Testing Scenarios

**Scenario 1: Basic Messaging**
1. [ ] User A logs in
2. [ ] User B logs in
3. [ ] User A and B join same thread
4. [ ] User A sends message
5. [ ] Message received by User B in real-time
6. [ ] Message appears in both UIs
7. [ ] Message saved to database

**Scenario 2: Read Receipts**
1. [ ] User A sends message to User B
2. [ ] Message marked as unread
3. [ ] User B marks message as read
4. [ ] User A sees read receipt
5. [ ] Database updated

**Scenario 3: Typing Indicators**
1. [ ] User A starts typing (no send)
2. [ ] User B sees "User A is typing"
3. [ ] User A stops typing (3 sec timeout)
4. [ ] User B sees indicator disappear

**Scenario 4: Notifications**
1. [ ] Notification created
2. [ ] Appears in real-time on client
3. [ ] Unread count increases
4. [ ] Mark as read updates UI
5. [ ] Mark all as read clears all

**Scenario 5: Connection Loss**
1. [ ] Connected to hub
2. [ ] Disconnect network (DevTools throttle)
3. [ ] UI shows "Reconnecting..."
4. [ ] Auto-reconnect after network returns
5. [ ] UI shows "Connected"
6. [ ] Can send messages again

**Scenario 6: Multiple Connections (Same User, Multiple Tabs)**
1. [ ] Open application in 2 browser tabs
2. [ ] Both tabs connect to hubs
3. [ ] Send message in Tab 1
4. [ ] Message appears in Tab 1 UI
5. [ ] Message appears in Tab 2 UI (via broadcast)
6. [ ] Both tabs can send/receive

## ✅ Deployment Steps

### Step 1: Prepare
- [ ] Create release branch
- [ ] Update version numbers
- [ ] Review all changes
- [ ] Run final build: `dotnet build`

### Step 2: Database
- [ ] Back up production database
- [ ] Create migration script
- [ ] Test migration on staging
- [ ] Execute migration on production

### Step 3: Backend Deployment
- [ ] Publish .NET application
- [ ] Update configuration for environment
- [ ] Restart application
- [ ] Verify API is accessible
- [ ] Check hubs are reachable

### Step 4: Frontend Deployment
- [ ] Build Angular: `ng build --prod`
- [ ] Deploy to static hosting
- [ ] Verify assets loaded correctly
- [ ] Check API calls go to correct URL

### Step 5: Post-Deployment
- [ ] Test end-to-end functionality
- [ ] Monitor logs for errors
- [ ] Test from multiple clients
- [ ] Verify performance is acceptable
- [ ] Document deployment steps for next time

## ✅ Common Issues & Solutions

### Issue: WebSocket Connection Failed
**Solutions:**
- [ ] Check CORS configuration
- [ ] Verify SSL certificate
- [ ] Check firewall rules
- [ ] Verify hub URL in client
- [ ] Check authentication token

### Issue: Messages Not Appearing
**Solutions:**
- [ ] Verify joined thread group
- [ ] Check connection status
- [ ] Verify method names match
- [ ] Check browser console for errors
- [ ] Check server logs

### Issue: Database Migration Failed
**Solutions:**
- [ ] Verify connection string
- [ ] Check SQL Server running
- [ ] Verify permissions
- [ ] Check no syntax errors in migration
- [ ] Try rollback and try again

### Issue: High Memory Usage
**Solutions:**
- [ ] Check subscription cleanup
- [ ] Monitor connection count
- [ ] Check for memory leaks
- [ ] Implement message pagination
- [ ] Add connection limits

## ✅ Performance Optimization (Phase 2)

- [ ] Implement message pagination
- [ ] Add Redis for scaling
- [ ] Implement message caching
- [ ] Optimize database indexes
- [ ] Add connection rate limiting
- [ ] Implement message compression
- [ ] Add monitoring dashboard

## ✅ Feature Enhancements (Future)

- [ ] Message search
- [ ] Media uploads
- [ ] Message reactions
- [ ] Thread categories
- [ ] Notification preferences
- [ ] Message encryption
- [ ] Voice/video calls (WebRTC)
- [ ] Mobile app (React Native)

## ✅ Sign-Off

- [ ] All items completed
- [ ] Testing passed
- [ ] Ready for production
- [ ] Documentation complete
- [ ] Support team trained

**Implementation Date:** _______________

**Implemented By:** _______________

**Reviewed By:** _______________

**Approved By:** _______________

## 📞 Quick Reference

### Key Files Created
```
SIGNALR_IMPLEMENTATION_GUIDE.md - Technical reference
DATABASE_MIGRATION_GUIDE.md - Database setup
ANGULAR_CLIENT_EXAMPLES.md - Frontend examples
README_SIGNALR_IMPLEMENTATION.md - Project summary
DEVELOPER_CHECKLIST.md - This file
```

### Important Endpoints
```
WebSocket Hubs:
  wss://localhost:5131/hubs/chat
  wss://localhost:5131/hubs/notifications

REST API:
  https://localhost:5131/api/chat
  https://localhost:5131/api/notifications
  https://localhost:5131/swagger (documentation)
```

### Emergency Contacts
- Backend: [Lead Developer]
- Frontend: [Frontend Lead]
- Database: [DBA]
- DevOps: [DevOps Engineer]

---

**Last Updated:** 2024
**Version:** 1.0
**Status:** Ready for Implementation
