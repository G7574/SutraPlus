using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SutraPlus_DAL.Models
{
    public  class ThemCode
    {
        public int Id { get; set; }
        public string Year { get; set; }
        public string ThemeCode { get; set; }
        public string BackCode { get; set; }
        public Boolean IsActive { get; set; }
    }
}
