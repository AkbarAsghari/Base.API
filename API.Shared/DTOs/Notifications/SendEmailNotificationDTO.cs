namespace API.Shared.DTOs.Notifications
{
    public class SendEmailNotificationDTO
    {
        public Guid ReciptUserId { get; set; }
        public int EmailTypeId { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public string Link { get; set; }
        public string LinkText { get; set; }
    }
}
