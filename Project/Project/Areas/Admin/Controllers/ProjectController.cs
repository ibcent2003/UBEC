using Project.Areas.Admin.Models;
using Project.Areas.SecurityGuard.Models;
using Project.Areas.Setup.Models;
using Project.DAL;
using Project.Models;
using Project.Properties;
using Project.UI.Models;
using SecurityGuard.Interfaces;
using SecurityGuard.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Roles = System.Web.Security.Roles;

namespace Project.Areas.Admin.Controllers
{
    public class ProjectController : Controller
    {
        private PROEntities db = new PROEntities();
        public ActionResult Index(int Id)
        {
            ProjectViewModel model = new ProjectViewModel();
            var getworkflow = db.Workflow.Where(x => Id == x.Id).FirstOrDefault();
            if(getworkflow==null)
            {
                TempData["message"] = Settings.Default.GenericExceptionMessage;
                TempData["messageType"] = "danger";
                return RedirectToAction("Index", "InspDashboard", new { area = "Admin" });
            }
            var getdraftProject = db.ProjectApplication.Where(x => x.ModifiedBy == User.Identity.Name).ToList();          
            model.projectList = getdraftProject;
            model.workflow = getworkflow;
            return View(model);
        }
        


        [HttpPost]
        public ActionResult GetLga(int stateId)
        {
            List<IntegerSelectListItem> ListLga = (from d in db.LGA where d.StateId == stateId && d.IsDeleted==false orderby d.Name select new IntegerSelectListItem { Text = d.Name, Value = d.Id }).ToList();
            return Json(ListLga);
        }
        public ActionResult ProjectDetail(int Id)
        {
            try
            {
                ProjectViewModel model = new ProjectViewModel();
                var getworkflow = db.Workflow.Where(x => Id == x.Id).FirstOrDefault();
                if (getworkflow == null)
                {
                    TempData["message"] = Settings.Default.GenericExceptionMessage;
                    TempData["messageType"] = "danger";
                    return RedirectToAction("Index", "InspDashboard", new { area = "Admin" });
                }                
                model.workflow = getworkflow;
                model.projectForm = new ProjectDetailForm();
                model.projectForm.workflowId = Id;
                model.StateList = (from s in db.State where s.IsDeleted==false select new IntegerSelectListItem { Text = s.Name, Value = s.Id }).ToList();
                model.LgaList = (from d in db.LGA select new IntegerSelectListItem { Value = d.Id, Text = d.Name }).ToList();
                model.ContractorList = (from d in db.Contractor select new IntegerSelectListItem { Value = d.Id, Text = d.Name }).ToList();
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
        public ActionResult ProjectDetail(ProjectViewModel model)
        {
            try
            {

                model.StateList = (from s in db.State where s.IsDeleted == false select new IntegerSelectListItem { Text = s.Name, Value = s.Id }).ToList();
                model.LgaList = (from d in db.LGA select new IntegerSelectListItem { Value = d.Id, Text = d.Name }).ToList();
                model.ContractorList = (from d in db.Contractor select new IntegerSelectListItem { Value = d.Id, Text = d.Name }).ToList();
                //if (ModelState.IsValid)
                //{
                    var workflow = db.Workflow.Where(x => x.Id == model.projectForm.workflowId).FirstOrDefault();
                    model.workflow = workflow;
                    var getproject = db.ProjectApplication.Where(x => x.SerialNo == model.projectForm.SerialNo).ToList();
                    if (getproject.Any())
                    {                      
                       
                        TempData["message"] = "ERROR: The Serial No " + model.projectForm.SerialNo + " already exist. Please enter different no.";
                        TempData["messageType"] = "danger";
                        model.workflow = workflow;
                        return View(model);
                    }
                    ProjectApplication addnew = new ProjectApplication
                    {
                        TransactionId = Guid.NewGuid(),
                        SerialNo = model.projectForm.SerialNo,
                        Status = "draft",
                        WorkFlowId = model.projectForm.workflowId,
                        Description = model.projectForm.Description,
                        Location = model.projectForm.Location,
                        Coordinate = model.projectForm.Coordinate,
                        LGAId = model.projectForm.LGAId,
                        ContractorId = model.projectForm.ContractorId,
                        ContractSum = model.projectForm.ContractSum,
                        StageOfCompletion = model.projectForm.StageOfCompletion,
                        DescriptionOfCompletion = model.projectForm.DescriptionOfCompletion,
                        ProjectQuality = model.projectForm.ProjectQuality,
                        HasDefect = model.projectForm.HasDefect,
                        DescriptionOfDefect = model.projectForm.DescriptionOfDefect,
                        ModifiedBy = User.Identity.Name,
                        ModifiedDate = DateTime.Now,
                        IsDeleted = false
                    };
                    db.ProjectApplication.AddObject(addnew);
                    db.SaveChanges();
                    TempData["message"] = "The project has been added successfully. Kindly add payment and upload photo of the project.";                   
                    return RedirectToAction("Index", "Project", new {Id=model.projectForm.workflowId, area = "Admin" });
            //}
            //var getworkflow = db.Workflow.Where(x => x.Id == model.projectForm.workflowId).FirstOrDefault();
            //model.StateList = (from s in db.State where s.IsDeleted == false select new IntegerSelectListItem { Text = s.Name, Value = s.Id }).ToList();
            //model.LgaList = (from d in db.LGA select new IntegerSelectListItem { Value = d.Id, Text = d.Name }).ToList();
            //model.ContractorList = (from d in db.Contractor select new IntegerSelectListItem { Value = d.Id, Text = d.Name }).ToList();
            //model.workflow = getworkflow;
            //TempData["message"] = "There is error submitting project information. Please sure you enter all fields with * sign.";
            //TempData["messageType"] = "danger";
            //return View(model);
        }
            catch(Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                TempData["message"] = Settings.Default.GenericExceptionMessage;
                TempData["messageType"] = "danger";
                return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
            }
        }

        public ActionResult ProjectPayment(Guid Id)
        {
            try
            {
                ProjectViewModel model = new ProjectViewModel();
                var getproject = db.ProjectApplication.Where(x =>x.TransactionId == Id).FirstOrDefault();
                if (getproject == null)
                {
                    TempData["message"] = Settings.Default.GenericExceptionMessage;
                    TempData["messageType"] = "danger";
                    return RedirectToAction("Index", "InspDashboard", new { area = "Admin" });
                }
                List<int> list = (from x in getproject.Payment select x.Id).ToList<int>();
                model.AvailablePayment = (from d in this.db.PaymentType where !list.Contains(d.Id) &&  d.IsDeleted == false select d).ToList<PaymentType>();
                model.ProjectPaymentList = getproject.Payment.ToList<Payment>();
                model.project = getproject;
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
