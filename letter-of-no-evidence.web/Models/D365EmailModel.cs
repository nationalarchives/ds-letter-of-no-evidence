using System.Xml.Serialization;

namespace letter_of_no_evidence.web.Models
{
    [XmlRoot("Case")]
    public class D365EmailModel
    {
        public string enquiry_id { get; set; }
        public string payment_reference { get; set; }
        public decimal amount_received { get; set; }
        public string? subject_firstname { get; set; }
        public string subject_lastname { get; set; }
        public string? alternative_firstname { get; set; }
        public string? alternative_lastname { get; set; }
        public string birth_date { get; set; }
        public string? death_date { get; set; }
        public string? country_of_birth { get; set; }
        public string? contact_title { get; set; }
        public string? contact_firstname { get; set; }
        public string contact_lastname { get; set; }
        public string contact_email { get; set; }
        public string contact_address1 { get; set; }
        public string? contact_address2 { get; set; }
        public string contact_town_city { get; set; }
        public string? contact_county { get; set; }
        public string contact_postcode { get; set; }
        public string contact_country { get; set; }
        public string? agent_companyname { get; set; }
        public string? agent_fullname { get; set; }
        public string? agent_address1 { get; set; }
        public string? agent_address2 { get; set; }
        public string? agent_town_city { get; set; }
        public string? agent_county { get; set; }
        public string? agent_postcode { get; set; }
        public string? agent_country { get; set; }
    }
}
