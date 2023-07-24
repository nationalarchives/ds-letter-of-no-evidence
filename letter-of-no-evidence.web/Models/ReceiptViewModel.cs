using letter_of_no_evidence.model;

namespace letter_of_no_evidence.web.Models
{
    public class ReceiptViewModel
    {
        public string? RequestNumber { get; set; } 
        public string? SubjectFirstName { get; set; }
        public string? SubjectLastName { get; set; }
        public string? SessionId { get; set; }
        public string? PaymentId { get; set; }
        public PaymentStatus? PaymentStatus { get; set; }
    }
}
