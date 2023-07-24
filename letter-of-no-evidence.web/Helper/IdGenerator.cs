using System.Text;

namespace letter_of_no_evidence.web.Helper
{
    public static class IdGenerator
    {
        public static string TNAReferenceNumber()
        {
            string prefix = "TNA";
            DateTime dt = DateTime.Now;
            string time_stamp = dt.ToString("HHmmssffff");
            Random random = new Random();
            int r = random.Next(0, 26);
            Char letter = (Char)('A' + r);
            Random rnd = new Random();
            string suffix = letter + rnd.Next(10, 99).ToString();
            return prefix + time_stamp + suffix;
        }

        private static string CODE_OPTIONS = "QWRTYPDFHJKXCVNM";
        private static char TRANSACTION_IDENTIFIER = 'N';
        private static string year = DateTime.Now.ToString("yy");
        public static string GenerateSessionId(int orderId)
        {
            int paddedid = orderId + 1000000000;
            string stringtransid = paddedid.ToString();
            string padtransid = stringtransid.Substring(2, 8);
            string transNoCheckChar = string.Format("{0}/{1}/{2}", TRANSACTION_IDENTIFIER, year, padtransid);
            return string.Format("{0}{1}", transNoCheckChar, GenerateCheckChar(transNoCheckChar));
        }

        public static char TransactionIdentifier
        {
            get => TRANSACTION_IDENTIFIER;
            set
            {
                if (char.IsLetter(value))
                {
                    if (char.IsLower(value))
                    {
                        TRANSACTION_IDENTIFIER = char.ToUpperInvariant(value);
                    }
                    else
                    {
                        TRANSACTION_IDENTIFIER = value;
                    }
                }
            }
        }

        private static string GenerateCheckChar(string baseInput)
        {
            string result = default(string);
            bool isOdd = true;
            int sumOdd = 0;
            int sumEven = 0;
            foreach (var asciiChar in Encoding.ASCII.GetBytes(baseInput))
            {
                if (isOdd)
                {
                    sumOdd = sumOdd + asciiChar;
                    isOdd = false;
                }
                else
                {
                    sumEven = sumEven + asciiChar;
                    isOdd = true;
                }
            }
            result = CODE_OPTIONS.Substring(((sumOdd % 4) * 4 + (sumEven % 4)), 1);
            return result;
        }
    }
}
