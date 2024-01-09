using System;
using System.Collections.Generic;

namespace SutraPlus_DAL.Models
{
    public partial class UserSession
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string UserId { get; set; } = null!;
    }
}
