using System;
using System.Collections.Generic;

namespace SutraPlus.Models
{
    public partial class UserSession
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string UserId { get; set; } = null!;
        public string? SessiondData { get; set; }
        public bool? IsActive { get; set; }
    }

    public class UserForm
    {
        public int FormId { get; set; }
        public string? GroupName { get; set; }
        public string? FormName { get; set; }
        public int UserId { get; set; }
        public bool IsAccess { get; set; }
    }
}
