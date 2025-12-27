namespace ITI.Gymunity.FP.Admin.MVC.ViewModels.Notifications
{
    public class NotificationsListViewModel
    {
        public IEnumerable<NotificationViewModel> Notifications { get; set; } = new List<NotificationViewModel>();
        public int UnreadCount { get; set; }
        public int TotalCount { get; set; }
    }
}
