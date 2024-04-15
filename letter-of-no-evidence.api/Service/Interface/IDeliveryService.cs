namespace letter_of_no_evidence.api.Service
{
    public interface IDeliveryService
    {
        Task<decimal> GetDeliveryCost(int zoneNo);
    }
}
