using System;
using System.Collections.Generic;

namespace SutraPlus_DAL.Models
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
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? GSTNo { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public Boolean? IsActive { get; set; }
        public Customer()
        {
            //CreatedDate = DateTime.Now;
            UpdatedDate = DateTime.Now;

        }

    }
}
