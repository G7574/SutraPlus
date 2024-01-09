using System;
using System.Collections.Generic;

namespace SutraPlus_DAL.Models
{
    public partial class CustomerFinancialYear
    {
        public int Id { get; set; }
        public string CustomerId { get; set; }
        public string? Year { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? Description { get; set; }
        public string? DatabaseUri { get; set; }
        public string ThemeCode { get; set; }
        public string? BackCode { get; set; }
        public string? WebUrl { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool? IsActive { get; set; }
        //for entity framework default value should no taking hence this will help to do so
        public CustomerFinancialYear()
        {
            //this.CreatedDate = DateTime.Now;
            this.UpdatedDate = DateTime.Now;
        }
    }
}
