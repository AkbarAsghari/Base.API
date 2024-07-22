using API.Core.Interfaces.Services;
using Quartz;

namespace API.Core.ScheduleJobs
{
    [DisallowConcurrentExecution]
    public class InquiryPaymentsStatusJob : IJob
    {
        private readonly IPaymentService _PaymentService;
        public InquiryPaymentsStatusJob(IPaymentService paymentService)
        {
            _PaymentService = paymentService;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            var existInquiryList = await _PaymentService.GetAllRequiredToInquiryTransactions();
            foreach (var item in existInquiryList)
            {
                try
                {
                    await _PaymentService.InquiryAsync(item);
                }
                catch (Exception ex)
                {
                }
            }
        }
    }
}
