using Project.DAL;
using Project.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Project.Areas.Admin.Models
{
    public class ProjectViewModel
    {
        public List<ProjectApplication> projectList { get; set; }
        public Workflow workflow { get; set; }
        public bool AllPhotoUploaded { get; set; }
        public ProjectApplication project { get; set; }
        public List<PaymentType> AvailablePayment { get; set; }
        public List<Payment> ProjectPaymentList { get; set; }
        public List<IntegerSelectListItem> ContractorList { get; set; }
        public List<IntegerSelectListItem> StateList { get; set; }
      
        public List<IntegerSelectListItem> LgaList { get; set; }
        public List<IntegerSelectListItem> PaymentTypeList { get; set; }
        public List<IntegerSelectListItem> DocumentTypeList { get; set; }
        public ProjectDetailForm projectForm { get; set; }
        public ProjectPaymentForm  projectPaymentForm { get; set; }

        public List<DocumentType> AvailableDocument { get; set; }
        public List<DocumentInfo> DocumentInfoList { get; set; }
        public string FullPhotoPath { get; set; }
        public DocumentForm documentForm { get; set; }
    }

    public class ProjectDetailForm
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter the serial No")]
        public string SerialNo { get; set; }
        [Required(ErrorMessage = "Please enter the description")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Please enter the location")]
        public string Location { get; set; }
        [Required(ErrorMessage = "Please enter the coordinate")]
        public string Coordinate { get; set; }
        [Required(ErrorMessage = "Please select the State")]
        public int StateId { get; set; }

        [Required(ErrorMessage = "Please select the local government")]
        public int LGAId { get; set; }


        [Required(ErrorMessage = "Please select the Contractor")]
        public int ContractorId { get; set; }

        [Required(ErrorMessage = "Please enter the Contract Sum")]
        public decimal ContractSum { get; set; }

        [Required(ErrorMessage = "Please enter the State Of Completion")]
        public string StageOfCompletion { get; set; }

        [Required(ErrorMessage = "Please enter the Description Of Completion")]
        public string DescriptionOfCompletion { get; set; }

        [Required(ErrorMessage = "Please enter the Project Quality")]
        public string ProjectQuality { get; set; }
        [Required(ErrorMessage = "Project has defect? please indicate")]
        public bool HasDefect { get; set; }
        public string TransactionId { get; set; }
        public string Status { get; set; }
        public int workflowId { get; set; }
        public string DescriptionOfDefect { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class ProjectPaymentForm
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Please select payment type")]
        public int PaymentTypeId { get; set; }

        [Required(ErrorMessage = "Please select payment type")]
        public decimal ProjectSum { get; set; }

    }

    public class DocumentForm
    {
        public int Id { get; set; }
        
        [Required]       
        public HttpPostedFileBase Photo { get; set; }
       
        [Required(ErrorMessage = "Please select Photo type")]
        public int DocumentTypeId { get; set; }

    }
}