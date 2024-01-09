using System;
using System.Collections.Generic;

namespace SutraPlus_DAL.Models
{
    public partial class User
    {
        public int? Id { get; set; }
        public int? UserId { get; set; }
        public string? UserType { get; set; }
        public string? UserName { get; set; } = null!;
        public string? Password { get; set; } = null!;
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? PhoneNo { get; set; }
        public bool? IsActive { get; set; }
    }
}
