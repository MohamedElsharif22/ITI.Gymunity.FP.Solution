# Admin Portal Views Implementation Guide

## Directory Structure
```
ITI.Gymunity.FP.Admin.MVC/
├── Views/
│   ├── Shared/
│   │   └── _AdminLayout.cshtml (Master Layout)
│   ├── Dashboard/
│   │   └── Index.cshtml ✅
│   ├── Clients/
│   │   └── Index.cshtml ✅
│   ├── Trainers/
│   │   ├── Index.cshtml ✅
│   │   └── Details.cshtml ✅
│   ├── Reviews/
│   │   └── Index.cshtml ✅
│   ├── Subscriptions/
│   │   └── Index.cshtml ✅
│   ├── Payments/
│   │   └── Index.cshtml ✅
│   ├── Programs/
│   │   └── (To be implemented)
│   ├── Analytics/
│   │   └── (To be implemented)
│   ├── Settings/
│   │   └── (To be implemented)
│   └── Auth/
│       └── (Login/Logout pages)
└── Controllers/
    ├── BaseAdminController.cs
    ├── DashboardController.cs ✅
    ├── ClientsController.cs ✅
    ├── TrainersController.cs ✅
    ├── ReviewsController.cs ✅
    ├── SubscriptionsController.cs ✅
    ├── PaymentsController.cs ✅
    ├── ProgramsController.cs
    ├── AnalyticsController.cs
    ├── SettingsController.cs
    ├── UsersController.cs
    └── AuthController.cs
```

## View Details

### 1. Dashboard/Index.cshtml
**Purpose**: Main admin dashboard with overview statistics
- Quick stats cards (Users, Trainers, Subscriptions, Revenue)
- Recent activity feed
- Revenue chart
- User distribution chart
- Top performers list

**Controller**: DashboardController
**Route**: `/admin/dashboard` or `/dashboard`
**Layout**: _AdminLayout.cshtml

### 2. Clients/Index.cshtml
**Purpose**: Manage all client accounts in the system
- Paginated client list
- Search functionality
- Client information (Name, Email, Username, Join Date)
- Quick links to client details
- View details action

**Controller**: ClientsController
**Route**: `/admin/clients`
**Layout**: _AdminLayout.cshtml

**Features**:
- Search by name/email/username
- Pagination (10, 25, 50 items per page)
- Responsive table design
- Client info cards with avatars

### 3. Trainers/Index.cshtml
**Purpose**: Manage trainer profiles and verification
- Trainer list with pagination
- Filter by verification status
- Trainer information (Handle, Username, Rating, Total Clients)
- Verification management actions
- Status indicators

**Controller**: TrainersController
**Route**: `/admin/trainers`
**Layout**: _AdminLayout.cshtml

**Features**:
- Filter by verified/unverified status
- Search by name/email/handle
- Verify/Reject/Suspend actions
- Star rating display
- Pagination support

### 4. Trainers/Details.cshtml
**Purpose**: View detailed trainer profile information
- Basic information (Username, Handle, Status)
- Statistics (Clients, Rating, Experience)
- Professional information (Bio, Video URL)
- Training packages section
- Client reviews section

**Controller**: TrainersController
**Route**: `/admin/trainers/{id}`
**Layout**: _AdminLayout.cshtml

**Features**:
- Comprehensive trainer profile view
- Statistics cards
- Action buttons for status management
- Email contact option
- Review information display

### 5. Reviews/Index.cshtml
**Purpose**: Manage and moderate client reviews
- Pending review list
- Review content display
- Rating visualization
- Approval/rejection workflow
- Pagination

**Controller**: ReviewsController
**Route**: `/admin/reviews`
**Layout**: _AdminLayout.cshtml

**Features**:
- Automatic pending review detection
- Star rating display
- Comment preview
- Approve/Reject actions
- Average rating calculation
- Pagination support

### 6. Subscriptions/Index.cshtml
**Purpose**: Monitor and manage client subscriptions
- Subscription list with status
- Filtering by status (Active, Canceled, Unpaid)
- Subscription details (Client, Package, Amount, Dates)
- Cancellation management
- Quick view options

**Controller**: SubscriptionsController
**Route**: `/admin/subscriptions`
**Layout**: _AdminLayout.cshtml

**Features**:
- Multiple status filters
- Quick view shortcuts
- Expiration date tracking
- Active subscription count
- Total value calculation
- Pagination support

### 7. Payments/Index.cshtml
**Purpose**: Track and manage payment transactions
- Payment list with detailed information
- Revenue statistics
- Status filtering (Pending, Completed, Failed, Refunded)
- Date range filtering
- Refund management

**Controller**: PaymentsController
**Route**: `/admin/payments`
**Layout**: _AdminLayout.cshtml

**Features**:
- Revenue overview cards
- Multiple status filters
- Date range selection
- Payment method display
- Color-coded status badges
- Refund processing
- Pagination support

## Styling Architecture

### CSS Framework
- **TailwindCSS 3.x** (via CDN)
- Custom CSS in `~/css/admin.css`
- Responsive breakpoints (sm, md, lg)

### Color Palette
```
Primary:   Blue-600 (#2563EB)
Success:   Green-600 (#16A34A)
Warning:   Yellow/Amber-600 (#D97706)
Danger:    Red-600 (#DC2626)
Info:      Blue-500 (#3B82F6)
Secondary: Purple-600 (#7C3AED)
```

### Component Library
All views use consistent components:
- Header sections with icons
- Data tables with hover effects
- Filter/search forms
- Pagination controls
- Modal-ready structure
- Cards with shadows
- Button styles (primary, secondary, danger)
- Alert/notification boxes
- Status badges

## JavaScript Functionality

### Layout-Level Scripts
- Sidebar toggle for mobile
- Auto-close sidebar on link click
- Active link highlighting
- Current time display

### View-Level Scripts (in @section Scripts)
- Form submissions and validations
- Action confirmations
- AJAX calls for async operations
- Dynamic table updates

## Performance Considerations

1. **Pagination**: Views handle large datasets with pagination
2. **Lazy Loading**: Icons and images load asynchronously
3. **CSS Framework**: TailwindCSS via CDN (production ready)
4. **Responsive Design**: Mobile-first approach
5. **Accessibility**: Semantic HTML, ARIA labels where needed

## Notes for Implementation

1. All views inherit from `_AdminLayout.cshtml`
2. Each controller should set `ViewBag.BreadcrumbItems` for navigation
3. TempData messages (Success/Error/Warning) are automatically displayed
4. Active link detection happens automatically via JavaScript
5. Responsive design is fully implemented for all screen sizes

## Future Implementation Priorities

1. **Analytics/Index.cshtml** - Dashboard with charts and metrics
2. **Programs/Index.cshtml** - Training programs management
3. **Settings/Index.cshtml** - Admin configuration options
4. **Auth Views** - Login and forgot password pages
5. **Error Pages** - 404, 500 error pages with styling
