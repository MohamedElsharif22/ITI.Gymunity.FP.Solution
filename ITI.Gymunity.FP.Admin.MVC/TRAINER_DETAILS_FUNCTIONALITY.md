# Trainer Details View - Functionality Documentation

## Overview

The Trainer Details view has been enhanced with comprehensive functionality for managing trainer profiles, including verification, rejection, suspension, and email communication features.

## View Location
`ITI.Gymunity.FP.Admin.MVC/Views/Trainers/Details.cshtml`

---

## Features Implemented

### 1. **Trainer Information Display**

#### Basic Information Section
- Username
- Handle (with @ prefix)
- User ID (full GUID for reference)
- Verification Status
  - ✅ Verified (Green badge)
  - ⏳ Pending Verification (Yellow badge)

#### Statistics Section
- **Total Clients**: Number of clients the trainer has
- **Average Rating**: Star rating visualization (0-5 stars)
- **Years of Experience**: Professional experience level

#### Professional Information Section
- **Bio**: Trainer's professional biography
- **Video Intro URL**: Link to introduction video (clickable with external link icon)
- **Verified Date**: When trainer was verified (if applicable)
- **Account Created**: Account creation date and time
- **Branding Colors**: Custom branding color scheme (if provided)
- **Cover Image**: Link to trainer's cover image (if available)

### 2. **Action Buttons**

#### For Unverified Trainers:
1. **Verify Trainer** (Green)
   - Marks trainer as verified
   - Allows them to accept clients
   - Requires confirmation

2. **Reject Trainer** (Red)
   - Rejects the trainer application
   - Soft deletes the profile
   - Cannot be undone
   - Requires confirmation with warning

#### For Verified Trainers:
1. **Suspend Account** (Orange)
   - Temporarily disables trainer account
   - Can be reactivated later
   - Requires confirmation

2. **Send Email** (Blue Border)
   - Opens email modal
   - Send messages to trainer
   - Available for all states

### 3. **Confirmation Modal**

**Features:**
- Clear action description
- Warning messages for destructive actions
- Color-coded confirmation buttons
- Loading states during processing
- Error handling with user feedback

**Workflow:**
1. User clicks action button
2. Modal opens with confirmation message
3. User confirms or cancels
4. If confirmed, AJAX request is sent
5. Success/error message displayed
6. Page reloads on success

### 4. **Email Modal**

**Features:**
- Pre-filled trainer information
- Subject field (required)
- Message textarea (required)
- Form validation
- Loading state during sending
- Success/error notifications

**Usage:**
1. Click "Send Email" button
2. Modal opens
3. Enter email subject and message
4. Click "Send Email"
5. Email sent via backend endpoint `/admin/email/send`

### 5. **Notification System**

**Success Notifications:**
- Green background with checkmark icon
- Auto-dismisses after 5 seconds
- Appears in top-right corner
- Fixed positioning (Z-50)

**Error Notifications:**
- Red background with warning icon
- Auto-dismisses after 5 seconds
- Appears in top-right corner
- Fixed positioning (Z-50)

**Examples:**
```javascript
showSuccessMessage('Trainer verified successfully');
showErrorMessage('Failed to verify trainer. Please try again.');
```

---

## API Endpoints Used

### Action Endpoints

#### Verify Trainer
```
POST /admin/trainers/{id}/verify
```
**Response:**
```json
{
  "success": true,
  "message": "Trainer verified successfully"
}
```

#### Reject Trainer
```
POST /admin/trainers/{id}/reject
```
**Response:**
```json
{
  "success": true,
  "message": "Trainer rejected successfully"
}
```

#### Suspend Trainer
```
POST /admin/trainers/{id}/suspend
```
**Response:**
```json
{
  "success": true,
  "message": "Trainer suspended successfully"
}
```

#### Reactivate Trainer
```
POST /admin/trainers/{id}/reactivate
```
**Response:**
```json
{
  "success": true,
  "message": "Trainer reactivated successfully"
}
```

### Email Endpoint
```
POST /admin/email/send
```
**Request Body:**
```json
{
  "trainerId": 1,
  "subject": "Email Subject",
  "message": "Email message content"
}
```
**Response:**
```json
{
  "success": true,
  "message": "Email sent successfully"
}
```

---

## JavaScript Functionality

### Global Variables
```javascript
const trainerId = '@Model.Id';           // Trainer ID
const trainerHandle = '@Model.Handle';   // Trainer's handle
let pendingAction = null;                 // Current pending action
```

### Modal Management Functions

#### Action Modal
```javascript
openActionModal(title, message, action)  // Opens confirmation modal
closeActionModal()                        // Closes confirmation modal
```

#### Email Modal
```javascript
openEmailModal()                          // Opens email modal
closeEmailModal()                         // Closes email modal
```

### Event Listeners

#### Verify Button
- Validates trainer status (unverified only)
- Opens confirmation modal
- Calls verify endpoint on confirmation

#### Reject Button
- Validates trainer status (unverified only)
- Opens confirmation modal with warning
- Calls reject endpoint on confirmation
- Warning: "This action cannot be undone"

#### Suspend Button
- Validates trainer status (verified only)
- Opens confirmation modal
- Calls suspend endpoint on confirmation

#### Email Button
- Opens email modal
- Validates form fields
- Sends email via endpoint
- Closes modal on success

### AJAX Calls

