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
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Roles = System.Web.Security.Roles;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using iTextSharp.text.html.simpleparser;


namespace Project.Areas.Admin.Controllers
{
    public class ProjectController : Controller
    {
        private PROEntities db = new PROEntities();
        private ProcessUtility util;
        GetProject services = new GetProject();
        public ProjectController()
        {
            this.util = new ProcessUtility();

        }
        public ActionResult Index(int Id)
        {
            ProjectViewModel model = new ProjectViewModel();
            var getworkflow = db.Workflow.Where(x => Id == x.Id).FirstOrDefault();
            if(getworkflow==null)
            {
                TempData["message"] = Settings.Default.GenericExceptionMessage;
                TempData["messageType"] = "danger";
                return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
            }
            var getuser = db.Users.Where(x => x.UserName == User.Identity.Name).FirstOrDefault();
            model.user = getuser;
            var getdraftProject = db.ProjectApplication.Where(x=>x.WorkFlowId==Id).ToList();          
            model.projectList = getdraftProject;
            model.workflow = getworkflow;
            return View(model);
        }        

        public ActionResult ApproveInspection(int Id)
        {
            try
            {
                var getInspection = db.Inspection.Where(x => x.Id == Id).FirstOrDefault();
                if (getInspection == null)
                {
                    TempData["message"] = Settings.Default.GenericExceptionMessage;
                    TempData["messageType"] = "danger";
                    return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
                }

                var getproject = db.ProjectApplication.Where(x => x.Id == getInspection.ProjectId).FirstOrDefault();
                if (getproject == null)
                {
                    TempData["message"] = Settings.Default.GenericExceptionMessage;
                    TempData["messageType"] = "danger";
                    return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
                }
                ProjectViewModel model = new ProjectViewModel();
                getInspection.InspectionStatus = "Approved";
                db.SaveChanges();
                TempData["message"] = "Report has been approved successfully.";
                return RedirectToAction("Detail", "Project", new {Id= getInspection.Id, area = "Admin" });
            }
            catch(Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                TempData["message"] = Settings.Default.GenericExceptionMessage;
                TempData["messageType"] = "danger";
                return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
            }
        }

