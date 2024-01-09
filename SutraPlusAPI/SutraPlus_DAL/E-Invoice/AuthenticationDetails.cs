using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SutraPlus_DAL.Models
{
    public class AuthenticationDetails
    {
        public string AuthenticationToken { get; set; }
        public string AuthenticationResponseData { get; set; }
        public string InvoiceResponse { get; set; }
    }
}
