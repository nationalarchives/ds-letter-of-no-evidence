using System.Globalization;

namespace letter_of_no_evidence.web.Helper
{
    public static class CostExtension
    {
        public static string LoneCost()
        {
            var amount = Decimal.Divide(int.Parse(Environment.GetEnvironmentVariable("LONE_Amount")), 100);
            return amount.ToString("C", CultureInfo.CurrentCulture);
        }

        public static string FormatedCost(decimal cost)
        {
            return cost.ToString("C", CultureInfo.CurrentCulture);
        }

        public static string FormatedTotalCost(decimal cost)
        {
            var amount = Decimal.Divide(int.Parse(Environment.GetEnvironmentVariable("LONE_Amount")), 100) + cost;
            return amount.ToString("C", CultureInfo.CurrentCulture);
        }
    }
}
