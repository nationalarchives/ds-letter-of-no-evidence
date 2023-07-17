using letter_of_no_evidence.domain;

namespace letter_of_no_evidence.api.Service
{
    public interface IRequestService
    {
        Task<Request> GetRequestByIdAsync(int requestId);
    }
}
