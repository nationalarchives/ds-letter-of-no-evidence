using letter_of_no_evidence.data;
using letter_of_no_evidence.domain;
using letter_of_no_evidence.model;

namespace letter_of_no_evidence.api.Service
{
    public class PaymentService : IPaymentService
    {
        private readonly LONEDBContext _context;
        public PaymentService(LONEDBContext context) 
        {
            _context = context;
        }

        public async Task CreatePaymentAsync(PaymentModel paymentModel)
        {
            await _context.Set<Payment>().AddAsync(new Payment
            {
                RequestId = paymentModel.RequestId,
                SessionId = paymentModel.SessionId,
                PaymentId = paymentModel.PaymentId,
                PaymentStatusId = (int)paymentModel.PaymentStatus,
                ProcessFinished = paymentModel.ProcessFinished,
                TransactionDate = paymentModel.TransactionDate,
                Amount = paymentModel.Amount
            });
            await _context.SaveChangesAsync();
        }

    }
}
