using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SutraPlus_DAL.Models.DTO
{
    public class LedgerDTO
    {
        public long CompanyId { get; set; }
        public long? LedgerId { get; set; }
        public string? LedgerName { get; set; }
        public string? GSTIN { get; set; }
        public string? Address1 { get; set; }
        public string? Address2 { get; set; }
        public string? EmailId { get; set; }
        public string? CellNo { get; set; }
        public string? Place { get; set; }
        public string? Pin { get; set; }
    }
}
