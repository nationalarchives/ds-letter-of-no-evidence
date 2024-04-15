using letter_of_no_evidence.web.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Caching.Memory;

namespace letter_of_no_evidence.web.Service
{
    public class RecordCopyingService : IRecordCopyingService
    {
        public HttpClient _client { get; }
        private IMemoryCache _cache;

        public RecordCopyingService(HttpClient client, IMemoryCache memoryCache)
        {
            _client = client;
            _cache = memoryCache;
        }
        public async Task<List<SelectListItem>> GetCountryAsListItem()
        {
            var results = await GetCountry();
            return results.Select(x => new SelectListItem { Text = x.Description, Value = x.Description }).ToList();
        }

        public async Task<int> GetDeliveryZone(string country)
        {
            var results = await GetCountry();
            return results.FirstOrDefault(c => c.Description == country).ZoneNo;
        }

        private async Task<List<CountryModel>> GetCountry()
        {
            if (_cache.TryGetValue("Countries", out List<CountryModel>? countries))
            {
                return countries;
            }
            var response = await _client.GetAsync("getcountry");
            response.EnsureSuccessStatusCode();
            countries = await response.Content.ReadFromJsonAsync<List<CountryModel>>();

            _cache.Set("Countries", countries, DateTime.Today.AddDays(1));
            return countries;
        }
    }
}
