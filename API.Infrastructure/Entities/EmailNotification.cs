using API.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Infrastructure.Entities
{
    public class EmailNotification : BaseEntity, ISoftDeleteEntity
    {
        public Guid UserId { get; set; }
        public int EmailTypeId { get; set; }
        public int EmailStatusId { get; set; }
        public string? Subject { get; set; }
        public string? Message { get; set; }
        public string? Link { get; set; }
        public string? LinkText { get; set; }
        public bool IsDeleted { get; set; }

        public User User { get; set; }
        public EmailType EmailType { get; set; }
        public EmailStatus EmailStatus { get; set; }
    }
    public class EmailNotificationEntityTypeConfiguration : IEntityTypeConfiguration<EmailNotification>
    {
        public void Configure(EntityTypeBuilder<EmailNotification> builder)
        {

        }
    }
}
