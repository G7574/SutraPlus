using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SutraPlus.Models
{
    public class CompanyModel
    {
        public int CompanyId { get; set; }
        public string? CompanyName { get; set; }
        public string? AddressLine1 { get; set; }
        public string? AddressLine2 { get; set; }
        public string? AddressLine3 { get; set; }
        public string Place { get; set; } = null!;
        public string? Gstin { get; set; }
        public string? ContactDetails { get; set; }       
    }
}
