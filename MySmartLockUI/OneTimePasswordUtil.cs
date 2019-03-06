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
            1, 2, 4, 6, 8, 16, 32, 64
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
            int len = userName.Length;

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


            long bitcoding = long.Parse(code);

            long res = bitcoding * len + len;


            // get time hout
            long datetime = long.Parse(DateTime.Now.ToString("yyyyMMddhh"));
            res = res * datetime + datetime;

            return res;
        }

        public static bool IsValidV2(string userNanme, long passCode, long verificationNumber)
        {
            if (passCode == 0 || verificationNumber == 0 || string.IsNullOrEmpty(userNanme))
                return false;
            
            long userCode = EncodeName(userNanme);

            long verification = (userCode - passCode) % PrimeNumber;
            return verification == verificationNumber;
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
