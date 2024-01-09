using System;
using System.Collections.Generic;

namespace SutraPlus_DAL.Models
{
    public partial class State
    {
        public long? Fid { get; set; } = null!;
        public string? Statename { get; set; } = null!;
        public string? Statecode { get; set; } = null!;
        public DateTime? CreatedDate { get; set; } = null!;
        public DateTime? UpdatedDate { get; set; } = null!;
        public int? CreatedBy { get; set; } = null!;
        public int? Id { get; set; } = null!;
        public int? IsActive { get; set; } = null!;
    }
}
