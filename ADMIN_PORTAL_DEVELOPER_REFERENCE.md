# Admin Portal Developer Quick Reference

## 🚀 Quick Start Guide

### Accessing the Admin Portal
All admin features are accessible via `/admin/` route with `Admin` role authorization.

### Common Patterns Used

#### 1. Controller Action Pattern
```csharp
[HttpGet("resource")]
public async Task<IActionResult> Index(int pageNumber = 1, int pageSize = 10)
{
    try
    {
        SetPageTitle("Page Title");
        SetBreadcrumbs("Dashboard", "Section");
        
        var specs = new ResourceFilterSpecs(pageNumber: pageNumber, pageSize: pageSize);
        var resources = await _service.GetAllAsync(specs);
        var totalCount = await _service.GetCountAsync(specs);
        
        var model = new ResourceListViewModel
        {
            Resources = resources.ToList(),
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalCount = totalCount
        };
        
        return View(model);
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Error loading resources");
        ShowErrorMessage("Error message");
        return RedirectToDashboard();
    }
}
```

#### 2. Service Method Pattern
```csharp
public async Task<IEnumerable<ResponseDTO>> GetAllAsync(FilterSpecs specs)
{
    try
    {
        var entities = await _unitOfWork
            .Repository<Entity>()
            .GetAllWithSpecsAsync(specs);
        
        return _mapper.Map<IEnumerable<ResponseDTO>>(entities);
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Error message");
        throw;
    }
}
```

#### 3. Specification Pattern
```csharp
public class ResourceFilterSpecs : BaseSpecification<Entity>
{
    public ResourceFilterSpecs(
        bool? filter1 = null,
        int pageNumber = 1,
        int pageSize = 10,
        string? searchTerm = null)
    {
        // Set criteria
        if (filter1.HasValue)
        {
            Criteria = e => e.Property == filter1.Value;
        }
        
        // Add includes
        AddInclude(e => e.RelatedEntity);
        
        // Add ordering
        AddOrderByDesc(e => e.CreatedAt);
        
        // Apply pagination
        ApplyPagination((pageNumber - 1) * pageSize, pageSize);
    }
}
```

---

## 📊 API Endpoints Overview

### Trainers
```
GET    /admin/trainers                    - List all trainers
GET    /admin/trainers/{id}               - Get trainer details
GET    /admin/trainers/pending            - List pending trainers
GET    /admin/trainers/search?q=term      - Search trainers
POST   /admin/trainers/{id}/verify        - Verify trainer
POST   /admin/trainers/{id}/reject        - Reject trainer
POST   /admin/trainers/{id}/suspend       - Suspend trainer
POST   /admin/trainers/{id}/reactivate    - Reactivate trainer
GET    /admin/trainers/list-json          - DataTable AJAX endpoint
```

### Clients
```
GET    /admin/clients                     - List all clients
GET    /admin/clients/{id}                - Get client details
GET    /admin/clients/active              - List active clients
GET    /admin/clients/suspended           - List suspended clients
GET    /admin/clients/search?q=term       - Search clients
POST   /admin/clients/{id}/suspend        - Suspend client
POST   /admin/clients/{id}/reactivate     - Reactivate client
GET    /admin/clients/list-json           - DataTable AJAX endpoint
```

### Payments
```
GET    /admin/payments                    - List all payments
GET    /admin/payments/{id}               - Get payment details
GET    /admin/payments/failed             - List failed payments
GET    /admin/payments/completed          - List completed payments
GET    /admin/payments/revenue            - Revenue for date range
GET    /admin/payments/total-revenue      - Total revenue
GET    /admin/payments/failed-count       - Failed payment count
POST   /admin/payments/{id}/refund        - Process refund
GET    /admin/payments/list-json          - DataTable AJAX endpoint
```

### Subscriptions
```
GET    /admin/subscriptions               - List all subscriptions
GET    /admin/subscriptions/{id}          - Get subscription details
GET    /admin/subscriptions/active        - List active subscriptions
GET    /admin/subscriptions/inactive      - List inactive subscriptions
GET    /admin/subscriptions/unpaid        - List unpaid subscriptions
GET    /admin/subscriptions/expiring-soon - Subscriptions expiring soon
POST   /admin/subscriptions/{id}/cancel   - Cancel subscription
GET    /admin/subscriptions/list-json     - DataTable AJAX endpoint
```

