using System.Globalization;

namespace letter_of_no_evidence.web.Helper
{
    public static class CostExtension
    {
        private static CultureInfo cultureInfo = new CultureInfo("en-GB");
        public static string LoneCost()
        {
            var amount = Decimal.Divide(int.Parse(Environment.GetEnvironmentVariable("LONE_Amount")), 100);
            return amount.ToString("C", cultureInfo);
        }

        public static string FormatedCost(decimal cost)
        {
            return cost.ToString("C", cultureInfo);
        }

        public static string FormatedTotalCost(decimal cost)
        {
            var amount = Decimal.Divide(int.Parse(Environment.GetEnvironmentVariable("LONE_Amount")), 100) + cost;
            return amount.ToString("C", cultureInfo);
        }
    }
}
