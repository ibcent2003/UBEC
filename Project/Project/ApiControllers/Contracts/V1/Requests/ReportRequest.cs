using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.ApiControllers.Contracts.V1.Requests
{
    public class ReportRequest
    {

        public int Id { get; set; }
        public Guid TransactionId { get; set; }
        public int ProjectId { get; set; }
        //public int ContractorId { get; set; }
        //public int workflowId { get; set; }
        //public string SerialNo { get; set; }
        //public string Description { get; set; }
        public string Location { get; set; }
        public string Coordinate { get; set; }
        public int LGAId { get; set; }
        public string StageOfCompletion { get; set; }
        public string DescriptionOfCompletion { get; set; }
        public string ProjectQuality { get; set; }
        public bool HasDefect { get; set; }
        public string DescriptionOfDefect { get; set; }
        public decimal ContractSum { get; set; }
        public string Status { get; set; }
        public DateTime InspectionDate { get; set; }
        //public bool IsDeleted { get; set; }
        public string Modifiedby { get; set; }

        public byte[] Attachment1 { get; set; }
        public byte[] Attachment2 { get; set; }

    }
}