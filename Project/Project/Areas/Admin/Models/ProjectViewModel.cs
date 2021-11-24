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
        public List<IntegerSelectListItem> StageOfCompletionList { get; set; }
        public int StageOfCompletionId { get; set; }
        public bool ReportOwner { get; set; }
        public List<DeliverableFormat> AvailableFormat { get; set; }
        public List<IntegerSelectListItem> deliverableTypeList { get; set; }
        public int DeliverableTypeId { get; set; }
        public List<IntegerSelectListItem> deliverableFormatList { get; set; }
        public System.Web.Mvc.SelectList InspectionUser { get; set; }
        public string username { get; set; }
        public List<DeliverableType> deliverableType { get; set; }

        public DeliverableType deliverable { get; set; }

        public List<ProjectDeliverable> ProjectDeliverableList { get; set; }

        public Users user { get; set; }
        public bool HasAssigned { get; set; }
        public List<UserDetail> userDetail { get; set; }
        public List<Payment> paymentlist { get; set; }
        public List<ProjectApplication> projectList { get; set; }
        public Workflow workflow { get; set; }
        public Inspection inspection { get; set; }
        public bool AllPhotoUploaded { get; set; }
        public ProjectApplication project { get; set; }
        public List<PaymentType> AvailablePayment { get; set; }
        public List<Payment> ProjectPaymentList { get; set; }
        public List<IntegerSelectListItem> ContractorList { get; set; }
        public List<IntegerSelectListItem> StateList { get; set; }
        public List<IntegerSelectListItem> ProjectTypeList { get; set; }
        public List<IntegerSelectListItem> StageOfCompletion { get; set; }
        public int StateId { get; set; }
        public List<IntegerSelectListItem> LgaList { get; set; }
        public List<IntegerSelectListItem> PaymentTypeList { get; set; }
        public List<IntegerSelectListItem> DocumentTypeList { get; set; }
        public ProjectDetailForm projectForm { get; set; }
        public ProjectPaymentForm  projectPaymentForm { get; set; }

        public List<DocumentType> AvailableDocument { get; set; }
        public List<DocumentInfo> DocumentInfoList { get; set; }
        public List<Inspection> InspectionList { get; set; }
        public string FullPhotoPath { get; set; }
        public DocumentForm documentForm { get; set; }
        public InspectionForm inspectionForm { get; set; }
        public ProjectDeliverableForm projectDeliverableForm { get; set; }
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

        [Required(ErrorMessage = "Please select the Project Type")]
        public int ProjectTypeId { get; set; }

        public string TransactionId { get; set; }
        public string Status { get; set; }
        public int workflowId { get; set; }
        public string DescriptionOfDefect { get; set; }
        public bool IsDeleted { get; set; }
        public bool ShowCost { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
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

    public class InspectionForm
    {
        public int Id { get; set; }
       
        [Required(ErrorMessage = "Please enter the location")]
        public string Location { get; set; }
        [Required(ErrorMessage = "Please enter the coordinate")]
        public string Coordinate { get; set; }
    
       
        [Required(ErrorMessage = "Please select the local government")]
        public int LGAId { get; set; }       
        [Required(ErrorMessage = "Please enter the State Of Completion")]
        public int StageOfCompletionId { get; set; }
        
        //public string DescriptionOfCompletion { get; set; }
        
        public string ProjectQuality { get; set; }
       
        public bool HasDefect { get; set; }
        public string DescriptionOfDefect { get; set; }
        public  int ProjectId { get; set; }
 
    }

    public class ProjectDeliverableForm
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Please select deliverable type")]
        [Display(Name = "Deliverable Type")]
        public int DeliverableId { get; set; }

        [Required(ErrorMessage = "Please select payment type")]
        [Display(Name = "Deliverable Name")]
        public int DeliverableFormatId { get; set; }

        [Required(ErrorMessage = "Please enter deliverable unit ")]
        [Display(Name = "Deliverable Unit")]
        public int DeliverableUnit { get; set; }

        [Required(ErrorMessage = "Please enter Remarks ")]
        [Display(Name = "Remarks")]
        public string Remarks { get; set; }

    }
}