using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SutraPlus_DAL.Common
{
    public static  class GetOTP
    {
        public static string Get_OTP()
        {
            Random rnd = new Random();
            return (rnd.Next(100000, 999999)).ToString();
        }
    }
}
