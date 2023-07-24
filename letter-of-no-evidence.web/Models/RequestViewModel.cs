using letter_of_no_evidence.web.Helper;
using System.ComponentModel.DataAnnotations;

namespace letter_of_no_evidence.web.Models
{
    public class RequestViewModel : AgentDetailsViewModel
    {
        public string? RequestNumber { get; set; }
        [MaxLength(100)]
        [DataType(DataType.EmailAddress)]
        [Email(ErrorMessage = Constants.Valid_Email_Required)]
        [RegularExpression(@"^(?!.*<[^>]+>).*", ErrorMessage = Constants.Html_Tags_Not_Allowed)]
        public string ContactEmail { get; set; }
        [MaxLength(100)]
        [DataType(DataType.EmailAddress)]
        [RegularExpression(@"^(?!.*<[^>]+>).*", ErrorMessage = Constants.Html_Tags_Not_Allowed)]
        [Compare("ContactEmail", ErrorMessage = Constants.Email_Not_Match)]
        public string CompareEmail { get; set; }
    }
}
