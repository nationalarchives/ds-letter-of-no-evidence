using letter_of_no_evidence.api.Service;
using letter_of_no_evidence.model;
using Microsoft.AspNetCore.Mvc;

namespace letter_of_no_evidence.api.Controllers
{
    [Route("letter-of-no-evidence-api/[controller]")]
    [ApiController]
    public class RequestController : ControllerBase
    {
        private readonly IRequestService _requestService;
        private readonly ILogger _logger;

        public RequestController(IRequestService requestService, ILogger<RequestController> logger)
        {
            _requestService = requestService;
            _logger = logger;
        }

        [HttpGet("getrequest/{requestNumber}")]
        public async Task<ActionResult<RequestModel>> GetRequest(string requestNumber)
        {
            var result = await _requestService.GetRequestByNumberAsync(requestNumber);
            return Ok(result);
        }

        [HttpPut("create")]
        public async Task<ActionResult<RequestResponseModel>> CreateBooking(RequestModel requestModel)
        {
            var result = await _requestService.CreateRequestAsync(requestModel);
            return Ok(result);
        }

        [HttpPost("update")]
        public async Task<ActionResult<RequestResponseModel>> UpdateBooking(RequestModel requestModel)
        {
            var result = await _requestService.UpdateRequestAsync(requestModel);
            return Ok(result);
        }
    }
}
