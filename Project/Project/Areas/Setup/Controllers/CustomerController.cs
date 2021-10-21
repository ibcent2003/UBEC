using Project.Areas.Setup.Models;
using Project.DAL;
using Project.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project.Areas.Setup.Controllers
{
    public class CustomerController : Controller
    {
        private PROEntities db = new PROEntities();

        public ActionResult Index()
        {
            try
            {
                CustomerViewModel model = new CustomerViewModel();
                var Getcustomer = db.Customer.ToList();
                model.CustomerList = Getcustomer;
                return View(model);
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                TempData["message"] = Settings.Default.GenericExceptionMessage;
                TempData["messageType"] = "danger";
                return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
            }
        }

        public ActionResult NewCustomer()
        {
            try
            {
                CustomerViewModel model = new CustomerViewModel();               
                return View(model);
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                TempData["message"] = Settings.Default.GenericExceptionMessage;
                TempData["messageType"] = "danger";
                return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
            }
        }

        [HttpPost]
        public ActionResult NewCustomer(CustomerViewModel model)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    var getCustomer = db.Customer.Where(x => x.Name == model.customerForm.Name).ToList();
                    if(getCustomer.Any())
                    {
                        TempData["message"] = "ERROR: The customer name " + model.customerForm.Name + " already exist. Please enter different name.";
                        TempData["messageType"] = "danger";
                        return View(model);
                    }
                    //do insert here
                    Customer addnew = new Customer
                    {
                        Name = model.customerForm.Name,
                        MobileNo = model.customerForm.MobileNo,
                        EmailAddress = model.customerForm.EmailAddress,
                        ContactAddress = model.customerForm.ContactAddress,
                        IsDeleted = model.customerForm.IsDeleted,
                        ModifiedBy = User.Identity.Name,
                        ModifiedDate = DateTime.Now
                    };
                    db.Customer.AddObject(addnew);
                    db.SaveChanges();
                    TempData["message"] = "<b>" +model.customerForm.Name+ "</b> has been added successfully.";
                    return RedirectToAction("Index");
                }
                TempData["message"] = "There is error submitting customer information. Please sure you enter all filds with * sign.";
                TempData["messageType"] = "danger";
                return View(model);
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                TempData["message"] = Settings.Default.GenericExceptionMessage;
                TempData["messageType"] = "danger";
                return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
            }
        }

        public ActionResult EditCustomer(int Id)
        {
            try
            {
                CustomerViewModel model = new CustomerViewModel();
                var getCustomer = db.Customer.Where(x => x.Id == Id).FirstOrDefault();
                if(getCustomer == null)
                {
                    TempData["message"] = "Ops! Something wrong. Please start the process of editting a customer.";
                    TempData["messageType"] = "danger";
                    return RedirectToAction("Index");
                }
                model.customerForm = new CustomerForm();
                model.customerForm.Id = getCustomer.Id;
                model.customerForm.Name = getCustomer.Name;
                model.customerForm.MobileNo = getCustomer.MobileNo;
                model.customerForm.EmailAddress = getCustomer.EmailAddress;
                model.customerForm.ContactAddress = getCustomer.ContactAddress;
                model.customerForm.IsDeleted = getCustomer.IsDeleted;
                return View(model);
            }
            catch(Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                TempData["message"] = Settings.Default.GenericExceptionMessage;
                TempData["messageType"] = "danger";
                return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
            }
        }

        [HttpPost]
        public ActionResult EditCustomer(CustomerViewModel model)
        {
            try
            {
                var getCustomer = db.Customer.Where(x => x.Id == model.customerForm.Id).FirstOrDefault();
                if (getCustomer == null)
                {
                    TempData["message"] = "Ops! Something wrong. Please start the process of editting a customer.";
                    TempData["messageType"] = "danger";
                    return RedirectToAction("Index");
                }
                if (ModelState.IsValid)
                {
                    getCustomer.Name = model.customerForm.Name;
                    getCustomer.MobileNo = model.customerForm.MobileNo;
                    getCustomer.EmailAddress = model.customerForm.EmailAddress;
                    getCustomer.ContactAddress = model.customerForm.ContactAddress;
                    getCustomer.IsDeleted = model.customerForm.IsDeleted;
                    getCustomer.ModifiedBy = User.Identity.Name;
                    getCustomer.ModifiedDate = DateTime.Now;
                    db.Employee.Context.SaveChanges();
                    db.SaveChanges();
                    TempData["message"] = "<b>" + model.customerForm.Name + "</b> has been updated successfully.";
                    return RedirectToAction("Index");
                }
                TempData["message"] = "There is error submitting customer information. Please sure you enter all filds with * sign.";
                TempData["messageType"] = "danger";
                return View(model);
                
            }
            catch(Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                TempData["message"] = Settings.Default.GenericExceptionMessage;
                TempData["messageType"] = "danger";
                return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
            }
        }
    }
}
