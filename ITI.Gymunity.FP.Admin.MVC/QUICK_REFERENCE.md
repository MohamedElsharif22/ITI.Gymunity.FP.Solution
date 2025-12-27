# Admin Portal - Quick Reference Card

## Navigation URLs

| Feature | URL Pattern | Status |
|---------|-----------|--------|
| **Dashboard** | `/dashboard` | ✅ Ready |
| **Clients Management** | `/clients` | ✅ Ready |
| **Trainers List** | `/trainers` | ✅ Ready |
| **Trainer Details** | `/trainers/{id}` | ✅ Ready |
| **Reviews Management** | `/reviews` | ✅ Ready |
| **Subscriptions** | `/subscriptions` | ✅ Ready |
| **Payments** | `/payments` | ✅ Ready |
| **Programs** | `/programs` | ⏳ Ready for implementation |
| **Analytics** | `/analytics` | ⏳ Ready for implementation |
| **Settings** | `/settings` | ⏳ Ready for implementation |

---

## View Files Location

```
ITI.Gymunity.FP.Admin.MVC/Views/

Shared/
  └── _AdminLayout.cshtml              ✅ Master layout

Clients/
  └── Index.cshtml                     ✅ Client list

Trainers/
  ├── Index.cshtml                     ✅ Trainer list
  └── Details.cshtml                   ✅ Trainer profile

Reviews/
  └── Index.cshtml                     ✅ Review management

Subscriptions/
  └── Index.cshtml                     ✅ Subscription list

Payments/
  └── Index.cshtml                     ✅ Payment list
```

---

## Controller Methods

### ClientsController
```csharp
Index()                    // GET /Clients/Index - Client list
```

### TrainersController
```csharp
Index()                    // GET /Trainers/Index - Trainer list
Details(int id)           // GET /Trainers/Details/{id} - Profile
Verify(int id)            // POST - Verify trainer
Reject(int id)            // POST - Reject trainer
Suspend(int id)           // POST - Suspend trainer
```

### ReviewsController
```csharp
Index()                    // GET /Reviews/Index - Review list
Approve(int id)           // POST - Approve review
Reject(int id)            // POST - Reject review
```

### SubscriptionsController
```csharp
Index()                    // GET /Subscriptions/Index - Sub list
Cancel(int id, string reason) // POST - Cancel subscription
```

### PaymentsController
```csharp
Index()                    // GET /Payments/Index - Payment list
Refund(int id)            // POST - Process refund
```

---

## CSS Classes Used

### Layout
```html
<nav id="sidebar">          <!-- Main sidebar -->
<div class="sidebar-item">  <!-- Menu item -->
<a class="nav-link">        <!-- Navigation link -->
<div class="page-content">  <!-- Main content area -->
<nav class="breadcrumb">    <!-- Breadcrumb nav -->
```

### Styling Utilities
```css
/* Colors */
.text-blue-600       /* Primary text */
.bg-blue-50          /* Light background */
.border-blue-600     /* Primary border */
.text-red-600        /* Danger text */
.text-green-600      /* Success text */

/* Spacing */
.px-4, .py-3         /* Padding */
.gap-3               /* Grid gap */
.w-5, .h-5           /* Icon sizes */

/* Effects */
.rounded-lg          /* Rounded corners */
.shadow-sm           /* Shadow effect */
.transition          /* Smooth transition */
.hover:bg-blue-50    /* Hover effects */
```

---

## JavaScript Functions

### In Layout (_AdminLayout.cshtml)
```javascript
// Sidebar toggle for mobile
document.getElementById('sidebarToggle')?.addEventListener('click', ...)

// Close sidebar on link click
document.querySelectorAll('.nav-link').forEach(link => { ... })

// Update current time
updateTime()                  // Runs every 1 second
setInterval(updateTime, 1000)

// Mark active navigation link
setActiveNavLink()           // Runs on page load
```

### Common in Views
```javascript
// Confirmation before action
if (confirm('Are you sure?')) {
    // Submit form or AJAX call
}

// AJAX calls ready to use
fetch('/controller/action', {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' }
})
```

---

## ViewModels Used

### ClientsListViewModel
```csharp
IEnumerable<ClientDto> Clients
int CurrentPage
int PageSize
int TotalCount
string SearchTerm
```

### TrainersListViewModel
```csharp
IEnumerable<TrainerListDto> Trainers
int CurrentPage
int PageSize
int TotalCount
string VerificationStatus  // "Verified" or "Pending"
string SearchTerm
```

### ReviewsListViewModel
```csharp
IEnumerable<ReviewDto> Reviews
int CurrentPage
int PageSize
int TotalCount
int PendingCount
decimal AverageRating
```

### SubscriptionsListViewModel
```csharp
IEnumerable<SubscriptionDto> Subscriptions
int CurrentPage
int PageSize
int TotalCount
int ActiveCount
decimal TotalValue
```

