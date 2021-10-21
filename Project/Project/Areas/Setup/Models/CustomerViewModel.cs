using Project.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Project.Areas.Setup.Models
{
    public class CustomerViewModel
    {
        public List<Customer> CustomerList { get; set; }
        public CustomerForm customerForm { get; set; }
    }
    public class CustomerForm
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter customer name")]        
        public string Name { get; set; }

        [Required(ErrorMessage = "Please enter customer mobile no")]
        public string MobileNo { get; set; }

        public string EmailAddress { get; set; }

        [Required(ErrorMessage = "Please enter customer contact address")]
        public string ContactAddress { get; set; }
        public bool IsDeleted { get; set; }
    }
}