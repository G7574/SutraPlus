using System;
using System.Collections.Generic;

namespace SutraPlus_DAL.Models
{
    public partial class ChequeSetting
    {
        public long LedgerId { get; set; }
        public double? NamefromLeft { get; set; }
        public double? NameFromTop { get; set; }
        public double? DatefromTop { get; set; }
        public double? DatefirstdigitFromLeft { get; set; }
        public double? DateSeconddigitFromLeft { get; set; }
        public double? DateThirddigitFromLeft { get; set; }
        public double? DateFourthdigitFromLeft { get; set; }
        public double? DateFivedigitFromLeft { get; set; }
        public double? DateSixthdigitFromLeft { get; set; }
        public double? DateSeventhdigitFromLeft { get; set; }
        public double? DateEighthdigitFromLeft { get; set; }
        public double? InwordsFromLeft { get; set; }
        public double? InwordsFromTop { get; set; }
        public double? DigitsFromLeft { get; set; }
        public double? DigitsFromTop { get; set; }
    }
}
