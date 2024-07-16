namespace API.Shared.DTOs.Notifications
{
    public class EmailNotificationDTO : SendEmailNotificationDTO
    {
        public int EmailStatusId { get; set; }
        public string EmailAddress { get; set; }
        public Guid ID { get; set; }
        public DateTime CreateDate { get; set; }

    }
}
