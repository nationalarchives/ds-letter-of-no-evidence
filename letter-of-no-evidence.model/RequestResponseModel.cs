namespace letter_of_no_evidence.model
{
    public class RequestResponseModel
    {
        public int RequestId { get; set; }
        public string RequestNumber { get; set; }
        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; }
    }
}
