using letter_of_no_evidence.web.Helper;
using System.ComponentModel.DataAnnotations;

namespace letter_of_no_evidence.web.Models
{
    public class AgentDetailsViewModel : ContactDetailsViewModel
    {
        [MaxLength(100)]
        public string? AgentCompanyName { get; set; }
        [MaxLength(50)]
        public string? AgentFirstName { get; set; }
        [RequiredIf("LetterToRequestor", false, ErrorMessage = Constants.Lastname_Required)]
        [MaxLength(50)]
        public string? AgentLastName { get; set; }
        [RequiredIf("LetterToRequestor", false, ErrorMessage = Constants.Address1_Required)]
        [MaxLength(100)]
        public string? AgentAddress1 { get; set; }
        [MaxLength(100)]
        public string? AgentAddress2 { get; set; }
        [RequiredIf("LetterToRequestor", false, ErrorMessage = Constants.City_Required)]
        [MaxLength(100)]
        public string? AgentCity { get; set; }
        [MaxLength(100)]
        public string? AgentCounty { get; set; }
        [RequiredIf("LetterToRequestor", false, ErrorMessage = Constants.PostCode_Required)]
        [MaxLength(30)]
        public string? AgentPostCode { get; set; }
        [RequiredIf("LetterToRequestor", false, ErrorMessage = Constants.Country_Required)]
        [MaxLength(100)]
        public string? AgentCountry { get; set; }
    }
}
