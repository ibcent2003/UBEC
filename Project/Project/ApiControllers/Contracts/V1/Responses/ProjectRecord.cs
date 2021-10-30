using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.ApiControllers.Contracts.V1.Responses
{
    public class ProjectRecord
    {
        public int ProjectId { get; set; }
        public string SerialNo { get; set; }
        public string ProjectType { get; set; }
        public string Contractor { get; set; }
        public string Description { get; set; }
        public string OwnedBy { get; set; }
    }
}