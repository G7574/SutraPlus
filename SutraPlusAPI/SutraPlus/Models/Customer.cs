using System;
using System.Collections.Generic;

namespace SutraPlus.Models
{
    public partial class Customer
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Code { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Pin { get; set; }
        public string? Mobile { get; set; }
        public string? Email { get; set; }
        public string? ContactPerson { get; set; }
        public string? Gstno { get; set; }
        public bool? IsActive { get; set; }
    }
}
