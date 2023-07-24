using letter_of_no_evidence.model;

namespace letter_of_no_evidence.api.Service
{
    public interface IPaymentService
    {
        Task CreatePaymentAsync(PaymentModel paymentModel);
    }
}
