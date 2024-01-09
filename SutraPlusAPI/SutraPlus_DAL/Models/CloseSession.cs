using System;
using System.Collections.Generic;

namespace SutraPlus_DAL.Models
{
    public partial class CloseSession
    {
        public long Companyid { get; set; }
        public DateTime ClosedDate { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
