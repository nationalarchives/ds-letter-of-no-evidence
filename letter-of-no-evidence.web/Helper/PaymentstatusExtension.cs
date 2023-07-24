using letter_of_no_evidence.model;

namespace letter_of_no_evidence.web.Helper
{
    public static class PaymentstatusExtension
    {
        public static PaymentStatus ToPaymentStatus(this string status)
        {
            switch (status?.ToLower())
            {
                case "created":
                    return PaymentStatus.Created;
                case "started":
                    return PaymentStatus.Started;
                case "submitted":
                    return PaymentStatus.Submitted;
                case "capturable":
                    return PaymentStatus.Capturable;
                case "success":
                    return PaymentStatus.Success;
                case "failed ":
                    return PaymentStatus.Failed;
                case "cancelled":
                    return PaymentStatus.Cancelled;
                default: // "error"
                    return PaymentStatus.Error;
            }
        }
    }
}
