using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SutraPlus.Models
{
    public class Sale
    {
        public string? CommodityName { get; set; }
        public decimal Amount { get; set; }
    }
}