### Reviews
```
GET    /admin/reviews                     - List reviews
GET    /admin/reviews/pending             - List pending reviews
POST   /admin/reviews/{id}/approve        - Approve review
POST   /admin/reviews/{id}/reject         - Reject review
DELETE /admin/reviews/{id}                - Delete review
GET    /admin/reviews/pending-json        - DataTable AJAX endpoint
```

---

## 🔧 Service Injection in Controllers

### Example: TrainersController
```csharp
[Authorize(Roles = "Admin")]
[Route("admin")]
public class TrainersController : BaseAdminController
{
    private readonly ILogger<TrainersController> _logger;
    private readonly TrainerAdminService _trainerService;

    public TrainersController(
        ILogger<TrainersController> logger,
        TrainerAdminService trainerService)
    {
        _logger = logger;
        _trainerService = trainerService;
    }
    
    // ... controller actions
}
```

### Dependency Injection Already Configured
```csharp
// In DependancyInjection.cs
services.AddScoped<TrainerAdminService>();
services.AddScoped<ClientAdminService>();
services.AddScoped<PaymentAdminService>();
services.AddScoped<SubscriptionAdminService>();
```

---

## 🎨 ViewModel Usage

### Example: TrainersListViewModel
```csharp
public class TrainersListViewModel
{
    public List<TrainerProfileDetailResponse> Trainers { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public int TotalCount { get; set; } = 0;
    public string? SearchTerm { get; set; }
    public bool? IsVerifiedFilter { get; set; }

    public int TotalPages => (TotalCount + PageSize - 1) / PageSize;
    public bool HasNextPage => PageNumber < TotalPages;
    public bool HasPreviousPage => PageNumber > 1;
}
```

---

## 📝 Creating a New Admin Feature

### Step 1: Create Specification
```csharp
// In Application/Specefications/Admin/
public class NewResourceFilterSpecs : BaseSpecification<NewEntity>
{
    public NewResourceFilterSpecs(
        bool? filter = null,
        int pageNumber = 1,
        int pageSize = 10)
    {
        if (filter.HasValue)
            Criteria = e => e.Property == filter.Value;
        
        AddInclude(e => e.Related);
        AddOrderByDesc(e => e.CreatedAt);
        ApplyPagination((pageNumber - 1) * pageSize, pageSize);
    }
}
```

### Step 2: Create Service
```csharp
// In Application/Services/Admin/
public class NewResourceAdminService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<NewResourceAdminService> _logger;
    
    public async Task<IEnumerable<ResponseDTO>> GetAllAsync(NewResourceFilterSpecs specs)
    {
        var entities = await _unitOfWork
            .Repository<NewEntity>()
            .GetAllWithSpecsAsync(specs);
        return _mapper.Map<IEnumerable<ResponseDTO>>(entities);
    }
}
```

### Step 3: Register in DependencyInjection
```csharp
services.AddScoped<NewResourceAdminService>();
```

### Step 4: Create Controller
```csharp
// In Admin.MVC/Controllers/
[Authorize(Roles = "Admin")]
[Route("admin")]
public class NewResourceController : BaseAdminController
{
    private readonly ILogger<NewResourceController> _logger;
    private readonly NewResourceAdminService _service;
    
    public NewResourceController(
        ILogger<NewResourceController> logger,
        NewResourceAdminService service)
    {
        _logger = logger;
        _service = service;
    }
    
    [HttpGet("new-resource")]
    public async Task<IActionResult> Index(int pageNumber = 1, int pageSize = 10)
    {
        // Implementation
    }
}
```

### Step 5: Create ViewModel
```csharp
// In Admin.MVC/ViewModels/NewResource/
public class NewResourceListViewModel
{
    public List<ResponseDTO> Resources { get; set; } = new();
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public int TotalCount { get; set; } = 0;
    
    public int TotalPages => (TotalCount + PageSize - 1) / PageSize;
}
```

### Step 6: Create Views
- `Views/NewResource/Index.cshtml`
- `Views/NewResource/Details.cshtml`

---

## 🐛 Debugging Tips

