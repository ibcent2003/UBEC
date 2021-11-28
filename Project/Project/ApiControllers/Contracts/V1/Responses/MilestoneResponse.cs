using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.ApiControllers.Contracts.V1.Responses
{
    public class MilestoneResponse: StatusMessage
    {
        public List<Milestone> Records { get; set; }
    }

    public class Milestone
    {
        public int? ProjectType { get; set; }
        public string Percentage { get; set; }
        public string Description { get; set; }
    }
}