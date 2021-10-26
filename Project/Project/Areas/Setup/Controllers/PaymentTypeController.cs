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
    public class PaymentTypeController : Controller
    {
        //
        // GET: /Setup/PaymentType/
        private PROEntities db = new PROEntities();

        public ActionResult Index()
        {
            try
            {
                PaymentTypeViewModel model = new PaymentTypeViewModel();
                var Getpayment = db.PaymentType.ToList();
                model.PaymentTypeList = Getpayment;
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

        public ActionResult NewPaymentType()
        {
            try
            {
                PaymentTypeViewModel model = new PaymentTypeViewModel();
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
        public ActionResult NewPaymentType(PaymentTypeViewModel model)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    var getpayment = db.PaymentType.Where(x => x.Name == model.paymentTypeForm.Name).ToList();
                    if (getpayment.Any())
                    {
                        TempData["message"] = "ERROR: The payment type name " + model.paymentTypeForm.Name + " already exist. Please enter different name.";
                        TempData["messageType"] = "danger";
                        return View(model);
                    }
                    //do insert here
                    PaymentType addnew = new PaymentType
                    {
                        Name = model.paymentTypeForm.Name,
                        IsDeleted = model.paymentTypeForm.IsDeleted,
                        ModifiedBy = User.Identity.Name,
                        ModifiedDate = DateTime.Now
                    };
                    db.PaymentType.AddObject(addnew);
                    db.SaveChanges();
                    TempData["message"] = "<b>" + model.paymentTypeForm.Name + "</b> has been added successfully.";
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


        public ActionResult EditPaymentType(int Id)
        {
            try
            {
                PaymentTypeViewModel model = new PaymentTypeViewModel();
                var getpayment = db.PaymentType.Where(x => x.Id == Id).FirstOrDefault();
                if (getpayment == null)
                {
                    TempData["message"] = "Ops! Something wrong. Please start the process of editting a payment type.";
                    TempData["messageType"] = "danger";
                    return RedirectToAction("Index");
                }
                model.paymentTypeForm = new  PaymentTypeForm();
                model.paymentTypeForm.Id = getpayment.Id;
                model.paymentTypeForm.Name = getpayment.Name;
                model.paymentTypeForm.IsDeleted = getpayment.IsDeleted;
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
        public ActionResult EditPaymentType(PaymentTypeViewModel model)
        {
            try
            {
                var getPayment = db.PaymentType.Where(x => x.Id == model.paymentTypeForm.Id).FirstOrDefault();
                if (getPayment == null)
                {
                    TempData["message"] = "Ops! Something wrong. Please start the process of payment type a contractor.";
                    TempData["messageType"] = "danger";
                    return RedirectToAction("Index");
                }
                if (ModelState.IsValid)
                {
                    getPayment.Name = model.paymentTypeForm.Name;

                    getPayment.IsDeleted = model.paymentTypeForm.IsDeleted;
                    getPayment.ModifiedBy = User.Identity.Name;
                    getPayment.ModifiedDate = DateTime.Now;
                    db.Contractor.Context.SaveChanges();
                    db.SaveChanges();
                    TempData["message"] = "<b>" + model.paymentTypeForm.Name + "</b> has been updated successfully.";
                    return RedirectToAction("Index");
                }
                TempData["message"] = "There is error submitting payment type information. Please sure you enter all filds with * sign.";
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
    }
}
