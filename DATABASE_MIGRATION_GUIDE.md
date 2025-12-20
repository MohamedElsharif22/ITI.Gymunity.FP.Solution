# SignalR Database Migration Guide

## Overview
This guide provides step-by-step instructions to create and apply the database migration for SignalR tables (Notifications).

## Prerequisites
- .NET 9 SDK installed
- Entity Framework Core tools installed
- Access to SQL Server instance configured in `appsettings.json`
- All code changes from the SignalR implementation completed

## Step-by-Step Migration Instructions

### Step 1: Open Package Manager Console
1. In Visual Studio, go to **Tools** → **NuGet Package Manager** → **Package Manager Console**
2. Ensure the Default project is set to `ITI.Gymunity.FP.Infrastructure`

### Step 2: Create the Migration

Run the following command in the Package Manager Console:

```powershell
Add-Migration AddSignalRNotifications -p ITI.Gymunity.FP.Infrastructure -StartupProject ITI.Gymunity.FP.APIs
```

**Or** if using .NET CLI in the project root directory:

```bash
dotnet ef migrations add AddSignalRNotifications -p ITI.Gymunity.FP.Infrastructure -s ITI.Gymunity.FP.APIs
```

### Step 3: Review the Migration

The migration file will be created in `ITI.Gymunity.FP.Infrastructure/Migrations/` directory. Open it to verify:

**Expected migration file content:**
```csharp
public partial class AddSignalRNotifications : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Notifications",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                UserId = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                Message = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                Type = table.Column<int>(type: "int", nullable: false),
                RelatedEntityId = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                IsRead = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Notifications", x => x.Id);
                table.ForeignKey(
                    name: "FK_Notifications_AspNetUsers_UserId",
                    column: x => x.UserId,
                    principalTable: "AspNetUsers",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_Notifications_UserId",
            table: "Notifications",
            column: "UserId");

        migrationBuilder.CreateIndex(
            name: "IX_Notifications_UserId_IsRead",
            table: "Notifications",
            columns: new[] { "UserId", "IsRead" });

        migrationBuilder.CreateIndex(
            name: "IX_Notifications_CreatedAt",
            table: "Notifications",
            column: "CreatedAt");

        migrationBuilder.CreateIndex(
            name: "IX_Notifications_Type",
            table: "Notifications",
            column: "Type");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "Notifications");
    }
}
```

### Step 4: Apply the Migration to Database

Run the following command in the Package Manager Console:

```powershell
Update-Database -p ITI.Gymunity.FP.Infrastructure -StartupProject ITI.Gymunity.FP.APIs
```

**Or** using .NET CLI:

```bash
dotnet ef database update -p ITI.Gymunity.FP.Infrastructure -s ITI.Gymunity.FP.APIs
```

### Step 5: Verify Migration Success

Check SQL Server to confirm the tables were created:

```sql
-- View Notifications table structure
SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Notifications'

-- View Notifications table columns
SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Notifications'

-- View Notifications table indexes
SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID('dbo.Notifications')
```

## Troubleshooting

### Issue: "No DbContext found"
**Solution:**
- Ensure ITI.Gymunity.FP.APIs is set as the startup project
- Verify connection string in `appsettings.json`

### Issue: "Automatic migration was not applied"
**Solution:**
- Manually run `Update-Database` command
- Check for SQL Server connectivity issues

### Issue: Migration already exists
**Solution:**
- If migration already created, just run `Update-Database`
- You can remove the last migration with: `Remove-Migration -p ITI.Gymunity.FP.Infrastructure`

### Issue: SQL Server connection failed
**Solution:**
- Verify connection string in `appsettings.json`
- Check SQL Server service is running
- Ensure firewall allows database access

## Verifying the Setup

### Test 1: Query Empty Notifications Table
```sql
SELECT * FROM Notifications
-- Should return empty result set
```

### Test 2: Insert Test Notification
```sql
-- Get a valid UserId first
DECLARE @UserId NVARCHAR(MAX) = (SELECT TOP 1 Id FROM AspNetUsers)

INSERT INTO Notifications (UserId, Title, Message, Type, CreatedAt, IsRead, IsDeleted, UpdatedAt)
VALUES (
    @UserId,
    'Test Notification',
    'This is a test notification',
    1,
    GETUTCDATE(),
    0,
    0,
    NULL
)

-- Verify insertion
SELECT * FROM Notifications WHERE Title = 'Test Notification'
```

