using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SutraPlus_DAL.Models
{
    public class UserAccesMaster
    {
        public int? Id { get; set; }
        public int? FormId { get; set; }
        public int? UserId { get; set; }
        public bool? IsAccess { get; set; }
        public int? PreparedBy { get; set; }
        public DateTime? PreparedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public Boolean? IsActive { get; set; }
    }
}
