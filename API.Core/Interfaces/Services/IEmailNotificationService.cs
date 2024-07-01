using API.Shared.DTOs.Notifications;

namespace API.Core.Interfaces.Services
{
    public interface IEmailNotificationService
    {
        public Task<bool> SendConfirmEmail(Guid userId, string link, bool resend = false);
        public Task<bool> SendTicketAnswerEmail(Guid userId, string ticketTitle, string ticketMessage);
        public Task<bool> SendDNSIPChangeEmail(Guid userId, string hostName, string oldIP, string newIP);
        public Task<bool> SendForgetPasswordEmail(Guid userId, string fullName, string link);
        public Task<IEnumerable<EmailNotificationDTO>> GetMustBeSendEmails();
        public Task<bool> ChangeEmailNotificationStatus(Guid emailId, int status);
    }
}
