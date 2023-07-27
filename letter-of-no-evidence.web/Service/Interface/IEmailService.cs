using letter_of_no_evidence.model;

namespace letter_of_no_evidence.web.Service
{
    public interface IEmailService
    {
        Task SendCustomerEmailAsync(RequestModel requestModel);
        Task SendD365EmailAsync(RequestModel requestModel);
    }
}
