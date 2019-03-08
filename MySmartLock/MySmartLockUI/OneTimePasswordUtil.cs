using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySmartLockUI
{
    public class OneTimePasswordUtil
    {
        private static readonly int[] bits = 
        {
            1, 2, 4, 8, 16, 32, 64
        };

        private static readonly long PrimeNumber = 15486719;

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

        public static bool IsValidV2(string userNanme, long code1, long code2)
        {
            if (code1 == 0 || code2 == 0 || string.IsNullOrEmpty(userNanme))
                return false;
            
            long userCode = EncodeName(userNanme);

            int len = userNanme.Length;

            // get time hout
            long datetime = long.Parse(DateTime.Now.ToString("yyyyMMddHH"));

            long temp = len + datetime + userCode + code1;
            long verification = temp % PrimeNumber;

            return verification == code2;
        }

        public static bool IsMasterCodeV2(string userName, long passCode, long verificationNumber)
        {
            if (!IsValidV2(userName, passCode, verificationNumber))
            {
                return false;
            }

            return userName.Equals("Samer");
        }

    }
}
