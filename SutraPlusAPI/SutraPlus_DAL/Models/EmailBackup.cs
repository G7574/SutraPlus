using System;
using System.Collections.Generic;

namespace SutraPlus_DAL.Models
{
    public partial class EmailBackup
    {
        public long Id { get; set; }
        public string? BackupName { get; set; }
        public DateTime TranctDate { get; set; }
    }
}
