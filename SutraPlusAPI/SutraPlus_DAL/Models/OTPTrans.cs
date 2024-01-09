using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SutraPlus_DAL.Models
{
    public partial class OTPTrans
    {
        public int Id { get; set; }
        public string CustomerId { get; set; }
        public string OTP { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ExpireDate { get; set; }
        public Boolean IsActive { get; set; }
    }
}
