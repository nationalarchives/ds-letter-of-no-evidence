using letter_of_no_evidence.model;
using letter_of_no_evidence.web.Models;

namespace letter_of_no_evidence.web.Helper
{
    public static class Mapper
    {
        public static RequestModel MapToRequestModel(this RequestViewModel model)
        {
            var returnModel = new RequestModel
            {
                RequestNumber = model.RequestNumber,
                SubjectFirstName = model.SubjectFirstName,
                SubjectLastName = model.SubjectLastName,
                AlternativeFirstName = model.AlternativeFirstName,
                AlternativeLastName = model.AlternativeLastName,
                DateOfBirth = model.DateOfBirth,
                DateOfDeath = model.DateOfDeath,
                CountryOfBirth = model.CountryOfBirth,
                ContactTitle = model.ContactTitle,
                ContactFirstName = model.ContactFirstName,
                ContactLastName = model.ContactLastName,
                ContactAddress1 = model.ContactAddress1,
                ContactAddress2 = model.ContactAddress2,
                ContactCity = model.ContactCity,
                ContactCounty = model.ContactCounty,
                ContactPostCode = model.ContactPostCode,
                ContactCountry = model.ContactCountry,
                LetterToRequestor = model.LetterToRequestor,
                AgentCompanyName = model.AgentCompanyName,
                AgentFirstName = model.AgentFirstName,
                AgentLastName = model.AgentLastName,
                AgentAddress1 = model.AgentAddress1,
                AgentAddress2 = model.AgentAddress2,
                AgentCity = model.AgentCity,
                AgentCounty = model.AgentCounty,
                AgentPostCode = model.AgentPostCode,
                AgentCountry = model.AgentCountry,
                ContactEmail = model.ContactEmail,
                Payments = new List<PaymentModel>()
            };
            return returnModel;
        }

        public static RequestViewModel MapToRequestViewModel(this AgentDetailsViewModel model, RequestViewModel requestViewModel)
        {
            requestViewModel.SubjectFirstName = model.SubjectFirstName;
            requestViewModel.SubjectLastName = model.SubjectLastName;
            requestViewModel.AlternativeFirstName = model.AlternativeFirstName;
            requestViewModel.AlternativeLastName = model.AlternativeLastName;
            requestViewModel.DateOfBirth = model.DateOfBirth;
            requestViewModel.DateOfDeath = model.DateOfDeath;
            requestViewModel.CountryOfBirth = model.CountryOfBirth;

            requestViewModel.ContactTitle = model.ContactTitle;
            requestViewModel.ContactFirstName = model.ContactFirstName;
            requestViewModel.ContactLastName = model.ContactLastName;
            requestViewModel.ContactAddress1 = model.ContactAddress1;
            requestViewModel.ContactAddress2 = model.ContactAddress2;
            requestViewModel.ContactCity = model.ContactCity;
            requestViewModel.ContactCounty = model.ContactCounty;
            requestViewModel.ContactPostCode = model.ContactPostCode;
            requestViewModel.ContactCountry = model.ContactCountry;
            requestViewModel.LetterToRequestor = model.LetterToRequestor;

            requestViewModel.AgentCompanyName = model.AgentCompanyName;
            requestViewModel.AgentFirstName = model.AgentFirstName;
            requestViewModel.AgentLastName = model.AgentLastName;
            requestViewModel.AgentAddress1 = model.AgentAddress1;
            requestViewModel.AgentAddress2 = model.AgentAddress2;
            requestViewModel.AgentCity = model.AgentCity;
            requestViewModel.AgentCounty = model.AgentCounty;
            requestViewModel.AgentPostCode = model.AgentPostCode;
            requestViewModel.AgentCountry = model.AgentCountry;

            return requestViewModel;
        }

        public static RequestViewModel MapToRequestViewModel(this ContactDetailsViewModel model, RequestViewModel requestViewModel)
        {
            requestViewModel.SubjectFirstName = model.SubjectFirstName;
            requestViewModel.SubjectLastName = model.SubjectLastName;
            requestViewModel.AlternativeFirstName = model.AlternativeFirstName;
            requestViewModel.AlternativeLastName = model.AlternativeLastName;
            requestViewModel.DateOfBirth = model.DateOfBirth;
            requestViewModel.DateOfDeath = model.DateOfDeath;
            requestViewModel.CountryOfBirth = model.CountryOfBirth;

            requestViewModel.ContactTitle = model.ContactTitle;
            requestViewModel.ContactFirstName = model.ContactFirstName;
            requestViewModel.ContactLastName = model.ContactLastName;
            requestViewModel.ContactAddress1 = model.ContactAddress1;
            requestViewModel.ContactAddress2 = model.ContactAddress2;
            requestViewModel.ContactCity = model.ContactCity;
            requestViewModel.ContactCounty = model.ContactCounty;
            requestViewModel.ContactPostCode = model.ContactPostCode;
            requestViewModel.ContactCountry = model.ContactCountry;
            requestViewModel.LetterToRequestor = model.LetterToRequestor;

            return requestViewModel;
        }

        public static RequestViewModel MapToRequestViewModel(this SubjectDetailsViewModel model, RequestViewModel requestViewModel)
        {
            requestViewModel.SubjectFirstName = model.SubjectFirstName;
            requestViewModel.SubjectLastName = model.SubjectLastName;
            requestViewModel.AlternativeFirstName = model.AlternativeFirstName;
            requestViewModel.AlternativeLastName = model.AlternativeLastName;
            requestViewModel.DateOfBirth = model.DateOfBirth;
            requestViewModel.DateOfDeath = model.DateOfDeath;
            requestViewModel.CountryOfBirth = model.CountryOfBirth;

            return requestViewModel;
        }

        public static RequestViewModel ClearAgentDetails(this RequestViewModel model)
        {
            model.AgentCompanyName = null;
            model.AgentFirstName = null;
            model.AgentLastName = null;
            model.AgentAddress1 = null;
            model.AgentAddress2 = null;
            model.AgentCity = null;
            model.AgentCounty = null;
            model.AgentPostCode = null;
            model.AgentCountry = null;

            return model;
        }
    }
}
