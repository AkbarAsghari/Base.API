using API.Infrastructure.Entities;
using API.Infrastructure.Interfaces;
using API.Shared;
using API.Shared.DTOs.Zibal.Inquery;
using API.Shared.DTOs.Zibal.Request;
using API.Shared.DTOs.Zibal.Verify;
using API.Shared.Enums;
using API.Shared.Exceptions;
using AutoMapper;
using API.Core.Interfaces.Services;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Text;
using System.Text.Json;

namespace API.Core.Services
{
    public class PaymentService : IPaymentService, IDisposable
    {
        private readonly IUnitOfWork _UnitOfWork;
        private readonly IMapper _Mapper;
        private readonly HttpClient _Client;

        public PaymentService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _Client = new HttpClient();
            _UnitOfWork = unitOfWork;
            _Client.BaseAddress = new Uri(Zibal.APIUrl);
            _Mapper = mapper;
        }

        public async Task<IEnumerable<long>> GetAllRequiredToInquiryTransactions()
        {
            return await _UnitOfWork.Repository<Payment>()
                .IgnoreQueryFilters()
                .GetEntities(x => x.PaymentStatusId == (int)ZibalStatusEnum.AwaitingPayment &&
                                  x.CreateDate >= DateTime.UtcNow.AddDays(-1))
                .Select(x => x.TrackId).ToListAsync();
        }

        public async Task InquiryAsync(long trackId)
        {
            var existTransaction = await _UnitOfWork.Repository<Payment>().FirstOrDefault(x => x.TrackId == trackId);
            if (existTransaction == null)
                throw new PaymentNotFoundException();

            var requestData = new InquiryDTO
            {
                Merchant = Zibal.Merchant,
                TrackId = trackId
            };

            var paymentResponse = await HttpRequestExecuterAsync<InquiryDTO, InquiryResponseDTO>
                (requestData, "/v1/inquiry");

            if (paymentResponse.Result == (int)ZibalResultEnum.Success)
            {
                long? refNumber = string.IsNullOrEmpty(paymentResponse.RefNumber) ? null : Convert.ToInt64(paymentResponse.RefNumber);
                existTransaction.PaymentStatusId = paymentResponse.Status;
                existTransaction.CardNumber = paymentResponse.CardNumber;
                existTransaction.RefNumber = refNumber;
                existTransaction.PaidAt = paymentResponse.PaidAt;
                existTransaction.VerifiedAt = paymentResponse.VerifiedAt;
            }

            if (_UnitOfWork.Repository<Payment>().Update(existTransaction))
                await _UnitOfWork.Save(Guid.Empty);
        }

        public async Task<bool> VerifyAsync(long trackId)
        {
            var existTransaction = await _UnitOfWork.Repository<Payment>().FirstOrDefault(x => x.TrackId == trackId);
            if (existTransaction == null)
                throw new PaymentNotFoundException();

            if (existTransaction.PaymentStatusId == (int)ZibalStatusEnum.PaidConfirmed)
                return true;

            var requestData = new VerifyDTO
            {
                Merchant = Zibal.Merchant,
                TrackId = trackId
            };

            var paymentResponse = await HttpRequestExecuterAsync<VerifyDTO, VerifyResponseDTO>
                (requestData, "/v1/verify");

            if (paymentResponse.Result == (int)ZibalResultEnum.Success)
            {
                long? refNumber = string.IsNullOrEmpty(paymentResponse.RefNumber) ? null : Convert.ToInt64(paymentResponse.RefNumber);
                existTransaction.PaymentStatusId = paymentResponse.Status!.Value;
                existTransaction.CardNumber = paymentResponse.CardNumber;
                existTransaction.RefNumber = refNumber;
                existTransaction.PaidAt = paymentResponse.PaidAt;

                if (_UnitOfWork.Repository<Payment>().Update(existTransaction))
                    await _UnitOfWork.Save();

                return paymentResponse.Status == (int)ZibalStatusEnum.PaidConfirmed;
            }

            return false;
        }

        private async Task<TOut> HttpRequestExecuterAsync<T, TOut>(T data, string url)
        {
            var dataJson = JsonSerializer.Serialize(data, new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            });
            var stringContent = new StringContent(dataJson, Encoding.UTF8, "application/json");
            var result = await _Client.PostAsync(url, stringContent);

            var response = await result.Content.ReadAsStringAsync();

            if (result.StatusCode == HttpStatusCode.OK)
                return JsonSerializer.Deserialize<TOut>(response, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;
            else
                throw new Exception();
        }

        public void Dispose() => _Client.Dispose();

        public async Task<RequestResponseDTO> RequestPaymentUrl(int amount)
        {
            var requestData = new RequestDTO
            {
                Merchant = Zibal.Merchant,
                Amount = amount,
                CallbackUrl = Zibal.CallBackUrl,
            };

            return await HttpRequestExecuterAsync<RequestDTO, RequestResponseDTO>
                (requestData, "/v1/request");
        }
    }
}
