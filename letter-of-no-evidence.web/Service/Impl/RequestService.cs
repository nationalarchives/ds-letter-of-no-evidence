using letter_of_no_evidence.model;
using System.Net;

namespace letter_of_no_evidence.web.Service
{
    public class RequestService : IRequestService
    {
        private readonly HttpClient _client;
        public RequestService(HttpClient client)
        {
            _client = client;
        }
        public async Task<RequestResponseModel> CreateRequestAsync(RequestModel requestModel)
        {
            var response = await _client.PutAsJsonAsync("request/create", requestModel);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<RequestResponseModel>();
            return result;
        }

        public async Task<RequestResponseModel> UpdateRequestAsync(RequestModel requestModel)
        {
            var response = await _client.PostAsJsonAsync("request/update", requestModel);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<RequestResponseModel>();
            return result;
        }

        public async Task<RequestModel> GetRequestAsync(string requestNumber)
        {
            var response = await _client.GetAsync($"request/getrequest/{requestNumber}");
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<RequestModel>();
            return result;
        }

        public async Task AddNewPaymentAsync(PaymentModel paymentModel)
        {
            var response = await _client.PutAsJsonAsync("payment/create", paymentModel);
            response.EnsureSuccessStatusCode();
        }
    }
}
