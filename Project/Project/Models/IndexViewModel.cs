using Project.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.Models
{
    public class IndexViewModel
    {
        public List<News> NewsList { get; set; }
        public string PicturePath { get; set; }
        public News news { get; set; }
        public ProjectApplication project { get; set; }
        public List<ProjectApplication> projectList { get; set; }
        public List<Inspection> inspectionlist { get; set; }
        public Workflow workflow { get; set; }
        public List<Workflow> ProjectTypes { get; set; }
        public bool EnableSum { get; set; }

    }
}