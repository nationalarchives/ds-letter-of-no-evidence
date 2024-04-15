using letter_of_no_evidence.model;

namespace letter_of_no_evidence.web.Service
{
    public interface IRequestService
    {
        Task<RequestResponseModel> CreateRequestAsync(RequestModel requestModel);
        Task<RequestResponseModel> UpdateRequestAsync(RequestModel requestModel);
        Task<RequestModel> GetRequestAsync(string requestNumber);
        Task AddNewPaymentAsync(PaymentModel paymentModel);
        Task<decimal> GetDeliveryCostAsync(int zoneNo);
    }
}
