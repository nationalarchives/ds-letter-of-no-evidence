﻿using letter_of_no_evidence.model;

namespace letter_of_no_evidence.api.Service
{
    public interface IRequestService
    {
        Task<RequestModel> GetRequestByNumberAsync(string requestNumber);
        Task<RequestResponseModel> CreateRequestAsync(RequestModel requestModel);
        Task<RequestResponseModel> UpdateRequestAsync(RequestModel requestModel);
    }
}