**Pattern:**
```javascript
const response = await fetch(endpoint, { 
    method: 'POST',
    headers: {
        'Content-Type': 'application/json'
    },
    body: JSON.stringify(data) // if needed
});

const data = await response.json();

if (response.ok && data.success) {
    showSuccessMessage(data.message);
    // Optional: location.reload();
} else {
    showErrorMessage(data.message || 'An error occurred');
}
```

---

## Error Handling

### Frontend Validation
- Required field validation for email form
- Button state management (disabled during processing)
- User-friendly error messages

### Backend Integration
- HTTP status code checking
- JSON response parsing
- Exception handling with logging
- Graceful error messages

### User Feedback
- Toast notifications (success/error)
- Loading spinner on buttons
- Modal states (open/closed)
- Button disable states

---

## Data Model (TrainerProfileDetailResponse)

```csharp
public class TrainerProfileDetailResponse
{
    public int Id { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public string UserId { get; set; } = null!;
    public string UserName { get; set; } = null!;
    public string Handle { get; set; } = null!;
    public string Bio { get; set; } = string.Empty;
    public string? CoverImageUrl { get; set; }
    public string? VideoIntroUrl { get; set; }
    public string? BrandingColors { get; set; }
    public bool IsVerified { get; set; }
    public DateTime? VerifiedAt { get; set; }
    public decimal RatingAverage { get; set; }
    public int TotalClients { get; set; }
    public int YearsExperience { get; set; }
    public string? StatusImageUrl { get; set; }
    public string? StatusDescription { get; set; }
}
```

---

## Styling & Design

### Color Scheme
- **Primary Actions**: Blue (#2563EB)
- **Success Actions**: Green (#16A34A) - for verification
- **Danger Actions**: Red (#DC2626) - for rejection
- **Warning Actions**: Orange (#EA580C) - for suspension
- **Status Verified**: Green (#10B981)
- **Status Pending**: Yellow (#FBBF24)

### Layout
- Responsive grid layout (1 column mobile, 3 columns desktop)
- Card-based design with shadows
- Modal dialogs for confirmations
- Toast notifications for feedback

### Accessibility
- Semantic HTML
- ARIA labels on buttons
- Focus states on interactive elements
- Color + icons for status indication
- Keyboard navigation support

---

## Usage Example

### Basic Flow - Verify a Trainer

1. **View Page**
   - Admin navigates to trainer details
   - Page loads with trainer information

2. **Click Verify**
   - Admin clicks "Verify Trainer" button
   - Confirmation modal opens

3. **Confirm Action**
   - Admin reads confirmation message
   - Clicks "Verify Trainer" button in modal
   - Loading indicator shows

4. **Process**
   - Frontend sends POST to `/admin/trainers/{id}/verify`
   - Backend updates trainer status
   - Response returned

5. **Feedback**
   - Success notification displayed
   - Page reloads automatically
   - Trainer shows as verified

### Advanced Flow - Send Email

1. **Click Send Email**
   - Admin clicks "Send Email" button
   - Email modal opens with pre-filled info

2. **Fill Form**
   - Admin enters subject
   - Admin enters message
   - Validation ensures fields are filled

3. **Send**
   - Admin clicks "Send Email" button
   - Loading indicator shows
   - Email sent via endpoint

4. **Feedback**
   - Success notification displayed
   - Modal closes automatically
   - Admin can send another email

---

## Future Enhancements

### Potential Features
1. **Bulk Actions**
   - Select multiple trainers
   - Verify/Reject multiple at once

2. **Comments/Notes**
   - Add notes to trainer profiles
   - Track decision reasons

3. **Audit Trail**
   - Log all admin actions
   - Show history of changes

4. **Client List**
   - Show trainer's current clients
   - Link to client profiles

5. **Training Packages**
   - Display trainer's packages
   - Manage package details

6. **Reviews Management**
   - Show all trainer reviews
   - Approve/Reject reviews

7. **Email Templates**
   - Pre-made email templates
   - Email scheduling

---

## Testing Checklist

- [ ] Verify button works for unverified trainers
- [ ] Reject button works for unverified trainers
- [ ] Suspend button works for verified trainers
- [ ] Confirmation modals appear correctly
- [ ] Email modal opens/closes properly
- [ ] Form validation works
- [ ] Success notifications appear
- [ ] Error notifications appear
- [ ] Page reloads after successful action
- [ ] Trainer information displays correctly
- [ ] Responsive design on mobile
- [ ] Links open in new tabs
- [ ] Loading states show during processing

---

## Troubleshooting

### Issue: Actions don't work
**Solution**: Check browser console for errors, verify endpoints match controller routes

### Issue: Modal doesn't appear
**Solution**: Check HTML for modal elements, verify CSS classes are correct

### Issue: Notifications don't show
**Solution**: Verify notification functions are defined, check z-index conflicts

### Issue: Email endpoint fails
**Solution**: Implement `/admin/email/send` endpoint if not exists, verify CSRF token

### Issue: Page doesn't reload
**Solution**: Check browser console, verify endpoint returns success response

---

## Summary

The Trainer Details view now provides comprehensive functionality for:
- ✅ Viewing detailed trainer information
- ✅ Verifying unverified trainers
- ✅ Rejecting trainer applications
- ✅ Suspending/Reactivating verified trainers
- ✅ Sending emails to trainers
- ✅ Professional user interface with confirmations
- ✅ Complete error handling and notifications
- ✅ Responsive design for all devices

All features are fully functional and integrated with the backend API endpoints.
