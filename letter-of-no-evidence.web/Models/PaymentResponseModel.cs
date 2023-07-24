namespace letter_of_no_evidence.web.Models
{
    public class PaymentResponseModel
    {
        public int amount { get; set; }
        public string description { get; set; }
        public string reference { get; set; }
        public string language { get; set; }
        public State state { get; set; }
        public string payment_id { get; set; }
        public string payment_provider { get; set; }
        public DateTime created_date { get; set; }
        public RefundSummary refund_summary { get; set; }
        public bool delayed_capture { get; set; }
        public bool moto { get; set; }
        public string return_url { get; set; }
        public Links _links { get; set; }
    }

    public class Cancel
    {
        public string href { get; set; }
        public string method { get; set; }
    }

    public class Events
    {
        public string href { get; set; }
        public string method { get; set; }
    }

    public class Links
    {
        public Self self { get; set; }
        public NextUrl next_url { get; set; }
        public NextUrlPost next_url_post { get; set; }
        public Events events { get; set; }
        public Refunds refunds { get; set; }
        public Cancel cancel { get; set; }
    }

    public class NextUrl
    {
        public string href { get; set; }
        public string method { get; set; }
    }

    public class NextUrlPost
    {
        public string type { get; set; }
        public Params @params { get; set; }
        public string href { get; set; }
        public string method { get; set; }
    }

    public class Params
    {
        public string chargeTokenId { get; set; }
    }

    public class Refunds
    {
        public string href { get; set; }
        public string method { get; set; }
    }

    public class RefundSummary
    {
        public string status { get; set; }
        public int amount_available { get; set; }
        public int amount_submitted { get; set; }
    }

    public class Self
    {
        public string href { get; set; }
        public string method { get; set; }
    }

    public class State
    {
        public string status { get; set; }
        public bool finished { get; set; }
    }
}
