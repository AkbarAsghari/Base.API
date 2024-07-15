using API.Core.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private IPaymentService _TransactionService;
        private readonly IHttpContextAccessor _ContextAccessor;
        public PaymentController(IHttpContextAccessor contextAccessor, IPaymentService transactionService)
        {
            _ContextAccessor = contextAccessor;
            _TransactionService = transactionService;
        }

        [HttpGet("Verify")]
        public async Task<IActionResult> Verify(long trackId)
        {
            return Ok(await _TransactionService.Verify(trackId));
        }
    }
}
