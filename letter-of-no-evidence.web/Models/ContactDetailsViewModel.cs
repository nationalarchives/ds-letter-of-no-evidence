using letter_of_no_evidence.web.Helper;
using System.ComponentModel.DataAnnotations;

namespace letter_of_no_evidence.web.Models
{
    public class ContactDetailsViewModel : SubjectDetailsViewModel
    {
        [MaxLength(30)]
        public string? ContactTitle { get; set; }
        [MaxLength(50)]
        public string? ContactFirstName { get; set; }
        [Required(ErrorMessage = Constants.Lastname_Required)]
        [MaxLength(50)]
        public string ContactLastName { get; set; }
        [Required(ErrorMessage = Constants.Address1_Required)]
        [MaxLength(100)]
        public string ContactAddress1 { get; set; }
        [MaxLength(100)]
        public string? ContactAddress2 { get; set; }
        [Required(ErrorMessage = Constants.City_Required)]
        [MaxLength(100)]
        public string? ContactCity { get; set; }
        [MaxLength(100)]
        public string? ContactCounty { get; set; }
        [Required(ErrorMessage = Constants.PostCode_Required)]
        [MaxLength(100)]
        public string? ContactPostCode { get; set; }
        [Required(ErrorMessage = Constants.Country_Required)]
        [MaxLength(100)]
        public string? ContactCountry { get; set; }
        public bool LetterToRequestor { get; set; }
    }
}
