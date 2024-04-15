using Microsoft.AspNetCore.Mvc.Rendering;

namespace letter_of_no_evidence.web.Service
{
    public interface IRecordCopyingService
    {
        Task<List<SelectListItem>> GetCountryAsListItem();
        Task<int> GetDeliveryZone(string country);
    }
}
