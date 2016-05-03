using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShortenMe.Services.Contracts.Models
{
    public class LinkAnalyticsModel
    {
        public int TotalHits { get; set; }
        public Tuple<DateTime, int>[] TotalHitsInLast7Days { get; set; }
        public int[] TotalHitsByBrowsers { get; set; }
    }
}
