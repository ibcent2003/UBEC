using Project.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Project.Models
{
    public class IndexViewModel
    {
        public List<News> NewsList { get; set; }
        public string PicturePath { get; set; }
        public string apkfileName { get; set; }
        public News news { get; set; }
        public ProjectApplication project { get; set; }
        public List<ProjectApplication> projectList { get; set; }
        public List<Inspection> inspectionlist { get; set; }
        public Workflow workflow { get; set; }
        public List<Workflow> ProjectTypes { get; set; }
        public bool EnableSum { get; set; }
        public List<IntegerSelectListItem> StateList { get; set; }
        public int StateId { get; set; }
        public List<IntegerSelectListItem> LgaList { get; set; }
        public List<Supplies> SupplyList { get; set; }

        public List<SupplyItems> supplyItems { get; set; }
        public Supplies Supply { get; set; }
        public FeedbackForm feedbackForm { get; set; }
        public SerachForm serachForm { get; set; }



    }

    public class FeedbackForm
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter the Full-Name")]
        [Display(Name = "Full-Name")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Please enter the Email Address")]
        [Display(Name = "Email Address")]
        public string EmailAddress { get; set; }

        [Required(ErrorMessage = "Please enter Mobile No")]
        public string MobileNo { get; set; }

        [Required(ErrorMessage = "Please enter comment")]
        public string Comment { get; set; }

        public bool IsDeleted { get; set; }
        public bool IsRepled { get; set; }
    }

    public class SerachForm
    {
        public int StateId { get; set; }
        public int LgaId { get; set; }
    }
}