        public ActionResult DisapproveInspection(int Id)
        {
            try
            {
                var getInspection = db.Inspection.Where(x => x.Id == Id).FirstOrDefault();
                if (getInspection == null)
                {
                    TempData["message"] = Settings.Default.GenericExceptionMessage;
                    TempData["messageType"] = "danger";
                    return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
                }

                var getproject = db.ProjectApplication.Where(x => x.Id == getInspection.ProjectId).FirstOrDefault();
                if (getproject == null)
                {
                    TempData["message"] = Settings.Default.GenericExceptionMessage;
                    TempData["messageType"] = "danger";
                    return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
                }
                ProjectViewModel model = new ProjectViewModel();
                getInspection.InspectionStatus = "Not Submitted";
                db.SaveChanges();
                TempData["message"] = "Report has been Disapproved successfully.";
                return RedirectToAction("Detail", "Project", new { Id = getInspection.Id, area = "Admin" });
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
        public ActionResult GetLga(int stateId)
        {
            List<IntegerSelectListItem> ListLga = (from d in db.LGA where d.StateId == stateId && d.IsDeleted==false orderby d.Name select new IntegerSelectListItem { Text = d.Name, Value = d.Id }).ToList();
            return Json(ListLga);
        }

        [HttpPost]
        public ActionResult GetDescription(int stageId)
        {
            // var getDes = (from d in db.StageOfCompletion where d.Id == stageId && d.IsDeleted == false  select d.Description).FirstOrDefault();
            List<IntegerSelectListItem> getDes = (from d in db.StageOfCompletion where d.Id == stageId && d.IsDeleted == false select new IntegerSelectListItem { Text = d.Description, Value = d.Id }).ToList();
            return Json(getDes);
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

                model.ProjectTypeList = (from d in db.ProjectType  select new IntegerSelectListItem { Value = d.Id, Text = d.Name }).ToList();
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
                model.ProjectTypeList = (from d in db.ProjectType select new IntegerSelectListItem { Value = d.Id, Text = d.Name }).ToList();
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
                        ProjectTypeId = model.projectForm.ProjectTypeId,
                        StartDate = model.projectForm.StartDate,
                        EndDate = model.projectForm.EndDate,
                        ModifiedBy = User.Identity.Name,
                        ModifiedDate = DateTime.Now,
                        IsDeleted = false,  
                        EnableSum = model.projectForm.ShowCost,
                        // =                  
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

        public ActionResult EditProject(Guid Id)
        {
            try
            {
                ProjectViewModel model = new ProjectViewModel();
                var getproject = db.ProjectApplication.Where(x => x.TransactionId == Id).FirstOrDefault();
                if (getproject == null)
                {
                    TempData["message"] = Settings.Default.GenericExceptionMessage;
                    TempData["messageType"] = "danger";
                    return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
                }
                model.projectForm = new ProjectDetailForm();
               
                model.StateList = (from s in db.State where s.IsDeleted == false select new IntegerSelectListItem { Text = s.Name, Value = s.Id }).ToList();
                model.LgaList = (from d in db.LGA select new IntegerSelectListItem { Value = d.Id, Text = d.Name }).ToList();
                model.ContractorList = (from d in db.Contractor select new IntegerSelectListItem { Value = d.Id, Text = d.Name }).ToList();
                model.ProjectTypeList = (from d in db.ProjectType select new IntegerSelectListItem { Value = d.Id, Text = d.Name }).ToList();
                model.projectForm.workflowId = getproject.WorkFlowId;
                model.projectForm.SerialNo = getproject.SerialNo;
                model.projectForm.Description = getproject.Description;
                model.projectForm.Location = getproject.Location;
                model.projectForm.LGAId = getproject.LGAId;
                model.projectForm.ContractorId = getproject.ContractorId;
                model.projectForm.ContractSum = getproject.ContractSum;
                model.projectForm.IsDeleted = getproject.IsDeleted;
                model.StateId = getproject.LGA.StateId;
                model.projectForm.ProjectTypeId = getproject.ProjectTypeId;
                model.project = getproject;
                model.projectForm.Coordinate = getproject.Coordinate;
                model.projectForm.StartDate = getproject.StartDate;
                model.projectForm.EndDate = getproject.EndDate;
                model.projectForm.ShowCost = getproject.EnableSum;
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
        public ActionResult EditProject(ProjectViewModel model)
        {
            try
            {
                model.StateList = (from s in db.State where s.IsDeleted == false select new IntegerSelectListItem { Text = s.Name, Value = s.Id }).ToList();
                model.LgaList = (from d in db.LGA select new IntegerSelectListItem { Value = d.Id, Text = d.Name }).ToList();
                model.ContractorList = (from d in db.Contractor select new IntegerSelectListItem { Value = d.Id, Text = d.Name }).ToList();
                model.ProjectTypeList = (from d in db.ProjectType select new IntegerSelectListItem { Value = d.Id, Text = d.Name }).ToList();
                var workflow = db.Workflow.Where(x => x.Id == model.project.WorkFlowId).FirstOrDefault();
                model.workflow = workflow;
                var getproject = db.ProjectApplication.Where(x => x.TransactionId == model.project.TransactionId).FirstOrDefault();
                if (getproject == null)
                {
                    TempData["message"] = "Ops! There is an error while updating project. Please try again or contact the system administrator";
                    TempData["messageType"] = "danger";
                    return RedirectToAction("Index", "Project", new { Id = model.project.WorkFlowId, area = "Admin" });
                }
                getproject.SerialNo = model.projectForm.SerialNo;
                getproject.Description = model.projectForm.Description;
                getproject.ContractorId = model.projectForm.ContractorId;
                getproject.ContractSum = model.projectForm.ContractSum;
                getproject.LGAId = model.projectForm.LGAId;
                getproject.EnableSum = model.projectForm.ShowCost;
                getproject.Location = model.projectForm.Location;
                getproject.ProjectTypeId = model.projectForm.ProjectTypeId;
                getproject.StartDate = model.projectForm.StartDate;
                getproject.EndDate = model.projectForm.EndDate;
                getproject.ModifiedBy = User.Identity.Name;
                getproject.ModifiedDate = DateTime.Now;
                db.SaveChanges();
                TempData["message"] = "The project <b> "+model.projectForm.SerialNo+ "</b> has been update successfully.";
                return RedirectToAction("Index", "Project", new { Id = getproject.WorkFlowId, area = "Admin" });
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
                List<int> list = (from x in getproject.Payment select x.PaymentTypeId).ToList<int>();
                model.AvailablePayment = (from d in this.db.PaymentType where !list.Contains(d.Id) &&  d.IsDeleted == false select d).ToList<PaymentType>();
                model.ProjectPaymentList = getproject.Payment.ToList<Payment>();
                model.project = getproject;
                var getdraftProject = db.ProjectApplication.Where(x=>x.TransactionId==Id).ToList();
                model.projectList = getdraftProject;
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
        public ActionResult ProjectPayment(ProjectViewModel model)
        {
            try
            {
                var getproject = db.ProjectApplication.Where(x => x.TransactionId == model.project.TransactionId).FirstOrDefault();
                if (getproject == null)
                {
                    TempData["message"] = Settings.Default.GenericExceptionMessage;
                    TempData["messageType"] = "danger";
                    return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
                }
                model.project = getproject;
                List<int> list = (from x in getproject.Payment select x.Id).ToList<int>();
                model.AvailablePayment = (from d in this.db.PaymentType where !list.Contains(d.Id) && d.IsDeleted == false select d).ToList<PaymentType>();
                model.ProjectPaymentList = getproject.Payment.ToList<Payment>();
                var getdraftProject = db.ProjectApplication.Where(x => x.TransactionId == getproject.TransactionId).ToList();
                model.projectList = getdraftProject;
                if (model.projectPaymentForm.PaymentTypeId == 0)
                {
                    TempData["messageType"] = "danger";
                    TempData["message"] = "Please select a paymnt type from the dropdownlist and click on the Add button";
                    return RedirectToAction("ProjectPayment", "Project", new { Id = model.project.TransactionId, area = "Admin" });
                }
                if (!(from x in getproject.Payment where x.Id == model.projectPaymentForm.PaymentTypeId select x).ToList<Payment>().Any<Payment>())
                {
                    Payment payment = (from x in this.db.Payment where x.Id == model.projectPaymentForm.PaymentTypeId select x).FirstOrDefault<Payment>();
                    getproject.Payment.FirstOrDefault<Payment>();

                    //add payment 
                    Payment addnew = new Payment
                    {
                        PaymentTypeId = model.projectPaymentForm.PaymentTypeId,
                        ProjectId = getproject.Id,
                        ContractSum = model.projectPaymentForm.ProjectSum,
                        ModifiedBy = User.Identity.Name,
                        ModifiedDate = DateTime.Now

                    };

                    db.Payment.AddObject(addnew);
                    this.db.SaveChanges();
                    List<int> listed = (from x in getproject.Payment select x.Id).ToList<int>();
                    model.AvailablePayment = (from d in this.db.PaymentType where !listed.Contains(d.Id) && d.IsDeleted == false select d).ToList<PaymentType>();
                    model.ProjectPaymentList = getproject.Payment.ToList<Payment>();
                    base.TempData["message"] = "The Payment has been added successfully.";
                    return RedirectToAction("ProjectPayment", "Project", new { Id = model.project.TransactionId, area = "Admin" });
                }
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

        public ActionResult RemovePayment(int Id, int paymentId)
        {
            ActionResult action;
            try
            {
                ProjectViewModel model = new ProjectViewModel();
                var getproject = (from d in db.ProjectApplication where d.Id == Id select d).FirstOrDefault();
                var payment = (from x in this.db.Payment where x.Id == paymentId select x).FirstOrDefault();
                Payment remove = new Payment
                {
                };
                db.Payment.DeleteObject(payment);
                db.SaveChanges();
                TempData["message"] = "The payment has been deleted successfully.";               
                action = base.RedirectToAction("ProjectPayment", "Project", new { Id = getproject.TransactionId, area = "Admin" });
            }
            catch (Exception exception1)
            {
                Exception exception = exception1;
                base.TempData["messageType"] = "danger";
                base.TempData["message"] = "There is an error in the application. Please try again or contact the system administrator";
                Elmah.ErrorSignal.FromCurrentContext().Raise(exception);
                action = base.RedirectToAction("Index", "Dashboard", new { area = "Admin" });
            }
            return action;
        }
      
        public ActionResult DocumentsUploadedPath(string path)
        {
            try
            {
                var filepath = new Uri(path);
                if (System.IO.File.Exists(filepath.AbsolutePath))
                {
                    byte[] filedata = System.IO.File.ReadAllBytes(filepath.AbsolutePath);
                    string contentType = MimeMapping.GetMimeMapping(filepath.AbsolutePath);

                    System.Net.Mime.ContentDisposition cd = new System.Net.Mime.ContentDisposition
                    {
                        FileName = path,
                        Inline = true,
                    };

                    Response.AppendHeader("Content-Disposition", cd.ToString());

                    return File(filedata, contentType);
                }
                else
                {
                    return null;                    
                }
            }
            catch (Exception ex)
            {

                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                return null;
            }
        }

        public ActionResult UploadPhoto(int Id)
        {
            try
            {
                ProjectViewModel model = new ProjectViewModel();
                var getInspection = db.Inspection.Where(x => x.Id == Id).FirstOrDefault();
                if (getInspection == null)
                {
                    TempData["message"] = Settings.Default.GenericExceptionMessage;
                    TempData["messageType"] = "danger";
                    return RedirectToAction("Index", "InspDashboard", new { area = "Admin" });
                }
                var getproject = db.ProjectApplication.Where(x => x.Id == getInspection.ProjectId).FirstOrDefault();
                if (getproject == null)
                {
                    TempData["message"] = Settings.Default.GenericExceptionMessage;
                    TempData["messageType"] = "danger";
                    return RedirectToAction("Index", "InspDashboard", new { area = "Admin" });
                }
                var getworkflow = db.Workflow.Where(x => x.Id == getproject.WorkFlowId).FirstOrDefault();
                List<int> list = (from x in getInspection.DocumentInfo select x.DocumentTypeId).ToList<int>();
                model.AvailableDocument = (from d in getworkflow.DocumentType where !list.Contains(d.Id) && d.IsDeleted == false select d).ToList<DocumentType>();
                if(model.AvailableDocument.Count==0)
                {
                    model.AllPhotoUploaded = true;
                }
                else
                {
                    model.AllPhotoUploaded = false;
                }
                model.DocumentInfoList = getInspection.DocumentInfo.ToList<DocumentInfo>();
                model.FullPhotoPath = Properties.Settings.Default.FullPhotoPath;
                model.project = getproject;
                var getdraftProject = db.ProjectApplication.Where(x => x.TransactionId == getproject.TransactionId).ToList();
                model.projectList = getdraftProject;
                model.inspection = getInspection;

                var inspectionlist = db.Inspection.Where(x=>x.Id==Id).ToList();
                model.InspectionList = inspectionlist;
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
        public ActionResult UploadPhoto(ProjectViewModel model)
        {
            try
            {

                var getInspection = db.Inspection.Where(x => x.Id == model.inspection.Id).FirstOrDefault();
                if (getInspection == null)
                {
                    TempData["message"] = Settings.Default.GenericExceptionMessage;
                    TempData["messageType"] = "danger";
                    return RedirectToAction("Index", "InspDashboard", new { area = "Admin" });
                }

                var getproject = db.ProjectApplication.Where(x => x.Id == getInspection.ProjectId).FirstOrDefault();
                if (getproject == null)
                {
                    TempData["message"] = Settings.Default.GenericExceptionMessage;
                    TempData["messageType"] = "danger";
                    return RedirectToAction("Index", "InspDashboard", new { area = "Admin" });
                }
                var getworkflow = db.Workflow.Where(x => x.Id == getproject.WorkFlowId).FirstOrDefault();
                List<int> list = (from x in getInspection.DocumentInfo select x.Id).ToList<int>();
                model.AvailableDocument = (from d in getworkflow.DocumentType where !list.Contains(d.Id) && d.IsDeleted == false select d).ToList<DocumentType>();
                model.DocumentInfoList = getInspection.DocumentInfo.ToList<DocumentInfo>();
                model.FullPhotoPath = Properties.Settings.Default.FullPhotoPath;
                model.project = getproject;
                var getdraftProject = db.ProjectApplication.Where(x => x.Id == getInspection.ProjectId).ToList();
                model.projectList = getdraftProject;

                //var getpath = getInspection.DocumentInfo.Where(x=>x.DocumentTypeId==model.documentForm.DocumentTypeId).FirstOrDefault();

                if (ModelState.IsValid)
                {
                    string url = Settings.Default.FullPhotoPath;
                    System.IO.Directory.CreateDirectory(url);                  

                    #region upload document

                    int max_upload = 5242880;

                    UI.Models.CodeGenerator CodePassport = new UI.Models.CodeGenerator();
                    string EncKey = util.MD5Hash(DateTime.Now.Ticks.ToString());
                    List<Project.DAL.DocumentFormat> Resumetypes = db.DocumentType.FirstOrDefault(x => x.DocumentCategoryId == 1).DocumentFormat.ToList();

                    List<string> supportedResume = new List<string>();
                    foreach (var item in Resumetypes)
                    {
                        supportedResume.Add(item.Extension);
                    }
                    var fileResume = System.IO.Path.GetExtension(model.documentForm.Photo.FileName);
                    if (!supportedResume.Contains(fileResume.ToLower()))
                    {
                        TempData["messageType"] = "danger";
                        TempData["message"] = "Invalid type. Only the following type " + String.Join(",", supportedResume) + " are supported for Photo ";
                        return View(model);

                    }

                    if (model.documentForm.Photo.ContentLength > max_upload)
                    {

                        TempData["messageType"] = "danger";
                        TempData["message"] = "The Photo uploaded is larger than the 5MB upload limit";
                        return View(model);
                    }
                    #endregion

                  

                    #region save Resume
                    int i = 0;
                    string filename;
                    filename = EncKey + i.ToString() + System.IO.Path.GetExtension(model.documentForm.Photo.FileName);
                    model.documentForm.Photo.SaveAs(url + filename);
                    #endregion
                    var getdoctype = db.DocumentType.Where(x => x.Id == model.documentForm.DocumentTypeId).FirstOrDefault();
                    if(getdoctype== null)
                    {
                        TempData["message"] = Settings.Default.GenericExceptionMessage;
                        TempData["messageType"] = "danger";
                        return RedirectToAction("Index", "InspDashboard", new { area = "Admin" });
                    }
                    DocumentInfo addnew = new DocumentInfo
                    {
                        Name  = getdoctype.Name,
                        Path = filename,
                        DocumentTypeId = model.documentForm.DocumentTypeId,
                        Size = model.documentForm.Photo.ContentLength.ToString(),
                        Extension = System.IO.Path.GetExtension(model.documentForm.Photo.FileName),
                        IssuedDate = DateTime.Now,
                        ModifiedBy = User.Identity.Name,
                        ModifiedDate = DateTime.Now
                    };
                    db.DocumentInfo.AddObject(addnew);
                    getInspection.DocumentInfo.Add(addnew);
                    db.SaveChanges();
                    TempData["message"] = "Photo type <b>"+ getdoctype.Name.ToUpper()+ "</b> has been uploaded successful";
                    return RedirectToAction("UploadPhoto", "Project", new {Id= getInspection.Id, area = "Admin" });
                }
                TempData["message"] = "Error uploading photo. Please make sure you select the photo type and click on the browse button to browse a photo from your computer.";
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

        public ActionResult RemovePhoto(int Id, int DocumentId)
        {
            try
            {
                ProjectViewModel model = new ProjectViewModel();
                var getInspection = db.Inspection.Where(x => x.Id == Id).FirstOrDefault();
                if (getInspection == null)
                {
                    TempData["message"] = Settings.Default.GenericExceptionMessage;
                    TempData["messageType"] = "danger";
                    return RedirectToAction("Index", "InspDashboard", new { area = "Admin" });
                }

                var getproject = db.ProjectApplication.Where(x => x.Id == getInspection.ProjectId).FirstOrDefault();
                if (getproject == null)
                {
                    TempData["message"] = Settings.Default.GenericExceptionMessage;
                    TempData["messageType"] = "danger";
                    return RedirectToAction("Index", "InspDashboard", new { area = "Admin" });
                }
                var getworkflow = db.Workflow.Where(x => x.Id == getproject.WorkFlowId).FirstOrDefault();
                List<int> list = (from x in getInspection.DocumentInfo select x.Id).ToList<int>();
                model.AvailableDocument = (from d in getworkflow.DocumentType where !list.Contains(d.Id) && d.IsDeleted == false select d).ToList<DocumentType>();
                model.DocumentInfoList = getInspection.DocumentInfo.ToList<DocumentInfo>();
                model.FullPhotoPath = Properties.Settings.Default.FullPhotoPath;
                model.project = getproject;
                var getdraftProject = db.ProjectApplication.Where(x => x.TransactionId == getproject.TransactionId).ToList();
                model.projectList = getdraftProject;

          
                string url = Settings.Default.FullPhotoPath;
                System.IO.Directory.CreateDirectory(url);

                var documentInfo = db.DocumentInfo.Where(x => x.Id == DocumentId).FirstOrDefault();
                if (documentInfo.Path != null)
                {
                    System.IO.FileInfo fi = new System.IO.FileInfo(url + documentInfo.Path);
                    fi.Delete();
                }
                DocumentInfo del = new DocumentInfo
                {

                };
                db.DocumentInfo.DeleteObject(documentInfo);
                getInspection.DocumentInfo.Remove(documentInfo);
                db.SaveChanges();
                TempData["message"] = "The photo has been deleted successful.";
               
                return RedirectToAction("UploadPhoto", "Project", new {Id= getInspection.Id, area = "Admin" });
            }
            catch(Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                TempData["message"] = Settings.Default.GenericExceptionMessage;
                TempData["messageType"] = "danger";
                return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
            }
        }
        public ActionResult Detail(int Id)
        {
            try
            {
                ProjectViewModel model = new ProjectViewModel();
                var getInspection = db.Inspection.Where(x => x.Id == Id).FirstOrDefault();
                if (getInspection == null)
                {
                    TempData["message"] = Settings.Default.GenericExceptionMessage;
                    TempData["messageType"] = "danger";
                    return RedirectToAction("Index", "InspDashboard", new { area = "Admin" });
                }
                var getproject = db.ProjectApplication.Where(x => x.Id == getInspection.ProjectId).FirstOrDefault();
                if (getproject == null)
                {
                    TempData["message"] = Settings.Default.GenericExceptionMessage;
                    TempData["messageType"] = "danger";
                    return RedirectToAction("Index", "InspDashboard", new { area = "Admin" });
                }
                var getuser = db.Users.Where(x => x.UserName == User.Identity.Name).FirstOrDefault();
                if(getproject.InspectionUserId == getuser.UserId )
                {
                    model.ReportOwner = true;
                }
                else
                {
                    model.ReportOwner = false;
                }
                model.project = getproject;
                var getdraftProject = db.ProjectApplication.Where(x => x.TransactionId == getproject.TransactionId).ToList();
                model.projectList = getdraftProject;
                model.inspection = getInspection;
                model.FullPhotoPath = Properties.Settings.Default.FullPhotoPath;
                model.DocumentInfoList = getInspection.DocumentInfo.ToList<DocumentInfo>();
                var inspection = db.Inspection.Where(x=>x.Id== Id).ToList();
                model.InspectionList = inspection;
                var getpayment = getproject.Payment.ToList();
                model.paymentlist = getpayment;
                var inspectionofficer = db.UserDetail.Where(x => x.UserId == getproject.InspectionUserId).ToList();
                if(inspectionofficer.Any())
                {
                    model.userDetail = inspectionofficer;
                }
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
        [ValidateInput(false)]
        public FileResult Detail(string GridHtml)
        {
            ProjectViewModel model = new ProjectViewModel();
            var getdraftProject = db.ProjectApplication.Where(x => x.TransactionId == model.project.TransactionId).ToList();
            var getInspection = db.Inspection.Where(x => x.Id == model.inspection.Id).FirstOrDefault();
            model.projectList = getdraftProject;
            model.inspection = getInspection;
            model.FullPhotoPath = Properties.Settings.Default.FullPhotoPath;
            model.DocumentInfoList = getInspection.DocumentInfo.ToList<DocumentInfo>();
            var inspection = db.Inspection.Where(x => x.Id == getInspection.Id).ToList();
            model.InspectionList = inspection;
            var getpayment = model.project.Payment.ToList();
            model.paymentlist = getpayment;
            var inspectionofficer = db.UserDetail.Where(x => x.UserId == model.project.InspectionUserId).ToList();
            if (inspectionofficer.Any())
            {
                model.userDetail = inspectionofficer;
            }

            using (MemoryStream stream = new System.IO.MemoryStream())
            {
                StringReader sr = new StringReader(GridHtml);
                Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 100f, 0f);
                PdfWriter writer = PdfWriter.GetInstance(pdfDoc, stream);
               
                pdfDoc.Open();
                XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, sr);
                pdfDoc.Add(new Paragraph("Here"));
                pdfDoc.Close();
                return File(stream.ToArray(), "application/pdf", "detail.pdf");
            }
        }
  
        public ActionResult AssignOfficer(Guid Id)
        {
            try
            {
                ProjectViewModel model = new ProjectViewModel();
                var getproject = db.ProjectApplication.Where(x => x.TransactionId == Id).FirstOrDefault();
                if (getproject == null)
                {
                    TempData["message"] = Settings.Default.GenericExceptionMessage;
                    TempData["messageType"] = "danger";
                    return RedirectToAction("Index", "InspDashboard", new { area = "Admin" });
                }
                model.project = getproject;
                if(getproject.InspectionUserId != null)
                {
                    model.HasAssigned = true;
                }
                else
                {
                    model.HasAssigned = false;
                }
               
                var getdraftProject = db.ProjectApplication.Where(x => x.TransactionId == Id).ToList();
                model.projectList = getdraftProject;
                model.FullPhotoPath = Properties.Settings.Default.FullPhotoPath;
                model.DocumentInfoList = getproject.DocumentInfo.ToList<DocumentInfo>();

                
                #region put all users of inspection officer role in dropdown
                var inspectionUser = new List<Users>();
                var rl = db.Roles.SingleOrDefault(x => x.RoleName == Properties.Settings.Default.InspectionUser);
                if (rl != null)
                    if (rl.Users != null)
                        inspectionUser = rl.Users.ToList();
                var InspUser = new SelectList(inspectionUser, "UserName", "UserName");
                model.InspectionUser = InspUser;
                #endregion

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
        public ActionResult AssignOfficer(ProjectViewModel model)
        {
            try
            {
                var getproject = db.ProjectApplication.Where(x => x.TransactionId == model.project.TransactionId).FirstOrDefault();
                if (getproject == null)
                {
                    TempData["message"] = Settings.Default.GenericExceptionMessage;
                    TempData["messageType"] = "danger";
                    return RedirectToAction("Index", "InspDashboard", new { area = "Admin" });
                }
                model.project = getproject;
                var getUser = db.Users.Where(x => x.UserName == model.username).FirstOrDefault();
                var getdraftProject = db.ProjectApplication.Where(x => x.TransactionId == model.project.TransactionId).ToList();
                model.projectList = getdraftProject;
                model.FullPhotoPath = Properties.Settings.Default.FullPhotoPath;
                model.DocumentInfoList = getproject.DocumentInfo.ToList<DocumentInfo>();
                var user = getUser.UserDetail.ToList();
                model.userDetail = user;

                #region put all users of inspection officer role in dropdown
                var inspectionUser = new List<Users>();
                var rl = db.Roles.SingleOrDefault(x => x.RoleName == Properties.Settings.Default.InspectionUser);
                if (rl != null)
                    if (rl.Users != null)
                        inspectionUser = rl.Users.ToList();
                var InspUser = new SelectList(inspectionUser, "UserName", "UserName");
                model.InspectionUser = InspUser;
                #endregion

                getproject.InspectionUserId = getUser.UserId;
                db.SaveChanges();
                TempData["message"] = "The Monitoring officer has been assign successfully to the project above";           
                return RedirectToAction("AssignOfficer", "Project", new {Id=getproject.TransactionId, area = "Admin" });
            }
            catch(Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                TempData["message"] = Settings.Default.GenericExceptionMessage;
                TempData["messageType"] = "danger";
                return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
            }
        }

        public ActionResult RemoveUser(Guid Id)
        {
            try
            {
                ProjectViewModel model = new ProjectViewModel();
                var getproject = db.ProjectApplication.Where(x => x.TransactionId == Id).FirstOrDefault();
                if (getproject == null)
                {
                    TempData["message"] = Settings.Default.GenericExceptionMessage;
                    TempData["messageType"] = "danger";
                    return RedirectToAction("Index", "InspDashboard", new { area = "Admin" });
                }
                model.project = getproject;
                var getUser = db.Users.Where(x => x.UserId == getproject.InspectionUserId).FirstOrDefault();
                var getdraftProject = db.ProjectApplication.Where(x => x.TransactionId == Id).ToList();
                model.projectList = getdraftProject;
                model.FullPhotoPath = Properties.Settings.Default.FullPhotoPath;
                model.DocumentInfoList = getproject.DocumentInfo.ToList<DocumentInfo>();
                var user = getUser.UserDetail.ToList();
                model.userDetail = user;

                #region put all users of inspection officer role in dropdown
                var inspectionUser = new List<Users>();
                var rl = db.Roles.SingleOrDefault(x => x.RoleName == Properties.Settings.Default.InspectionUser);
                if (rl != null)
                    if (rl.Users != null)
                        inspectionUser = rl.Users.ToList();
                var InspUser = new SelectList(inspectionUser, "UserName", "UserName");
                model.InspectionUser = InspUser;
                #endregion

                getproject.InspectionUserId = null;
                db.SaveChanges();
                TempData["message"] = "The Monitoring officer has been removed successfully to the project above";
                return RedirectToAction("AssignOfficer", "Project", new { Id = getproject.TransactionId, area = "Admin" });
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                TempData["message"] = Settings.Default.GenericExceptionMessage;
                TempData["messageType"] = "danger";
                return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
            }
        }

        public ActionResult InspectionList(int Id)
        {
            try
            {
                ProjectViewModel model = new ProjectViewModel();
                var getproject = db.ProjectApplication.Where(x => x.Id == Id).FirstOrDefault();
                if (getproject == null)
                {
                    TempData["message"] = Settings.Default.GenericExceptionMessage;
                    TempData["messageType"] = "danger";
                    return RedirectToAction("Index", "InspDashboard", new { area = "Admin" });
                }
                model.project = getproject;
                var getdraftProject = db.ProjectApplication.Where(x => x.Id == Id).ToList();
                model.projectList = getdraftProject;
                var inspection = getproject.Inspection.ToList();
                model.InspectionList = inspection;
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

        public ActionResult NewInspection(int Id)
        {
            try
            {
                ProjectViewModel model = new ProjectViewModel();               
                var getproject = db.ProjectApplication.Where(x => x.Id == Id).FirstOrDefault();
                if (getproject == null)
                {
                    TempData["message"] = Settings.Default.GenericExceptionMessage;
                    TempData["messageType"] = "danger";
                    return RedirectToAction("Index", "InspDashboard", new { area = "Admin" });
                }
                model.project = getproject;
                var getdraftProject = db.ProjectApplication.Where(x => x.Id == Id).ToList();
                model.projectList = getdraftProject;
                model.StateList = (from s in db.State where s.IsDeleted == false select new IntegerSelectListItem { Text = s.Name, Value = s.Id }).ToList();
                model.LgaList = (from d in db.LGA where d.IsDeleted == false select new IntegerSelectListItem { Value = d.Id, Text = d.Name }).ToList();
                model.StageOfCompletion = (from s in db.StageOfCompletion where s.IsDeleted == false && s.ProjectTypeId==getproject.ProjectTypeId select new IntegerSelectListItem { Text = s.Percentage, Value = s.Id }).ToList();
                model.StageOfCompletionList = (from s in db.StageOfCompletion where s.IsDeleted == false && s.ProjectTypeId == getproject.ProjectTypeId select new IntegerSelectListItem { Text = s.Description, Value = s.Id }).ToList();
                model.inspectionForm = new InspectionForm();
                model.inspectionForm.Location = getproject.Location;
                model.inspectionForm.LGAId = getproject.LGAId;
                model.inspectionForm.ProjectId = Id;                
                model.StateId = getproject.LGA.StateId;
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
        public ActionResult NewInspection(ProjectViewModel model)
        {
            try
            {
                var getproject = db.ProjectApplication.Where(x => x.Id == model.project.Id).FirstOrDefault();
                if (getproject == null)
                {
                    TempData["message"] = Settings.Default.GenericExceptionMessage;
                    TempData["messageType"] = "danger";
                    return RedirectToAction("Index", "InspDashboard", new { area = "Admin" });
                }
                model.project = getproject;
                var getdraftProject = db.ProjectApplication.Where(x => x.Id == model.project.Id).ToList();
                model.projectList = getdraftProject;
                model.StateList = (from s in db.State where s.IsDeleted == false select new IntegerSelectListItem { Text = s.Name, Value = s.Id }).ToList();
                model.LgaList = (from d in db.LGA select new IntegerSelectListItem { Value = d.Id, Text = d.Name }).ToList();
                model.StageOfCompletion = (from s in db.StageOfCompletion where s.IsDeleted == false && s.ProjectTypeId == getproject.ProjectTypeId select new IntegerSelectListItem { Text = s.Percentage, Value = s.Id }).ToList();
                model.StageOfCompletionList = (from s in db.StageOfCompletion where s.IsDeleted == false && s.ProjectTypeId == getproject.ProjectTypeId select new IntegerSelectListItem { Text = s.Description, Value = s.Id }).ToList();
                //if (ModelState.IsValid)
                //{
                    var getstage = db.StageOfCompletion.Where(x => x.Id == model.inspectionForm.StageOfCompletionId).FirstOrDefault();
                    Inspection addnew = new Inspection
                    {
                        TransactionId = Guid.NewGuid(),
                        InspectionStatus = "Not Submitted",
                        Location = model.inspectionForm.Location,
                        Coordinate = model.inspectionForm.Coordinate,
                        LgaId = model.inspectionForm.LGAId,
                        StageOfCompletion = getstage.Percentage,
                        DescriptionOfCompletion = getstage.Description,
                        StageOfCompletionId = model.inspectionForm.StageOfCompletionId,
                        ProjectQuality = model.inspectionForm.ProjectQuality,
                        HasDefect = model.inspectionForm.HasDefect,
                        DescriptionOfDefect = model.inspectionForm.DescriptionOfDefect,
                        ModifiedBy = User.Identity.Name,
                        ModifiedDate = DateTime.Now, 
                        InspectionDate = DateTime.Now,
                        ProjectId = model.inspectionForm.ProjectId
                    };
                    db.Inspection.AddObject(addnew);
                    db.SaveChanges();
                    TempData["message"] = "The report has been added successfully. Kindly upload photo of the project.";
                    return RedirectToAction("InspectionList", "Project", new { Id = model.project.Id, area = "Admin" });
                //}
                //TempData["message"] = "ERROR: Please enter all fields with the * sign";
                //TempData["messageType"] = "danger";
                //return View(model);
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                TempData["message"] = Settings.Default.GenericExceptionMessage;
                TempData["messageType"] = "danger";
                return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
            }
        }
        public ActionResult EditInspection(int Id)
        {
            try
            {
                ProjectViewModel model = new ProjectViewModel();

                var getInspection = db.Inspection.Where(x => x.Id == Id).FirstOrDefault();
                if (getInspection == null)
                {
                    TempData["message"] = Settings.Default.GenericExceptionMessage;
                    TempData["messageType"] = "danger";
                    return RedirectToAction("Index", "InspDashboard", new { area = "Admin" });
                }

                var getproject = db.ProjectApplication.Where(x => x.Id == getInspection.ProjectId).FirstOrDefault();
                if (getproject == null)
                {
                    TempData["message"] = Settings.Default.GenericExceptionMessage;
                    TempData["messageType"] = "danger";
                    return RedirectToAction("Index", "InspDashboard", new { area = "Admin" });
                }
                model.project = getproject;
                var getdraftProject = db.ProjectApplication.Where(x => x.Id == getInspection.ProjectId).ToList();
                model.projectList = getdraftProject;
                model.StateList = (from s in db.State where s.IsDeleted == false select new IntegerSelectListItem { Text = s.Name, Value = s.Id }).ToList();
                model.LgaList = (from d in db.LGA where d.IsDeleted == false select new IntegerSelectListItem { Value = d.Id, Text = d.Name }).ToList();
                model.StageOfCompletion = (from s in db.StageOfCompletion where s.IsDeleted == false && s.ProjectTypeId == getproject.ProjectTypeId select new IntegerSelectListItem { Text = s.Percentage, Value = s.Id }).ToList();
                model.StageOfCompletionList = (from s in db.StageOfCompletion where s.IsDeleted == false && s.ProjectTypeId == getproject.ProjectTypeId select new IntegerSelectListItem { Text = s.Description, Value = s.Id }).ToList();
                model.inspectionForm = new InspectionForm();
                model.inspectionForm.Location = getInspection.Location;
                model.inspectionForm.Coordinate = getInspection.Coordinate;
                model.inspectionForm.LGAId = getInspection.LgaId;
                model.StateId = getInspection.LGA.StateId;
                model.inspectionForm.ProjectId = getInspection.ProjectId;
                model.inspectionForm.StageOfCompletionId = getInspection.StageOfCompletionId;
                model.StageOfCompletionId = getInspection.StageOfCompletionId;
                model.inspectionForm.ProjectQuality = getInspection.ProjectQuality;
                model.inspectionForm.HasDefect = getInspection.HasDefect;
                model.inspectionForm.DescriptionOfDefect = getInspection.DescriptionOfDefect;
                model.inspectionForm.Id = Id;
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
        public ActionResult EditInspection(ProjectViewModel model)
        {
            try
            {
                var getInspection = db.Inspection.Where(x => x.Id == model.inspectionForm.Id).FirstOrDefault();
                if (getInspection == null)
                {
                    TempData["message"] = Settings.Default.GenericExceptionMessage;
                    TempData["messageType"] = "danger";
                    return RedirectToAction("Index", "InspDashboard", new { area = "Admin" });
                }
                var getproject = db.ProjectApplication.Where(x => x.Id == getInspection.ProjectId).FirstOrDefault();
                if (getproject == null)
                {
                    TempData["message"] = Settings.Default.GenericExceptionMessage;
                    TempData["messageType"] = "danger";
                    return RedirectToAction("Index", "InspDashboard", new { area = "Admin" });
                }
                model.project = getproject;
                var getdraftProject = db.ProjectApplication.Where(x => x.Id == getInspection.ProjectId).ToList();
                model.projectList = getdraftProject;
                model.StateList = (from s in db.State where s.IsDeleted == false select new IntegerSelectListItem { Text = s.Name, Value = s.Id }).ToList();
                model.LgaList = (from d in db.LGA select new IntegerSelectListItem { Value = d.Id, Text = d.Name }).ToList();
                model.StageOfCompletion = (from s in db.StageOfCompletion where s.IsDeleted == false && s.ProjectTypeId == getproject.ProjectTypeId select new IntegerSelectListItem { Text = s.Percentage, Value = s.Id }).ToList();
                model.StageOfCompletionList = (from s in db.StageOfCompletion where s.IsDeleted == false && s.ProjectTypeId == getproject.ProjectTypeId select new IntegerSelectListItem { Text = s.Description, Value = s.Id }).ToList();
                //if (ModelState.IsValid)
                //{
                var getstage = db.StageOfCompletion.Where(x => x.Id == model.inspectionForm.StageOfCompletionId).FirstOrDefault();
                    getInspection.Location = model.inspectionForm.Location;
                    getInspection.Coordinate = model.inspectionForm.Coordinate;
                    getInspection.LgaId = model.inspectionForm.LGAId;
                    getInspection.StageOfCompletion = getstage.Percentage;
                    getInspection.StageOfCompletionId = model.inspectionForm.StageOfCompletionId;
                    getInspection.DescriptionOfCompletion = getstage.Description; //model.inspectionForm.DescriptionOfCompletion;
                    getInspection.ProjectQuality = model.inspectionForm.ProjectQuality;
                    getInspection.HasDefect = model.inspectionForm.HasDefect;
                    getInspection.DescriptionOfDefect = model.inspectionForm.DescriptionOfDefect;
                    getInspection.ModifiedBy = User.Identity.Name;
                    getInspection.ModifiedDate = DateTime.Now;
                    db.SaveChanges();
                    TempData["message"] = "The report has been update successfully.";
                    return RedirectToAction("InspectionList", "Project", new { Id = getInspection.ProjectId, area = "Admin" });
                //}
                //TempData["message"] = "ERROR: Please enter all fields with the * sign";
                //TempData["messageType"] = "danger";
                //return View(model);
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                TempData["message"] = Settings.Default.GenericExceptionMessage;
                TempData["messageType"] = "danger";
                return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
            }
        }

        public ActionResult SubmitInspection(int Id)
        {
            try
            {
                ProjectViewModel model = new ProjectViewModel();
                var getInspection = db.Inspection.Where(x => x.Id == Id).FirstOrDefault();
                if (getInspection == null)
                {
                    TempData["message"] = Settings.Default.GenericExceptionMessage;
                    TempData["messageType"] = "danger";
                    return RedirectToAction("Index", "InspDashboard", new { area = "Admin" });
                }

                var getproject = db.ProjectApplication.Where(x => x.Id == getInspection.ProjectId).FirstOrDefault();
                if (getproject == null)
                {
                    TempData["message"] = Settings.Default.GenericExceptionMessage;
                    TempData["messageType"] = "danger";
                    return RedirectToAction("Index", "InspDashboard", new { area = "Admin" });
                }
                getInspection.InspectionStatus = "Submitted";
                db.SaveChanges();
                TempData["message"] = "The report has been submitted successfully.";
                return RedirectToAction("InspectionList", "Project", new { Id = getInspection.ProjectId, area = "Admin" });
            }
            catch(Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                TempData["message"] = Settings.Default.GenericExceptionMessage;
                TempData["messageType"] = "danger";
                return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
            }
        }

        public ActionResult ManageDeliverable(Guid Id)
        {
            try
            {
                ProjectViewModel model = new ProjectViewModel();
                var getproject = db.ProjectApplication.Where(x => x.TransactionId == Id).FirstOrDefault();
                if (getproject == null)
                {
                    TempData["message"] = Settings.Default.GenericExceptionMessage;
                    TempData["messageType"] = "danger";
                    return RedirectToAction("Index", "InspDashboard", new { area = "Admin" });
                }
                model.project = getproject;
                var getdraftProject = db.ProjectApplication.Where(x => x.TransactionId == Id).ToList();
                var getworkflow = db.Workflow.Where(x => x.Id == getproject.WorkFlowId).FirstOrDefault();
                var getdeliverable = getworkflow.DeliverableType.ToList();
                model.deliverableType = getdeliverable;
                model.projectList = getdraftProject;

                List<int> list = (from x in getworkflow.DeliverableType select x.Id).ToList<int>();
              

                model.deliverableTypeList = (from s in db.DeliverableType where list.Contains(s.Id) select new IntegerSelectListItem { Text = s.Name, Value = s.Id }).ToList();
                //model.deliverableFormatList = (from d in db.DeliverableFormat where list.Contains(d.DeliverableTypeId) select new IntegerSelectListItem { Value = d.Id, Text = d.Name }).ToList();
                
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
        public ActionResult AddDeliverable(ProjectViewModel model)
        {
            try
            {
                var getproject = db.ProjectApplication.Where(x => x.TransactionId == model.project.TransactionId).FirstOrDefault();
                if (getproject == null)
                {
                    TempData["message"] = Settings.Default.GenericExceptionMessage;
                    TempData["messageType"] = "danger";
                    return RedirectToAction("Index", "InspDashboard", new { area = "Admin" });
                }

                var deliverable = db.DeliverableType.Where(x => x.Id == model.deliverable.Id).FirstOrDefault();
                if (deliverable == null)
                {
                    TempData["message"] = Settings.Default.GenericExceptionMessage;
                    TempData["messageType"] = "danger";
                    return RedirectToAction("Index", "InspDashboard", new { area = "Admin" });
                }
                model.deliverable = deliverable;
                model.project = getproject;
                var getdraftProject = db.ProjectApplication.Where(x => x.TransactionId == model.project.TransactionId).ToList();
                var getworkflow = db.Workflow.Where(x => x.Id == getproject.WorkFlowId).FirstOrDefault();
                var getdeliverable = getworkflow.DeliverableType.ToList();
                model.deliverableType = getdeliverable;
                model.projectList = getdraftProject;

                List<int> list = (from x in db.ProjectDeliverable where x.ProjectId == getproject.Id && x.DeliverableId == model.deliverable.Id select x.DeliverableFormatId).ToList<int>();
                model.deliverableFormatList = (from d in deliverable.DeliverableFormat where !list.Contains(d.Id) select new IntegerSelectListItem { Value = d.Id, Text = d.Name }).ToList();
                if (ModelState.IsValid)
                {
                    ProjectDeliverable addnew = new ProjectDeliverable
                    {
                        ProjectId = model.project.Id,
                        DeliverableId = model.deliverable.Id,
                        DeliverableFormatId = model.projectDeliverableForm.DeliverableFormatId,
                        DeliverableUnit = model.projectDeliverableForm.DeliverableUnit,
                        Remarks = model.projectDeliverableForm.Remarks,
                        CreatedBy = User.Identity.Name,
                        CreatedDate = DateTime.Now
                    };
                    db.ProjectDeliverable.AddObject(addnew);
                    db.SaveChanges();

                    base.TempData["message"] = "The Deliverable has been added successfully.";
                    return RedirectToAction("AddDeliverable", "Project", new { Id = model.project.TransactionId, DId = model.deliverable.Id, area = "Admin" });
                }
                TempData["message"] ="ERROR: Please select the deliverable type and enter unit and a remarks.";
                TempData["messageType"] = "danger";
                return RedirectToAction("AddDeliverable", "Project", new {Id=getproject.TransactionId,DId=deliverable.Id, area = "Admin" });

            }
            catch(Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                TempData["message"] = Settings.Default.GenericExceptionMessage;
                TempData["messageType"] = "danger";
                return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
            }
        }

        public ActionResult AddDeliverable(Guid Id,int DId)
        {
            try
            {
                ProjectViewModel model = new ProjectViewModel();
                var getproject = db.ProjectApplication.Where(x => x.TransactionId == Id).FirstOrDefault();
                if (getproject == null)
                {
                    TempData["message"] = Settings.Default.GenericExceptionMessage;
                    TempData["messageType"] = "danger";
                    return RedirectToAction("Index", "InspDashboard", new { area = "Admin" });
                }
                var deliverable = db.DeliverableType.Where(x => x.Id == DId).FirstOrDefault();
                if (deliverable == null)
                {
                    TempData["message"] = Settings.Default.GenericExceptionMessage;
                    TempData["messageType"] = "danger";
                    return RedirectToAction("Index", "InspDashboard", new { area = "Admin" });
                }
                model.deliverable = deliverable;
                model.project = getproject;
                var getdraftProject = db.ProjectApplication.Where(x => x.TransactionId == Id).ToList();
                var getworkflow = db.Workflow.Where(x => x.Id == getproject.WorkFlowId).FirstOrDefault();
                var getdeliverable = getworkflow.DeliverableType.ToList();
                model.deliverableType = getdeliverable;
                model.projectList = getdraftProject;
                model.ProjectDeliverableList = getproject.ProjectDeliverable.Where(x => x.DeliverableId == DId).ToList();

                List<int> list = (from x in db.ProjectDeliverable where x.ProjectId==getproject.Id && x.DeliverableId==DId select x.DeliverableFormatId).ToList<int>();
                model.deliverableFormatList = (from d in deliverable.DeliverableFormat where !list.Contains(d.Id) select new IntegerSelectListItem { Value = d.Id, Text = d.Name }).ToList();
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

        public ActionResult RemoveDeliverable(int Id, int DId)
        {
            ActionResult action;
            try
            {
                ProjectViewModel model = new ProjectViewModel();
                var getproject = (from d in db.ProjectApplication where d.Id == Id select d).FirstOrDefault();
                var deliverable = (from x in db.ProjectDeliverable where x.ProjectId == getproject.Id && x.DeliverableId== DId select x).FirstOrDefault();
                ProjectDeliverable remove = new ProjectDeliverable
                {
                };
                db.ProjectDeliverable.DeleteObject(deliverable);
                db.SaveChanges();
                TempData["message"] = "The Deliverable has been deleted successfully.";
                action = base.RedirectToAction("AddDeliverable", "Project", new { Id = getproject.TransactionId,DId=DId, area = "Admin" });
            }
            catch (Exception exception1)
            {
                Exception exception = exception1;
                base.TempData["messageType"] = "danger";
                base.TempData["message"] = "There is an error in the application. Please try again or contact the system administrator";
                Elmah.ErrorSignal.FromCurrentContext().Raise(exception);
                action = base.RedirectToAction("Index", "Dashboard", new { area = "Admin" });
            }
            return action;
        }


    }

}
