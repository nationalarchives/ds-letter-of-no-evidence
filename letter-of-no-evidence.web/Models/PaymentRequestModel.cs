namespace letter_of_no_evidence.web.Models
{
    public class PaymentRequestModel
    {
        public int amount { get; set; }
        public string reference { get; set; }
        public string return_url { get; set; }
        public string description { get; set; }
        public string email { get; set; }
        public string language { get; set; }
    }
}
