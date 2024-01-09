using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SutraPlus_DAL.Models
{
    public class TmpPaymentList
    {
        public int Id { get; set; }
        public int? CompanyId { get; set; }
        public int? LedgerID { get; set; }
        public DateTime? AsOnDate { get; set; }
        public decimal? YadiBalance { get; set; }
        public decimal? AccountBalance { get; set; }
    }
}
