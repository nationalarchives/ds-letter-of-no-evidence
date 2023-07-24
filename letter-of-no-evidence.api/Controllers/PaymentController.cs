using letter_of_no_evidence.api.Service;
using letter_of_no_evidence.model;
using Microsoft.AspNetCore.Mvc;

namespace letter_of_no_evidence.api.Controllers
{
    [Route("letter-of-no-evidence-api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        private readonly ILogger _logger;

        public PaymentController(IPaymentService paymentService, ILogger<RequestController> logger)
        {
            _paymentService = paymentService;
            _logger = logger;
        }

        [HttpPut("create")]
        public async Task<ActionResult<RequestResponseModel>> CreatePayment(PaymentModel paymentModel)
        {
            await _paymentService.CreatePaymentAsync(paymentModel);
            return Ok();
        }
    }
}
