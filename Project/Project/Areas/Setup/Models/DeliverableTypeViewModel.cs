using Project.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Project.Areas.Setup.Models
{
    public class DeliverableTypeViewModel
    {
        public List<DeliverableFormat> AvailableDeliverableFormat { get; set; }
        public DeliverableType deliverableType { get; set; }
        public int DeliverableFormatId { get; set; }

        public List<DeliverableType> DeliverableTypeList { get; set; }
        public List<DeliverableFormat> DeliverableFormatList { get; set; }

        public DeliverableFormatForm DeliverableFormatform { get; set; }
        public DeliverableTypeForm DeliverableTypeform { get; set; }
    }

    public class DeliverableFormatForm
    {
        public int Id { get; set; }     
        [Required(ErrorMessage = "Please enter deliverable name")]
        [Display(Name = "Deliverable Name")]
        public string Name { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class DeliverableTypeForm
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Please enter deliverable type")]
        [Display(Name = "Deliverable Type")]
        public string Name { get; set; }
        public bool IsDeleted { get; set; }
    }
}