using letter_of_no_evidence.web.Models;

namespace letter_of_no_evidence.web.Service
{
    public interface IPaymentService
    {
        Task<PaymentResponseModel> CreateNewPayment(PaymentRequestModel requestModel);
        Task<PaymentResponseModel> GetPaymentById(string paymentId);
    }
}
