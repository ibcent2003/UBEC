using Project.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Project.Areas.Setup.Models
{
    public class PaymentTypeViewModel
    {
        public List<PaymentType> PaymentTypeList { get; set; }
        public PaymentTypeForm paymentTypeForm { get; set; }
    }
    public class PaymentTypeForm
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Please enter Payment Type name")]
        public string Name { get; set; }
        public bool IsDeleted { get; set; }
    }
}