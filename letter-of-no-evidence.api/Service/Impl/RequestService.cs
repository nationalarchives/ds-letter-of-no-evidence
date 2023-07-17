using letter_of_no_evidence.data;
using letter_of_no_evidence.domain;
using letter_of_no_evidence.model;
using Microsoft.EntityFrameworkCore;
using System.CodeDom;

namespace letter_of_no_evidence.api.Service
{
    public class RequestService : IRequestService
    {
        private readonly LONEDBContext _context;
        public RequestService(LONEDBContext context)
        {
            _context = context;
        }

        public async Task<Request> GetRequestByIdAsync(int requestId)
        {
            throw new NotImplementedException();
        }
    }
}
