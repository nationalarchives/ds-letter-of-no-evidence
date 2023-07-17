using letter_of_no_evidence.data;

namespace letter_of_no_evidence.api.Service
{
    public class PaymentService : IPaymentService
    {
        private readonly LONEDBContext _context;
        public PaymentService(LONEDBContext context) 
        {
            _context = context;
        }

    }
}
