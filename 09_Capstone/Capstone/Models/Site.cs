using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.Models
{
    public class Site
    {
        public int Id { get; set; }
        public int CampgroundId { get; set; }
        public int SiteNumber { get; set; }
        public int MaxOccupancy { get; set; }
        public bool IsAccessible { get; set; }
        public int MaxRVLength { get; set; }
        public bool HasUtilities { get; set; }
        public decimal DailyFee { get; set; }
    }
}
