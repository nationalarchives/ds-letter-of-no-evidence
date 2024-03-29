﻿namespace letter_of_no_evidence.domain
{
    public class Payment
    {
        public int Id { get; set; }
        public string SessionId { get; set; }//N/23xxxx --> GOV pay reference
        public string? PaymentId { get; set; }
        public int PaymentStatusId { get; set; }
        public PaymentStatus PaymentStatus { get; set;}
        public bool ProcessFinished { get; set; }
        public DateTime TransactionDate { get; set;}
        public decimal Amount { get; set; }
        public int RequestId { get; set; }
        public Request Request { get; set; }
    }
}
