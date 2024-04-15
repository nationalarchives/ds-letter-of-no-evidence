using letter_of_no_evidence.web.Helper;
using System.ComponentModel.DataAnnotations;

namespace letter_of_no_evidence.web.Models
{
    public class SubjectDetailsViewModel
    {
        [Required(ErrorMessage = Constants.Firstname_Required)]
        [MaxLength(50)]
        public string SubjectFirstName { get; set; }
        [Required(ErrorMessage = Constants.Lastname_Required)]
        [MaxLength(50)]
        public string SubjectLastName { get; set; }
        [MaxLength(50)]
        public string? AlternativeFirstName { get; set; }
        [MaxLength(50)]
        public string? AlternativeLastName { get; set; }
        [Required(ErrorMessage = Constants.DOB_Required)]
        [MaxLength(30)]
        public string DateOfBirth { get; set; }
        [MaxLength(30)]
        public string? DateOfDeath { get; set; }
        [MaxLength(100)]
        public string? CountryOfBirth { get; set; }
        public bool Renunciation { get; set; }
    }
}
