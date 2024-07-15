﻿using API.Shared.DTOs.Zibal.Request;

namespace API.Core.Interfaces.Services
{
    public interface IPaymentService
    {
        Task Inquiry(long trackId);
        Task<bool> Verify(long trackId);
        Task<IEnumerable<long>> GetAllRequiredToInquiryTransactions();
        Task<RequestResponseDTO> RequestPaymentUrl(int amount);
    }
}
