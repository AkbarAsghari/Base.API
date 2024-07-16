using API.Core.Interfaces.Services;
using API.Infrastructure.Entities;
using API.Infrastructure.Interfaces;
using API.Shared.DTOs.Notifications;
using API.Shared.Enums;
using AutoMapper;
using MD.PersianDateTime.Standard;
using Microsoft.EntityFrameworkCore;

namespace API.Core.Services
{
    public class EmailNotificationService : IEmailNotificationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public EmailNotificationService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<bool> ChangeEmailNotificationStatus(Guid emailId, int status)
        {
            var existEmail = await _unitOfWork.Repository<EmailNotification>().FirstOrDefault(x => x.ID == emailId);
            if (existEmail == null)
                return false;

            existEmail.EmailStatusId = status;
            _unitOfWork.Repository<EmailNotification>().Update(existEmail);
            return await _unitOfWork.Save(Guid.Empty);
        }

        public async Task<IEnumerable<EmailNotificationDTO>> GetMustBeSendEmails()
        {
            return _mapper.Map<IEnumerable<EmailNotificationDTO>>(
                await _unitOfWork.Repository<EmailNotification>().GetAll(x =>
                x.EmailStatusId == (int)EmailStatusEnum.MustBeSend, include: x => x.Include(i => i.User)));
        }

        public async Task<bool> SendConfirmEmail(Guid userId, string link, bool resend = false)
        {
            return await SendEmail(new SendEmailNotificationDTO
            {
                ReciptUserId = userId,
                EmailTypeId = resend ? (int)EmailTypeEnum.ResendActivationLink : (int)EmailTypeEnum.ConfirmEmail,
                Subject = "تایید آدرس پست الکترونیک",
                Message = "از اینکه یک حساب در دی‌ان‌اس لب ایجاد کرده‌اید متشکریم. خوشحالیم که ما را پیدا کردید برای تایید حساب خود، لطفا روی دکمه زیر کلیک کنید.",
                Link = link,
                LinkText = "تایید آدرس پست الکترونیک"
            });
        }

        public async Task<bool> SendForgetPasswordEmail(Guid userId, string fullName, string link)
        {
            return await SendEmail(new SendEmailNotificationDTO
            {
                ReciptUserId = userId,
                EmailTypeId = (int)EmailTypeEnum.ForgetPassword,
                Subject = $"درود {fullName}",
                Message = "درخواست شما را برای فراموشی رمز عبور در سرویس رایگان DNSLab دریافت کردیم . برای تنظیم رمز عبور جدید بر روی دکمه زیر کلیک کنید در غیر این صورت این ایمیل را نا دیده بگیرید",
                Link = link,
                LinkText = "بازیابی رمز عبور"
            });
        }

        public async Task<bool> SendTicketAnswerEmail(Guid userId, string ticketTitle, string ticketMessage)
        {
            return await SendEmail(new SendEmailNotificationDTO
            {
                ReciptUserId = userId,
                EmailTypeId = (int)EmailTypeEnum.AnswerTicket,
                Subject = $"پاسخ به تیکت \" {ticketTitle} \"",
                Message = $"تیکت شما در سایت دی ان اس لب پاسخ داده شده است برای مشاهده وارد سایت شوید." +
                                  $"پاسخ داده شده : {ticketMessage!.Trim()}",
                Link = "https://dnslab.link/user/auth",
                LinkText = "لینک ورود"
            });
        }

        public async Task<bool> SendSubscriptionWillBeExpired(Guid userId, string subscriptionType, DateTime endTime)
        {
            return await SendEmail(new SendEmailNotificationDTO
            {
                ReciptUserId = userId,
                EmailTypeId = (int)EmailTypeEnum.SubscriptionWillBeExpired,
                Subject = $"یادآوری مهلت اشتراک",
                Message = $"اشتراک {subscriptionType} شما در حال منقضی شدن میباشد و در تاریخ {new PersianDateTime(endTime)} مهلت استفاده است سرویس به پایان خواهد رسید. </br>" +
                $"لطفا جهت تمدید یا تغییر اشتراک خود از لینک زیر اقدام نمایید",
                Link = "https://dnslab.link/Subscriptions/Plans",
                LinkText = "تمدید اشتراک"
            });
        }

        private async Task<bool> SendEmail(SendEmailNotificationDTO emailNotification)
        {
            _unitOfWork.Repository<EmailNotification>().Insert(new EmailNotification
            {
                EmailStatusId = (int)EmailStatusEnum.MustBeSend,
                EmailTypeId = emailNotification.EmailTypeId,
                UserId = emailNotification.ReciptUserId,
                Subject = emailNotification.Subject,
                Message = emailNotification.Message,
                Link = emailNotification.Link,
                LinkText = emailNotification.LinkText,
            });

            return await _unitOfWork.Save(Guid.Empty);
        }
    }
}
