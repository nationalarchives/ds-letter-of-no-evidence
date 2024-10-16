using letter_of_no_evidence.api.Service;
using letter_of_no_evidence.model;
using Microsoft.AspNetCore.Mvc;

namespace letter_of_no_evidence.api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        private readonly IDeliveryService _deliveryService;

        public PaymentController(IPaymentService paymentService, IDeliveryService deliveryService)
        {
            _paymentService = paymentService;
            _deliveryService = deliveryService;
        }

        [HttpPut("create")]
        public async Task<ActionResult<RequestResponseModel>> CreatePayment(PaymentModel paymentModel)
        {
            await _paymentService.CreatePaymentAsync(paymentModel);
            return Ok();
        }

        [HttpGet("getdeliverycost/{zoneNo}")]
        public async Task<ActionResult<decimal>> GetDeliveryCost(int zoneNo)
        {
            var result = await _deliveryService.GetDeliveryCost(zoneNo);
            return Ok(result);
        }
    }
}
