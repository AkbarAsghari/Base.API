using API.Shared.DTOs.Zibal.Request;

namespace API.Core.Interfaces.Services
{
    public interface IPaymentService
    {
        Task InquiryAsync(long trackId);
        Task<bool> VerifyAsync(long trackId);
        Task<IEnumerable<long>> GetAllRequiredToInquiryTransactions();
        Task<RequestResponseDTO> RequestPaymentUrl(int amount);
    }
}
