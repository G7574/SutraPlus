using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SutraPlus_DAL.Models
{
    public class FormMaster
    {
        public int Id { get; set; }
        public int FormId { get; set; }
        public string GroupName { get; set; }
        public string FormName { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public Boolean IsActive { get; set; }

    }
}
