using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySmartLockUI;

namespace MySmartLockWatchDogTester
{
    class Program
    {
        static void Main(string[] args)
        {
            var temp = OneTimePasswordUtil.EncodeName("Samer");
            Console.WriteLine(temp);

            Console.WriteLine(OneTimePasswordUtil.IsValidV2("Samer", 0, 5088020, 14507535));
            Console.WriteLine(OneTimePasswordUtil.IsValidV2("Jihad", 5, 7726068, 8156248));
        }
    }
}
