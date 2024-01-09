using System;
using System.Collections.Generic;

namespace SutraPlus.Models
{
    public partial class SuperAdminLogin
    {
        public int Id { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public string? UserType { get; set; }
        public bool? IsActive { get; set; }
        public string? PreparedBy { get; set; }
        public DateTime? PreparedDate { get; set; }
    }
}
