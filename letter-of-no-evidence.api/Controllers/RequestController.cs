using letter_of_no_evidence.api.Service;
using letter_of_no_evidence.model;
using Microsoft.AspNetCore.Mvc;

namespace letter_of_no_evidence.api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class RequestController : ControllerBase
    {
        private readonly IRequestService _requestService;

        public RequestController(IRequestService requestService)
        {
            _requestService = requestService;
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
