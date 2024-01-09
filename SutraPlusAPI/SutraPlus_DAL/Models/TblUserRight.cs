using System;
using System.Collections.Generic;

namespace SutraPlus_DAL.Models
{
    public partial class TblUserRight
    {
        public long Id { get; set; }
        public long? UserId { get; set; }
        public string? BtnName { get; set; }
        public string? BtnDescription { get; set; }
        public int? IsActive { get; set; }
    }
}
