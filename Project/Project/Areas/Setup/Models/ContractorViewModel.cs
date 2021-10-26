using Project.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Project.Areas.Setup.Models
{
    public class ContractorViewModel
    {
        public List<Contractor> ContractorList { get; set; }
        public ContractorForm contractorForm { get; set; }
    }
    public class ContractorForm
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter customer name")]        
        public string Name { get; set; }        
        public bool IsDeleted { get; set; }
    }
}