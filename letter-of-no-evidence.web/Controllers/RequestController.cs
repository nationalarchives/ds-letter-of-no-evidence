using Amazon.SimpleSystemsManagement.Model;
using letter_of_no_evidence.model;
using letter_of_no_evidence.web.Helper;
using letter_of_no_evidence.web.Models;
using letter_of_no_evidence.web.Service;
using Microsoft.AspNetCore.Mvc;
using System.ServiceModel;

namespace letter_of_no_evidence.web.Controllers
{
    public class RequestController : Controller
    {
        private readonly IRequestService _requestService;
        private readonly IPaymentService _paymentService;

        public RequestController(IRequestService requestService, IPaymentService paymentService)
        {
            _requestService = requestService;
            _paymentService = paymentService;
        }

        [HttpGet]
        public IActionResult SubjectDetails()
        {
            var model = HttpContext.Session.GetObject<SubjectDetailsViewModel>("RequestFormDetails");
            return View(model);
        }

        [HttpPost]
        public IActionResult SubjectDetails(SubjectDetailsViewModel model)
        {
            if (ModelState.IsValid)
            {
                var requestViewModel = HttpContext.Session.GetObject<RequestViewModel>("RequestFormDetails") ?? new RequestViewModel();
                HttpContext.Session.SetObject("RequestFormDetails", model.MapToRequestViewModel(requestViewModel));
                return RedirectToAction("ContactDetails");
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult ContactDetails()
        {
            var model = HttpContext.Session.GetObject<ContactDetailsViewModel>("RequestFormDetails");
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
                var requestViewModel = HttpContext.Session.GetObject<RequestViewModel>("RequestFormDetails");
                if (model.LetterToRequestor)
                {
                    requestViewModel = requestViewModel.ClearAgentDetails();
                    HttpContext.Session.SetObject("RequestFormDetails", model.MapToRequestViewModel(requestViewModel));
                    return RedirectToAction("ContactEmail");
                }
                HttpContext.Session.SetObject("RequestFormDetails", model.MapToRequestViewModel(requestViewModel));
                return RedirectToAction("PostalDetails");
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult PostalDetails()
        {
            var model = HttpContext.Session.GetObject<AgentDetailsViewModel>("RequestFormDetails");
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
                var requestViewModel = HttpContext.Session.GetObject<RequestViewModel>("RequestFormDetails");
                HttpContext.Session.SetObject("RequestFormDetails", model.MapToRequestViewModel(requestViewModel));
                return RedirectToAction("ContactEmail");
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> ContactEmail()
        {
            var model = HttpContext.Session.GetObject<RequestViewModel>("RequestFormDetails");
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
                HttpContext.Session.SetObject("RequestFormDetails", model);
                return RedirectToAction("RequestSummary");
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult RequestSummary()
        {
            var model = HttpContext.Session.GetObject<RequestViewModel>("RequestFormDetails");
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> RequestSummary(string submitButton)
        {
            if (submitButton == Constants.Previous_Button)
            {
                return RedirectToAction("ContactEmail");
            }
            var model = HttpContext.Session.GetObject<RequestViewModel>("RequestFormDetails");
            model.RequestNumber = IdGenerator.TNAReferenceNumber();

            var requestModel = model.MapToRequestModel();

            var response = await _requestService.CreateRequestAsync(requestModel);
            if (response.IsSuccess)
            {
                HttpContext.Session.RemoveObject("RequestFormDetails");

                var amount = int.Parse(Environment.GetEnvironmentVariable("LONE_Amount"));
                var sessionId = IdGenerator.GenerateSessionId(response.RequestId);
                var returnURL = $"{Environment.GetEnvironmentVariable("LONE_Return_URL")}{model.RequestNumber}";

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
                    RequestId = response.RequestId,
                    SessionId = paymentResponse.reference,
                    PaymentId = paymentResponse.payment_id,
                    Amount = paymentResponse.amount / 100,
                    PaymentStatus = paymentResponse.state.status.ToPaymentStatus(),
                    ProcessFinished = paymentResponse.state.finished,
                    TransactionDate = DateTime.Now
                };

                await _requestService.AddNewPaymentAsync(paymentModel);

                return Redirect(paymentResponse._links.next_url.href);
            }
            return View(model);
        }

        public async Task<IActionResult> RequestReceipt(string requestNumber)
        {
            var response = await _requestService.GetRequestAsync(requestNumber);
            if (response != null)
            {
                var model = new ReceiptViewModel
                {
                    RequestNumber = requestNumber,
                    SubjectFirstName = response.SubjectFirstName,
                    SubjectLastName = response.SubjectLastName,
                    PaymentId = response.Payments?.FirstOrDefault()?.PaymentId,
                    SessionId = response.Payments?.FirstOrDefault()?.SessionId
                };

                var paymentResponse = await _paymentService.GetPaymentById(model.PaymentId);

                model.PaymentStatus = paymentResponse.state.status.ToPaymentStatus();
                return View(model);
            }
            return NotFound();
        }
    }
}
