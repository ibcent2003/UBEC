using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.Areas.Admin.Models
{
    public class DashboardViewModel
    {
        public int TotalConstructionSubmitted { get; set; }
        public int TotalRenovationSubmitted { get; set; }
        public int TotalSupplySubmitted { get; set; }
        public int TotalSubmitted { get; set; }
    }
}