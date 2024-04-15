using letter_of_no_evidence.model;
using letter_of_no_evidence.web.Helper;
using letter_of_no_evidence.web.Models;
using letter_of_no_evidence.web.Service;
using Microsoft.AspNetCore.Mvc;

namespace letter_of_no_evidence.web.Controllers
{
    public class RequestController : Controller
    {
        private readonly IRequestService _requestService;
        private readonly IPaymentService _paymentService;
        private readonly IEmailService _emailService;
        private readonly IRecordCopyingService _recordCopyingService;

        public RequestController(IRequestService requestService, IPaymentService paymentService, IEmailService emailService, IRecordCopyingService recordCopyingService)
        {
            _requestService = requestService;
            _paymentService = paymentService;
            _emailService = emailService;
            _recordCopyingService = recordCopyingService;
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
        public async Task<IActionResult> ContactDetails()
        {
            ViewBag.Countries = await _recordCopyingService.GetCountryAsListItem();
            var model = HttpContext.Session.GetObject<ContactDetailsViewModel>("RequestFormDetails");
            model.ContactCountry = model.ContactCountry ?? "United Kingdom";
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
        public async Task<IActionResult> PostalDetails()
        {
            ViewBag.Countries = await _recordCopyingService.GetCountryAsListItem();
            var model = HttpContext.Session.GetObject<AgentDetailsViewModel>("RequestFormDetails");
            model.AgentCountry = model.AgentCountry ?? "United Kingdom";
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
        public async Task<IActionResult> RequestSummary()
        {
            var model = HttpContext.Session.GetObject<RequestViewModel>("RequestFormDetails");
            var country = model.LetterToRequestor ? model.ContactCountry : model.AgentCountry;
            var zoneNo = await _recordCopyingService.GetDeliveryZone(country);
            model.PostalCost = await _requestService.GetDeliveryCostAsync(zoneNo);
            HttpContext.Session.SetObject("RequestFormDetails", model);
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
                requestModel.Id = response.RequestId;
                HttpContext.Session.RemoveObject("RequestFormDetails");

                return await CreatePayment(requestModel);
            }
            return View(model);
        }

        public async Task<IActionResult> RequestReceipt(string requestNumber)
        {
            var response = await _requestService.GetRequestAsync(requestNumber);
            if (response != null)
            {
                var payment = response.Payments?.OrderByDescending(x => x.Id)?.FirstOrDefault();
                if (payment != null)
                {
                    if (payment.ProcessFinished)
                    {
                        var viewModel = new ReceiptViewModel
                        {
                            RequestNumber = requestNumber,
                            PaymentStatus = payment.PaymentStatus,
                            Amount = payment.Amount,
                            SessionId = payment.SessionId
                        };

                        return View(viewModel);
                    }
                    
                    var paymentResponse = await _paymentService.GetPaymentById(payment.PaymentId);

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
            var amount = int.Parse(Environment.GetEnvironmentVariable("LONE_Amount")) + Convert.ToInt32(model.PostalCost * 100);
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
