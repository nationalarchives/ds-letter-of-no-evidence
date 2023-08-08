using letter_of_no_evidence.model;
using letter_of_no_evidence.web.Helper;
using letter_of_no_evidence.web.Models;
using letter_of_no_evidence.web.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace letter_of_no_evidence.web.Controllers
{
    public class RequestController : Controller
    {
        private readonly IRequestService _requestService;
        private readonly IPaymentService _paymentService;
        private readonly IEmailService _emailService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RequestController(IRequestService requestService, IPaymentService paymentService, IEmailService emailService, IHttpContextAccessor httpContextAccessor)
        {
            _requestService = requestService;
            _paymentService = paymentService;
            _emailService = emailService;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        public IActionResult SubjectDetails()
        {
            var model = _httpContextAccessor.HttpContext.Session.GetObject<SubjectDetailsViewModel>("RequestFormDetails");
            return View(model);
        }

        [HttpPost]
        public IActionResult SubjectDetails(SubjectDetailsViewModel model)
        {
            if (ModelState.IsValid)
            {
                
                var requestViewModel = _httpContextAccessor.HttpContext.Session.GetObject<RequestViewModel>("RequestFormDetails") ?? new RequestViewModel();
                _httpContextAccessor.HttpContext.Session.SetObject("RequestFormDetails", model.MapToRequestViewModel(requestViewModel));
                return RedirectToAction("ContactDetails");
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult ContactDetails()
        {
            var model = _httpContextAccessor.HttpContext.Session.GetObject<ContactDetailsViewModel>("RequestFormDetails");
            return View(model);
        }

        [HttpPost]
        public IActionResult ContactDetails(ContactDetailsViewModel model, string submitButton)
        {
            if (submitButton == Constants.Previous_Button)
            {
                return RedirectToAction("SubjectDetails");
            }
            if (ModelState.IsValid)
            {
                var requestViewModel = _httpContextAccessor.HttpContext.Session.GetObject<RequestViewModel>("RequestFormDetails");
                if (model.LetterToRequestor)
                {
                    requestViewModel = requestViewModel.ClearAgentDetails();
                    _httpContextAccessor.HttpContext.Session.SetObject("RequestFormDetails", model.MapToRequestViewModel(requestViewModel));
                    return RedirectToAction("ContactEmail");
                }
                _httpContextAccessor.HttpContext.Session.SetObject("RequestFormDetails", model.MapToRequestViewModel(requestViewModel));
                return RedirectToAction("PostalDetails");
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult PostalDetails()
        {
            var model = _httpContextAccessor.HttpContext.Session.GetObject<AgentDetailsViewModel>("RequestFormDetails");
            return View(model);
        }

        [HttpPost]
        public IActionResult PostalDetails(AgentDetailsViewModel model, string submitButton)
        {
            if (submitButton == Constants.Previous_Button)
            {
                return RedirectToAction("ContactDetails");
            }
            if (ModelState.IsValid)
            {
                var requestViewModel = _httpContextAccessor.HttpContext.Session.GetObject<RequestViewModel>("RequestFormDetails");
                _httpContextAccessor.HttpContext.Session.SetObject("RequestFormDetails", model.MapToRequestViewModel(requestViewModel));
                return RedirectToAction("ContactEmail");
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> ContactEmail()
        {
            var model = _httpContextAccessor.HttpContext.Session.GetObject<RequestViewModel>("RequestFormDetails");
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ContactEmail(RequestViewModel model, string submitButton)
        {
            if (submitButton == Constants.Previous_Button)
            {
                if (model.LetterToRequestor)
                {
                    return RedirectToAction("ContactDetails");
                }
                return RedirectToAction("PostalDetails");
            }
            if (ModelState.IsValid)
            {
                _httpContextAccessor.HttpContext.Session.SetObject("RequestFormDetails", model);
                return RedirectToAction("RequestSummary");
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult RequestSummary()
        {
            var model = _httpContextAccessor.HttpContext.Session.GetObject<RequestViewModel>("RequestFormDetails");
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> RequestSummary(string submitButton)
        {
            if (submitButton == Constants.Previous_Button)
            {
                return RedirectToAction("ContactEmail");
            }
            var model = _httpContextAccessor.HttpContext.Session.GetObject<RequestViewModel>("RequestFormDetails");
            model.RequestNumber = IdGenerator.TNAReferenceNumber();

            var requestModel = model.MapToRequestModel();

            var response = await _requestService.CreateRequestAsync(requestModel);
            if (response.IsSuccess)
            {
                requestModel.Id = response.RequestId;
                _httpContextAccessor.HttpContext.Session.RemoveObject("RequestFormDetails");

                return await CreatePayment(requestModel);
            }
            return View(model);
        }

        public async Task<IActionResult> RequestReceipt(string requestNumber)
        {
            var response = await _requestService.GetRequestAsync(requestNumber);
            if (response != null)
            {
                var paymentId = response.Payments?.OrderByDescending(x => x.Id)?.FirstOrDefault()?.PaymentId;
                var paymentResponse = await _paymentService.GetPaymentById(paymentId);

                var paymentModel = new PaymentModel
                {
                    RequestId = response.Id,
                    SessionId = paymentResponse.reference,
                    PaymentId = paymentResponse.payment_id,
                    Amount = Decimal.Divide(paymentResponse.amount, 100),
                    PaymentStatus = paymentResponse.state.status.ToPaymentStatus(),
                    ProcessFinished = paymentResponse.state.finished,
                    TransactionDate = DateTime.Now
                };

                await _requestService.AddNewPaymentAsync(paymentModel);

                if (paymentModel.PaymentStatus == PaymentStatus.Success)
                {
                    await _emailService.SendCustomerEmailAsync(response);

                    await _emailService.SendD365EmailAsync(response);
                }

                var model = new ReceiptViewModel
                {
                    RequestNumber = requestNumber,
                    PaymentStatus = paymentModel.PaymentStatus,
                    Amount = paymentModel.Amount,
                    SessionId = paymentResponse.reference
                };

                return View(model);
            }
            return NotFound();
        }

        public async Task<IActionResult> TryAgain(string requestNumber)
        {
            var response = await _requestService.GetRequestAsync(requestNumber);
            if (response != null)
            {
                return await CreatePayment(response);
            }
            return NotFound();
        }

        private async Task<IActionResult> CreatePayment(RequestModel model)
        {
            var retryCount = model.Payments?.Count ?? 0;
            var amount = int.Parse(Environment.GetEnvironmentVariable("LONE_Amount"));
            var sessionId = IdGenerator.GenerateSessionId(model.Id);
            var returnURL = $"{Environment.GetEnvironmentVariable("LONE_Return_URL")}{model.RequestNumber}";

            if (retryCount > 0)
            {
                sessionId = $"{sessionId}_{retryCount - 1}";
            }

            var paymentRequest = new PaymentRequestModel
            {
                amount = amount,
                reference = sessionId,
                return_url = returnURL,
                description = $"Letter of no evidence search request - {model.RequestNumber}",
                email = model.ContactEmail,
                language = "en"
            };

            var paymentResponse = await _paymentService.CreateNewPayment(paymentRequest);

            var paymentModel = new PaymentModel
            {
                RequestId = model.Id,
                SessionId = paymentResponse.reference,
                PaymentId = paymentResponse.payment_id,
                Amount = Decimal.Divide(paymentResponse.amount, 100),
                PaymentStatus = paymentResponse.state.status.ToPaymentStatus(),
                ProcessFinished = paymentResponse.state.finished,
                TransactionDate = DateTime.Now
            };

            await _requestService.AddNewPaymentAsync(paymentModel);

            return Redirect(paymentResponse._links.next_url.href);
        }
    }
}
