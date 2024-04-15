namespace letter_of_no_evidence.model
{
    public class RequestModel
    {
        public int Id { get; set; }
        public string? RequestNumber { get; set; }
        public string? SubjectFirstName { get; set; }
        public string? SubjectLastName { get; set; }
        public string? AlternativeFirstName { get; set; }
        public string? AlternativeLastName { get; set; }
        public string? DateOfBirth { get; set; }
        public string? DateOfDeath { get; set; }
        public string? CountryOfBirth { get; set; }
        public bool Renunciation { get; set; }
        public string? ContactTitle { get; set; }
        public string? ContactFirstName { get; set; }
        public string? ContactLastName { get; set; }
        public string? ContactAddress1 { get; set; }
        public string? ContactAddress2 { get; set; }
        public string? ContactCity { get; set; }
        public string? ContactCounty { get; set; }
        public string? ContactPostCode { get; set; }
        public string? ContactCountry { get; set; }
        public bool LetterToRequestor { get; set; }
        public string? AgentCompanyName { get; set; }
        public string? AgentFirstName { get; set; }
        public string? AgentLastName { get; set; }
        public string? AgentAddress1 { get; set; }
        public string? AgentAddress2 { get; set; }
        public string? AgentCity { get; set; }
        public string? AgentCounty { get; set; }
        public string? AgentPostCode { get; set; }
        public string? AgentCountry { get; set; }
        public string? ContactEmail { get; set; }
        public decimal PostalCost { get; set; }
        public List<PaymentModel> Payments { get; set; }
    }
}
