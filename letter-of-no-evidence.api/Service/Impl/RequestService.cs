using letter_of_no_evidence.data;
using letter_of_no_evidence.domain;
using letter_of_no_evidence.model;
using Microsoft.EntityFrameworkCore;

namespace letter_of_no_evidence.api.Service
{
    public class RequestService : IRequestService
    {
        private readonly LONEDBContext _context;
        public RequestService(LONEDBContext context)
        {
            _context = context;
        }

        public async Task<RequestModel> GetRequestByNumberAsync(string requestNumber)
        {
            var request = await _context.Requests
                                .Include(b => b.Payments)
                                .FirstOrDefaultAsync(r => r.RequestNumber == requestNumber);

            if (request != null)
            {
                var payments = request.Payments?
                                .Select(x => new PaymentModel
                                {
                                    PaymentId = x.PaymentId,
                                    SessionId = x.SessionId,
                                    Amount = x.Amount,
                                    ProcessFinished = x.ProcessFinished,
                                    TransactionDate = x.TransactionDate,
                                    PaymentStatus = (model.PaymentStatus)x.PaymentStatusId,
                                    Id = x.Id
                                })?.ToList();

                return new RequestModel
                {
                    Id = request.Id,
                    RequestNumber = request.RequestNumber,
                    SubjectFirstName = request.SubjectFirstName,
                    SubjectLastName = request.SubjectLastName,
                    AlternativeFirstName = request.AlternativeFirstName,
                    AlternativeLastName = request.AlternativeLastName,
                    DateOfBirth = request.DateOfBirth,
                    DateOfDeath = request.DateOfDeath,
                    CountryOfBirth = request.CountryOfBirth,
                    ContactTitle = request.ContactTitle,
                    ContactFirstName = request.ContactFirstName,
                    ContactLastName = request.ContactLastName,
                    ContactAddress1 = request.ContactAddress1,
                    ContactAddress2 = request.ContactAddress2,
                    ContactCity = request.ContactCity,
                    ContactCounty = request.ContactCounty,
                    ContactPostCode = request.ContactPostCode,
                    ContactCountry = request.ContactCountry,
                    LetterToRequestor = request.LetterToRequestor,
                    AgentCompanyName = request.AgentCompanyName,
                    AgentFirstName = request.AgentFirstName,
                    AgentLastName = request.AgentLastName,
                    AgentAddress1 = request.AgentAddress1,
                    AgentAddress2 = request.AgentAddress2,
                    AgentCity = request.AgentCity,
                    AgentCounty = request.AgentCounty,
                    AgentPostCode = request.AgentPostCode,
                    AgentCountry = request.AgentCountry,
                    ContactEmail = request.ContactEmail,
                    Payments = payments
                };
            }
            else
            {
                return null;
            }
        }

        public async Task<RequestResponseModel> CreateRequestAsync(RequestModel requestModel)
        {
            var response = new RequestResponseModel() { IsSuccess = true, RequestNumber = requestModel.RequestNumber };
            try
            {
                var request = new Request
                {
                    RequestNumber = requestModel.RequestNumber,
                    SubjectFirstName = requestModel.SubjectFirstName,
                    SubjectLastName = requestModel.SubjectLastName,
                    AlternativeFirstName = requestModel.AlternativeFirstName,
                    AlternativeLastName = requestModel.AlternativeLastName,
                    DateOfBirth = requestModel.DateOfBirth,
                    DateOfDeath = requestModel.DateOfDeath,
                    CountryOfBirth = requestModel.CountryOfBirth,
                    ContactTitle = requestModel.ContactTitle,
                    ContactFirstName = requestModel.ContactFirstName,
                    ContactLastName = requestModel.ContactLastName,
                    ContactAddress1 = requestModel.ContactAddress1,
                    ContactAddress2 = requestModel.ContactAddress2,
                    ContactCity = requestModel.ContactCity,
                    ContactCounty = requestModel.ContactCounty,
                    ContactPostCode = requestModel.ContactPostCode,
                    ContactCountry = requestModel.ContactCountry,
                    LetterToRequestor = requestModel.LetterToRequestor,
                    AgentCompanyName = requestModel.AgentCompanyName,
                    AgentFirstName = requestModel.AgentFirstName,
                    AgentLastName = requestModel.AgentLastName,
                    AgentAddress1 = requestModel.AgentAddress1,
                    AgentAddress2 = requestModel.AgentAddress2,
                    AgentCity = requestModel.AgentCity,
                    AgentCounty = requestModel.AgentCounty,
                    AgentPostCode = requestModel.AgentPostCode,
                    AgentCountry = requestModel.AgentCountry,
                    ContactEmail = requestModel.ContactEmail
                };
                await _context.Set<Request>().AddAsync(request);
                await _context.SaveChangesAsync();
                response.RequestId = request.Id;
            }
            catch
            {
                response.IsSuccess = false;
                response.ErrorMessage = $"Error creating request for {requestModel.RequestNumber}";
            }
            return response;
        }

        public async Task<RequestResponseModel> UpdateRequestAsync(RequestModel requestModel)
        {
            var response = new RequestResponseModel() { IsSuccess = true, RequestNumber = requestModel.RequestNumber };

            try
            {
                var request = await _context.Set<Request>().FirstOrDefaultAsync(r => r.RequestNumber == requestModel.RequestNumber);

                if (request == null)
                {
                    response.IsSuccess = false;
                    response.ErrorMessage = $"No request found with the request number {requestModel.RequestNumber}";
                    return response;
                }

                _context.Attach(request);

                request.SubjectFirstName = requestModel.SubjectFirstName;
                request.SubjectLastName = requestModel.SubjectLastName;
                request.AlternativeFirstName = requestModel.AlternativeFirstName;
                request.AlternativeLastName = requestModel.AlternativeLastName;
                request.DateOfBirth = requestModel.DateOfBirth;
                request.DateOfDeath = requestModel.DateOfDeath;
                request.CountryOfBirth = requestModel.CountryOfBirth;
                request.ContactTitle = requestModel.ContactTitle;
                request.ContactFirstName = requestModel.ContactFirstName;
                request.ContactLastName = requestModel.ContactLastName;
                request.ContactAddress1 = requestModel.ContactAddress1;
                request.ContactAddress2 = requestModel.ContactAddress2;
                request.ContactCity = requestModel.ContactCity;
                request.ContactCounty = requestModel.ContactCounty;
                request.ContactPostCode = requestModel.ContactPostCode;
                request.ContactCountry = requestModel.ContactCountry;
                request.LetterToRequestor = requestModel.LetterToRequestor;
                request.AgentCompanyName = requestModel.AgentCompanyName;
                request.AgentFirstName = requestModel.AgentFirstName;
                request.AgentLastName = requestModel.AgentLastName;
                request.AgentAddress1 = requestModel.AgentAddress1;
                request.AgentAddress2 = requestModel.AgentAddress2;
                request.AgentCity = requestModel.AgentCity;
                request.AgentCounty = requestModel.AgentCounty;
                request.AgentPostCode = requestModel.AgentPostCode;
                request.AgentCountry = requestModel.AgentCountry;
                request.ContactEmail = requestModel.ContactEmail;

                await _context.SaveChangesAsync();
            }
            catch
            {
                response.IsSuccess = false;
                response.ErrorMessage = $"Error updating request for {requestModel.RequestNumber}";
            }
            return response;
        }
    }
}
