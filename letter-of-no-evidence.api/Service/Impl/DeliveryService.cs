
using letter_of_no_evidence.data;
using Microsoft.EntityFrameworkCore;

namespace letter_of_no_evidence.api.Service
{
    public class DeliveryService : IDeliveryService
    {
        private readonly LONEDBContext _context;
        public DeliveryService(LONEDBContext context)
        {
            _context = context;
        }
        public async Task<decimal> GetDeliveryCost(int zoneNo)
        {
            var result = await _context.DeliveryZoneCost.FirstOrDefaultAsync(z => z.ZoneNo == zoneNo);
            return result.CostWithTracking;
        }
    }
}
