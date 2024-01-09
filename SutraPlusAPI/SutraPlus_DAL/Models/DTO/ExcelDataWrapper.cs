using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SutraPlus_DAL.Models.DTO
{
    public class ExcelDataWrapper
    {
        public LedgerDTO Ledger { get; set; }
        public BillSummaryDTO BillSummary { get; set; }
    }
}
