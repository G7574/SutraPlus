using System;
using System.Collections.Generic;

namespace SutraPlus.Models
{
    public partial class StateMaster
    {
        public int Id { get; set; }
        public string? Statename { get; set; }
        public string? Statecode { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public bool? IsActive { get; set; }
    }
}