### Enable Detailed Logging
```csharp
// In appsettings.json
{
  "Logging": {
    "LogLevel": {
      "ITI.Gymunity.FP.Application.Services.Admin": "Debug",
      "ITI.Gymunity.FP.Admin.MVC.Controllers": "Debug"
    }
  }
}
```

### Common Issues & Solutions

#### Issue: "Authorization: No matching credentials found"
**Solution**: Ensure user has Admin role via `UserManager.AddToRoleAsync(user, "Admin")`

#### Issue: "The specified type member 'Property' is not supported in LINQ to Entities"
**Solution**: Use `.Compile().Invoke()` for complex criteria or move filtering to LINQ-to-Objects with `.ToList()` first

#### Issue: Navigation properties are null
**Solution**: Ensure `AddInclude()` is called in specification for related entities

#### Issue: Pagination not working
**Solution**: Verify `ApplyPagination((pageNumber - 1) * pageSize, pageSize)` is called with correct values

---

## 📈 Performance Optimization

### Use Specifications to Reduce Database Calls
✅ **Good**: Include related entities in specification
```csharp
AddInclude(t => t.User);
AddInclude(t => t.Trainer);
```

❌ **Bad**: Loading entities separately and joining in memory
```csharp
var trainers = await _unitOfWork.Repository<TrainerProfile>().GetAllAsync();
var users = await _unitOfWork.Repository<AppUser>().GetAllAsync();
var result = trainers.Join(users, ...); // N+1 problem
```

### Use Pagination for Large Result Sets
```csharp
ApplyPagination((pageNumber - 1) * pageSize, pageSize);
```

### Filter at Database Level (Not in Memory)
```csharp
// ✅ Good - Filter in specification
Criteria = e => e.Status == status.Value;

// ❌ Bad - Filter after loading all
var all = await repo.GetAllAsync();
var filtered = all.Where(e => e.Status == status).ToList();
```

---

## 🧪 Testing Pattern

### Example Unit Test
```csharp
[Fact]
public async Task GetAllTrainersAsync_WithVerifiedFilter_ReturnsOnlyVerifiedTrainers()
{
    // Arrange
    var specs = new TrainerFilterSpecs(isVerified: true);
    var mockRepository = new Mock<IRepository<TrainerProfile>>();
    var service = new TrainerAdminService(mockRepository.Object, _mapper, _logger);
    
    // Act
    var result = await service.GetAllTrainersAsync(specs);
    
    // Assert
    mockRepository.Verify(
        r => r.GetAllWithSpecsAsync(It.IsAny<TrainerFilterSpecs>()),
        Times.Once);
}
```

---

## 📚 Related Documentation

- [Implementation Plan](./ADMIN_DASHBOARD_IMPLEMENTATION_PLAN.md)
- [Implementation Summary](./ADMIN_PORTAL_IMPLEMENTATION_SUMMARY.md)
- [Clean Architecture Principles](https://docs.microsoft.com/en-us/dotnet/architecture/clean-code/)
- [Specification Pattern](https://en.wikipedia.org/wiki/Specification_pattern)
- [Repository Pattern](https://docs.microsoft.com/en-us/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/infrastructure-persistence-layer-design)

---

## 🔗 Important Files

**Specifications**: `ITI.Gymunity.FP.Application/Specefications/Admin/`
**Services**: `ITI.Gymunity.FP.Application/Services/Admin/`
**Controllers**: `ITI.Gymunity.FP.Admin.MVC/Controllers/`
**ViewModels**: `ITI.Gymunity.FP.Admin.MVC/ViewModels/`
**Configuration**: `ITI.Gymunity.FP.Application/Dependancy Injection/DependancyInjection.cs`

---

## ✅ Checklist for Completing Admin Portal

- [ ] Create Razor Views for all controllers
- [ ] Add navigation menu items to main layout
- [ ] Implement DataTable integration for list pages
- [ ] Add confirmation dialogs for delete/suspend actions
- [ ] Create admin dashboard to display key metrics
- [ ] Add export functionality (CSV/PDF) for reports
- [ ] Implement audit logging for admin actions
- [ ] Create admin role and user seed data
- [ ] Write unit tests for services
- [ ] Write integration tests for controllers
- [ ] Deploy and test in production-like environment

---

**Last Updated**: Today
**Status**: ✅ Backend Implementation Complete
**Next Step**: Create Views & Frontend Implementation
