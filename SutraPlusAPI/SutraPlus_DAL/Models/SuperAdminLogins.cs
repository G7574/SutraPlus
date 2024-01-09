using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SutraPlus_DAL.Models
{
    public partial class SuperAdminLogins
    {
        public int Id { get; set; }
        public string UserName { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string UserType { get; set; } = null!;
        public Boolean IsActive { get; set; } 
        public string PreparedBy { get; set; } = null!;
        public DateTime PreparedDate { get; set; }
    }
}
