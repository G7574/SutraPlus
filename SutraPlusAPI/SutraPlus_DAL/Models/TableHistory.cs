using System;
using System.Collections.Generic;

namespace SutraPlus_DAL.Models
{
    public partial class TableHistory
    {
        public Guid Id { get; set; }
        public string TableName { get; set; } = null!;
        public string CrudType { get; set; } = null!;
        public DateTime TransDate { get; set; }
        public string Remark { get; set; } = null!;
    }
}
