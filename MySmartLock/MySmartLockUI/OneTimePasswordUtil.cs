using System;
using System.Linq;

namespace MySmartLockUI
{
    using System.Windows.Forms.Layout;

    public class OneTimePasswordUtil
    {
        private static readonly int[] bits = 
        {
            1, 2, 4, 8, 16, 32, 64
        };

        private static readonly int PrimeNumber = 15486719;
        private static Random rnd = new Random();

        public static int RandomCode1()
        {
            return rnd.Next(PrimeNumber);
        }

        public static bool IsValidV1(string userNanme, string passCode)
        {
            return passCode.IndexOf('3') == 2;
        }

        public static bool IsMasterCodeV1(string userName, string passCode)
        {
            if (string.IsNullOrEmpty(passCode) || passCode.Length == 1)
            {
                return false;
            }

            return passCode.Last() == passCode.First();
        }

        public static long EncodeName(string userName)
        {
            string code = "0";
            int sum = 0;

            foreach (var c in userName.ToUpper())
            {
                var d = c - 64;
                sum += d;
            }

            foreach (var bit in bits)
            {
                var bitAnd = sum & bit;
                code += bitAnd;
            }

            return long.Parse(code);
        }

        public static bool IsValidV2(string userNanme, int minutes, long code1, long code2)
        {
            if (code1 == 0 || code2 == 0 || string.IsNullOrEmpty(userNanme))
                return false;
            
            long userCode = EncodeName(userNanme);
            
            // get time hout
            long datetime = long.Parse(DateTime.Now.ToString("yyyyMMddHH"));

            long temp = minutes * datetime + userCode + code1;
            long verification = temp % PrimeNumber;

            return verification == code2;
        }

        public static bool IsMasterCodeV2(string userName, long passCode, long verificationNumber)
        {
            if (!userName.Equals("Samer", StringComparison.InvariantCultureIgnoreCase))
            {
                return false;
            }

            return IsValidV2(userName, 1, passCode, verificationNumber);
        }

    }
}
