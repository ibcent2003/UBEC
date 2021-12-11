using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.ApiControllers.Contracts.V1.Requests
{
    public class SupplyRequest
    {
        public string RepresentativePhoneNumber { get; set; }
        public string RepresentativeDesignation { get; set; }
        public string Representative { get; set; }
        public string VerificationOfficer { get; set; }
        public DateTime? VerificationDate { get; set; }
        public string Contractor { get; set; }
        public string Location { get; set; }
        public string LGAId { get; set; }
        public string ModifiedBy { get; set; }
        //public List<SupplyItem> SupplyItems { get; set; }
    }

    //public class SupplyItem
    //{
    //    public string SerialNumber { get; set; }
    //    public string Description { get; set; }
    //    public string QuantityOrdered { get; set; }
    //    public string QuantityDelivered { get; set; }
    //    public string Remarks { get; set; }
    //}
}