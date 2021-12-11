using Project.DAL;
using Project.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Project.Areas.Admin.Models
{
    public class SupplyViewModel
    {

        public Users user { get; set; }
        public DeliverableType deliverable { get; set; }
        public List<SupplyDeliverable> SupplyDeliverableList { get; set; }
        public List<IntegerSelectListItem> deliverableFormatList { get; set; }

        public List<DeliverableType> deliverableType { get; set; }
        public List<IntegerSelectListItem> deliverableTypeList { get; set; }
        public List<DocumentInfo> DocumentInfoList { get; set; }
        public bool AllPhotoUploaded { get; set; }
        public List<DocumentType> AvailableDocument { get; set; }
        public List<IntegerSelectListItem> ContractorList { get; set; }
        public List<IntegerSelectListItem> StateList { get; set; }
        public string FullPhotoPath { get; set; }
        public int StateId { get; set; }
        public List<IntegerSelectListItem> LgaList { get; set; }
        public List<Supplies> SupplyList { get; set; }
        public List<SupplyItems> SupplyItemList { get; set; }
        public Supplies Supply { get; set; }
        public Workflow workflow { get; set; }
        public SupplyForm supplyForm { get; set; }
        public DocumentForm documentForm { get; set; }
        public SupplyItemsForm SupplyItemsform { get; set; }
        public ProjectDeliverableForm projectDeliverableForm { get; set; }
    }

    public class SupplyForm
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter Serial No")]
        [Display(Name = "SerialNo")]
        public string SerialNo { get; set; }

        [Required(ErrorMessage = "Please select LGA")]
        [Display(Name = "LGA")]
        public int LGAId { get; set; }

        [Required(ErrorMessage = "Please enter location")]
        [Display(Name = "Location")]
        public string Location { get; set; }

        [Required(ErrorMessage = "Please select contractor ")]
        [Display(Name = "Contractor")]
        public int ContractorId { get; set; }

        [Required(ErrorMessage = "Please enter verification Date ")]
        [Display(Name = "Verification Date")]
        public DateTime VerificationDate { get; set; }

        [Required(ErrorMessage = "Please select verification Officer ")]
        [Display(Name = "Verification Officer")]
        public string VerificationOfficer { get; set; }

        [Required(ErrorMessage = "Please enter representative name ")]
        [Display(Name = "Representative Name")]
        public string RepresentativeName { get; set; }

        [Required(ErrorMessage = "Please enter representative Designation ")]
        [Display(Name = "Representative Designation")]
        public string RepresentativeDesignation { get; set; }


        [Required(ErrorMessage = "Please enter representative Phone Number ")]
        [Display(Name = "Representative Phone Number")]
        public string RepresentativePhoneNumber { get; set; }

        [Display(Name = "Coordinate")]
        public string Coordinate { get; set; }

    }

    public class SupplyItemsForm
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter Serial No")]
        [Display(Name = "SerialNo")]
        public string SerialNo { get; set; }

        
        [Required(ErrorMessage = "Please enter Description")]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Please enter Quantity Ordered ")]
        [Display(Name = "Quantity Ordered")]
        public int? QuantityOrdered { get; set; }

        [Required(ErrorMessage = "Please enter Quantity Delivered ")]
        [Display(Name = "Quantity Delivered")]
        public int? QuantityDelivered { get; set; }

        [Required(ErrorMessage = "Please enter Remarks ")]
        [Display(Name = "Remarks")]
        public string Remarks { get; set; }

       


    }
}