using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.ApiControllers.Contracts.V1.Responses
{
    public class SupplyItems { 
    public List<SupplyItem> Items { get; set; }
    }
    public class SupplyItem
    {
        public string id { get; set; }
        public string supplyId { get; set; }
        public string serialNo { get; set; }
        public string description { get; set; }
        public string quantityOrdered { get; set; }
        public string quantityDelivered { get; set; }
        public string remarks { get; set; }
        public string modifiedBy { get; set; }
    }
}