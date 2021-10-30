using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.Areas.Admin.Models
{
    public class DashboardViewModel
    {
        public int TotalConstruction { get; set; }
        public int TotalRenovation { get; set; }
        public int TotalSupply { get; set; }
        public int TotalProject { get; set; }
    }
}