using System;
using System.Collections.Generic;

namespace SutraPlus.Models
{
    public partial class CustomerFinancialYear
    {
        public int Id { get; set; }
        public string CustomerId { get; set; } = null!;
        public string? Year { get; set; }
        public string? Description { get; set; }
        public string? ServerName { get; set; }
        public string? ServerUserId { get; set; }
        public string? ServerPassword { get; set; }
        public string? DatabaseUri { get; set; }
        public string? ThemeCode { get; set; }
        public string? BackCode { get; set; }
        public string? WebUrl { get; set; }
        public bool? IsActive { get; set; }
    }
}