### PaymentsListViewModel
```csharp
IEnumerable<PaymentDto> Payments
int CurrentPage
int PageSize
int TotalCount
decimal TotalRevenue
string StatusFilter  // "All", "Completed", "Failed", etc.
DateTime? FromDate
DateTime? ToDate
```

---

## TempData Messages

### Success Messages
```csharp
TempData["SuccessMessage"] = "Operation completed successfully!";
```

### Error Messages
```csharp
TempData["ErrorMessage"] = "An error occurred!";
```

### Warning Messages
```csharp
TempData["WarningMessage"] = "Please review your action!";
```

---

## Icon References (Font Awesome 6.4)

```html
<!-- Dashboard -->
<i class="fas fa-chart-line"></i>      Dashboard
<i class="fas fa-chart-bar"></i>       Analytics

<!-- Users -->
<i class="fas fa-users"></i>           Clients/Users
<i class="fas fa-dumbbell"></i>        Trainers

<!-- Content -->
<i class="fas fa-star"></i>            Reviews
<i class="fas fa-list-check"></i>      Programs

<!-- Business -->
<i class="fas fa-credit-card"></i>     Subscriptions
<i class="fas fa-money-bill-wave"></i> Payments

<!-- System -->
<i class="fas fa-cog"></i>             Settings
<i class="fas fa-sign-out-alt"></i>    Logout

<!-- Actions -->
<i class="fas fa-check-circle"></i>    Approve
<i class="fas fa-times"></i>           Reject/Close
<i class="fas fa-bell"></i>            Notifications
<i class="fas fa-user"></i>            Profile
```

---

## Responsive Breakpoints

```css
/* TailwindCSS Breakpoints */
sm     640px       Tablets
md     768px       Large tablets
lg     1024px      Desktops
xl     1280px      Wide desktops
2xl    1536px      Ultra-wide

/* Usage Examples */
.hidden.lg:block   /* Hidden on mobile, visible on desktop */
.w-full.md:w-1/2   /* Full width on mobile, half on tablet+ */
.text-sm.md:text-base  /* Smaller text on mobile */
```

---

## Common Patterns

### Pagination
```html
<div class="flex items-center justify-between">
    <div>Showing X to Y of Z items</div>
    <div class="flex gap-2">
        <button>Previous</button>
        <button class="active">1</button>
        <button>2</button>
        <button>Next</button>
    </div>
</div>
```

### Status Badge
```html
<span class="px-2 py-1 text-xs font-semibold bg-green-500 text-white rounded-full">
    Active
</span>
```

### Alert Message
```html
<div class="p-4 bg-green-50 border border-green-200 rounded-lg text-green-700">
    <i class="fas fa-check-circle"></i>
    <span>Success message</span>
    <button onclick="this.parentElement.remove()">
        <i class="fas fa-times"></i>
    </button>
</div>
```

### Data Table Row
```html
<tr class="hover:bg-gray-50">
    <td class="px-4 py-3">Value 1</td>
    <td class="px-4 py-3">Value 2</td>
    <td class="px-4 py-3 text-right">
        <a href="#">View</a>
        <a href="#">Edit</a>
    </td>
</tr>
```

---

## Build & Deployment

### Local Development
```bash
cd ITI.Gymunity.FP.Admin.MVC
dotnet restore
dotnet build      # Build solution
dotnet run        # Run locally
```

### Production Build
```bash
dotnet publish -c Release
# Output in: bin/Release/net9.0/publish/
```

---

## Performance Tips

1. **Pagination**: Always use pagination for large datasets
2. **Search**: Implement server-side search filtering
3. **Lazy Loading**: Load images asynchronously
4. **Caching**: Cache frequently accessed data
5. **CDN**: TailwindCSS and Font Awesome are served via CDN
6. **Minification**: Production build automatically minifies

---

## Browser Compatibility

- Chrome 90+
- Firefox 88+
- Safari 14+
- Edge 90+

*Note: All modern browsers with ES6 support*

---

## Files Quick Links

| File | Purpose |
|------|---------|
| `_AdminLayout.cshtml` | Master layout template |
| `admin.css` | Custom admin styles |
| `admin.js` | Layout interactivity |
| `NAVIGATION_STRUCTURE.md` | Navigation details |
| `VIEWS_IMPLEMENTATION.md` | View specifications |
| `VISUAL_LAYOUT_GUIDE.md` | UI/UX guide |
| `IMPLEMENTATION_SUMMARY.md` | Complete overview |

---

## Support Resources

- **TailwindCSS Docs**: https://tailwindcss.com/docs
- **Font Awesome**: https://fontawesome.com/docs/web
- **ASP.NET Core**: https://docs.microsoft.com/aspnet/core
- **Razor Syntax**: https://docs.microsoft.com/aspnet/web-pages/overview/getting-started/introducing-razor-syntax

---

## Version Info

- **.NET Version**: 9.0
- **TailwindCSS**: 3.x (CDN)
- **Font Awesome**: 6.4
- **ASP.NET Core MVC**: Latest in .NET 9
- **Build Status**: ✅ Successful

---

*Quick Reference Card - Admin Portal v1.0*
