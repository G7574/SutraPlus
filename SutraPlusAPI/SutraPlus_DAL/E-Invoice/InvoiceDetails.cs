using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SutraPlus_DAL.Models
{
    public class InvoiceDetails
    {
        public string Status { get; set; }
        public string IRN { get; set; }
        public string InoiceNumber { get; set; }
        public string AckNo { get; set; }
        public string SignedQR { get; set; }
    }
}
