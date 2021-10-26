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
    public class ContractorController : Controller
    {
        private PROEntities db = new PROEntities();

        public ActionResult Index()
        {
            try
            {
                ContractorViewModel model = new ContractorViewModel();
                var Getcontractor = db.Contractor.ToList();
                model.ContractorList = Getcontractor;
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

        public ActionResult NewContractor()
        {
            try
            {
                ContractorViewModel model = new ContractorViewModel();               
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
        public ActionResult NewContractor(ContractorViewModel model)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    var getcontractor = db.Contractor.Where(x => x.Name == model.contractorForm.Name).ToList();
                    if(getcontractor.Any())
                    {
                        TempData["message"] = "ERROR: The contractor name " + model.contractorForm.Name + " already exist. Please enter different name.";
                        TempData["messageType"] = "danger";
                        return View(model);
                    }
                    //do insert here
                    Contractor addnew = new Contractor
                    {
                        Name = model.contractorForm.Name,                        
                        IsDeleted = model.contractorForm.IsDeleted,
                        ModifiedBy = User.Identity.Name,
                        ModifiedDate = DateTime.Now
                    };
                    db.Contractor.AddObject(addnew);
                    db.SaveChanges();
                    TempData["message"] = "<b>" +model.contractorForm.Name+ "</b> has been added successfully.";
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

        public ActionResult EditContractor(int Id)
        {
            try
            {
                ContractorViewModel model = new ContractorViewModel();
                var getcontractor = db.Contractor.Where(x => x.Id == Id).FirstOrDefault();
                if(getcontractor == null)
                {
                    TempData["message"] = "Ops! Something wrong. Please start the process of editting a contractor.";
                    TempData["messageType"] = "danger";
                    return RedirectToAction("Index");
                }
                model.contractorForm = new  ContractorForm();
                model.contractorForm.Id = getcontractor.Id;
                model.contractorForm.Name = getcontractor.Name;              
                model.contractorForm.IsDeleted = getcontractor.IsDeleted;
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
        public ActionResult EditContractor(ContractorViewModel model)
        {
            try
            {
                var getContractor = db.Contractor.Where(x => x.Id == model.contractorForm.Id).FirstOrDefault();
                if (getContractor == null)
                {
                    TempData["message"] = "Ops! Something wrong. Please start the process of editting a contractor.";
                    TempData["messageType"] = "danger";
                    return RedirectToAction("Index");
                }
                if (ModelState.IsValid)
                {
                    getContractor.Name = model.contractorForm.Name;

                    getContractor.IsDeleted = model.contractorForm.IsDeleted;
                    getContractor.ModifiedBy = User.Identity.Name;
                    getContractor.ModifiedDate = DateTime.Now;
                    db.Contractor.Context.SaveChanges();
                    db.SaveChanges();
                    TempData["message"] = "<b>" + model.contractorForm.Name + "</b> has been updated successfully.";
                    return RedirectToAction("Index");
                }
                TempData["message"] = "There is error submitting contractor information. Please sure you enter all filds with * sign.";
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
