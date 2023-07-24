using System.Net;
using letter_of_no_evidence.web.Models;

namespace letter_of_no_evidence.web.Service
{
    public class PaymentService : IPaymentService
    {
        private readonly HttpClient _client;
        public PaymentService(HttpClient client)
        {
            _client = client;
        }

        public async Task<PaymentResponseModel> CreateNewPayment(PaymentRequestModel requestModel)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            var response = await _client.PostAsJsonAsync("", requestModel);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<PaymentResponseModel>();
            return result;
        }

        public async Task<PaymentResponseModel> GetPaymentById(string paymentId)
        {
            var url = $"{paymentId}";
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            var response = await _client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<PaymentResponseModel>();
            return result;
        }
    }
}
