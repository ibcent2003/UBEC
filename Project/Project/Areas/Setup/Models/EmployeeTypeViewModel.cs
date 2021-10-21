using Project.DAL;
using Project.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Project.Areas.Setup.Models
{
    public class EmployeeTypeViewModel
    {
        public List<EmployeeType> EmployeeTypeList { get; set; }
        public List<Employee> EmployeeList { get; set; }
        public EmployeeTypeForm EmployeeTypeForm { get; set; }
        public List<IntegerSelectListItem> ListEmployeeType { get; set; }
        public EmployeeForm employeeForm { get; set; }
        public string FullPhotoPath { get; set; }
        public string UploadedPhoto { get; set; }
    }

    public class EmployeeTypeForm
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter Employee type")]
        [Display(Name = "Employee Type")]
        public string Name { get; set; }

        public bool IsDeleted { get; set; }
    }
    public class EmployeeForm
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter first name")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please enter last name")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Please Mobile No")]
        [Display(Name = "Mobile Name")]
        public string MobileNo { get; set; }

       
        [Display(Name = "Email Address")]
        public string EmailAddress { get; set; }

        [Required(ErrorMessage = "Please Contact Address")]
        [Display(Name = "Contact Address")]
        public string Address { get; set; }

        [Display(Name = "Photo")]
        public HttpPostedFileBase Photo { get; set; }

        [Display(Name = "Document Category")]
        [Required(ErrorMessage = "Please a Employee Type")]
        public int EmployeeTypeId { get; set; }
        
        public bool IsDeleted { get; set; }
        

    }
}