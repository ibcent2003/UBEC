using Project.DAL;
using Project.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project.Areas.Setup.Models
{
    public class NewsManagementViewModel : PageInfoModel
    {
        public string PicturePath { get; set; }
        public IList<News> Rows { get; set; }
        public News news { get; set; }              
        public NewsForm newsform { get; set; }
    }

    public class NewsForm
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter the News Headline")]
        [Display(Name = "News Headline")]
        public string NewsHeadline { get; set; }

        [Required(ErrorMessage = "Please enter the News Content")]
        [Display(Name = "News Content")]
        public string NewsContent { get; set; }       

        [Required(ErrorMessage = "Please upload Photo for the news")]
        public HttpPostedFileBase Photo { get; set; }

        [Display(Name = "Is Published")]
        public bool IsPublished { get; set; }

        [Display(Name = "Is Deleted")]
        public bool IsDeleted { get; set; }
    }
}