### Test 3: Check Indexes
```sql
SELECT 
    name AS IndexName,
    type_desc AS IndexType
FROM sys.indexes
WHERE object_id = OBJECT_ID('dbo.Notifications')
AND name IS NOT NULL
```

## Next Steps After Migration

1. **Start the application:**
   ```bash
   dotnet run --project ITI.Gymunity.FP.APIs
   ```

2. **Test SignalR connectivity:**
   - Open browser developer tools
   - Navigate to application
   - Check Network tab for WebSocket connections to `/hubs/chat` and `/hubs/notifications`

3. **Test API endpoints:**
   ```bash
   # Get notifications (requires auth)
   GET /api/notifications

   # Get unread count
   GET /api/notifications/unread-count

   # Mark as read
   PUT /api/notifications/{id}/read

   # Mark all as read
   PUT /api/notifications/read-all
   ```

4. **Monitor logs:**
   - Check Application Insights or console output
   - Look for SignalR connection logs

## Rollback Migration (if needed)

If you need to revert the migration:

### Using Package Manager Console:
```powershell
Update-Database -Migration <PreviousMigrationName> -p ITI.Gymunity.FP.Infrastructure
Remove-Migration -p ITI.Gymunity.FP.Infrastructure
```

### Using .NET CLI:
```bash
dotnet ef database update <PreviousMigrationName> -p ITI.Gymunity.FP.Infrastructure
dotnet ef migrations remove -p ITI.Gymunity.FP.Infrastructure
```

## Migration Naming Convention

The migration follows the naming convention:
- **Name:** `AddSignalRNotifications`
- **Location:** `ITI.Gymunity.FP.Infrastructure/Migrations/`
- **Timestamp:** Automatic (added by EF Core)

To list all migrations:
```powershell
Get-Migration -p ITI.Gymunity.FP.Infrastructure
```

## Database Schema

### Notifications Table

| Column | Type | Nullable | Default | Description |
|--------|------|----------|---------|-------------|
| Id | int | NO | IDENTITY | Primary key |
| UserId | nvarchar(256) | NO | | Foreign key to AspNetUsers |
| Title | nvarchar(200) | NO | | Notification title |
| Message | nvarchar(2000) | NO | | Notification message body |
| Type | int | NO | | NotificationType enum value |
| RelatedEntityId | nvarchar(256) | YES | NULL | Optional reference to related entity |
| CreatedAt | datetimeoffset | NO | | Creation timestamp |
| IsRead | bit | NO | 0 | Read status flag |
| IsDeleted | bit | NO | 0 | Soft delete flag |
| UpdatedAt | datetimeoffset | YES | NULL | Last update timestamp |

### Indexes

1. **IX_Notifications_UserId** - Single column on UserId
2. **IX_Notifications_UserId_IsRead** - Composite index for unread queries
3. **IX_Notifications_CreatedAt** - For sorting by date
4. **IX_Notifications_Type** - For filtering by notification type

## Performance Considerations

### Recommended Indexes (Optional, for very high volume)

If expecting millions of notifications:

```sql
-- Archive old notifications (move to archive table)
CREATE TABLE NotificationsArchive (
    LIKE Notifications INCLUDING ALL
);

CREATE INDEX IX_NotificationsArchive_UserId_CreatedAt 
ON NotificationsArchive(UserId, CreatedAt DESC);

-- Add archival job in SQL Agent to move old notifications
```

### Query Optimization Tips

```sql
-- Good: Uses index on (UserId, IsRead)
SELECT * FROM Notifications 
WHERE UserId = @UserId AND IsRead = 0
ORDER BY CreatedAt DESC;

-- Less efficient: Full table scan
SELECT * FROM Notifications 
WHERE CONVERT(DATE, CreatedAt) = @Date;
```

## Documentation References

- [Entity Framework Core Migrations](https://docs.microsoft.com/en-us/ef/core/managing-schemas/migrations/)
- [Package Manager Console (PMC)](https://docs.microsoft.com/en-us/nuget/reference/package-manager-console)
- [SQL Server Indexes](https://docs.microsoft.com/en-us/sql/relational-databases/indexes/indexes)
