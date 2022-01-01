using Project.DAL;
using Project.Models;
using Project.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project.Controllers
{
   
    public class HomeController : Controller
    {
        private PROEntities db = new PROEntities();
        public ActionResult Index(IndexViewModel model)
        {
            try
            {
                var news = db.News.Where(x => x.IsPublished == true && x.IsDeleted == false).ToList();
                model.NewsList = news;
                var getinspectionlist = db.Inspection.Where(x => x.InspectionStatus == "Approved").ToList();
                model.inspectionlist = getinspectionlist;
                model.PicturePath = Properties.Settings.Default.FullPhotoPath;
                return View(model);
            }
            catch (Exception ex)
            {
                TempData["messageType"] = "alert-danger";
                TempData["message"] = Settings.Default.GenericExceptionMessage;
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                return RedirectToAction("Error404");
            }
           
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            try
            {
                IndexViewModel model = new IndexViewModel();
                
                return View(model);
            }
            catch(Exception ex)
            {
                TempData["messageType"] = "alert-danger";
                TempData["message"] = Settings.Default.GenericExceptionMessage;
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                return RedirectToAction("Error404");
            }
        }

        [HttpPost]
        public ActionResult Contact(IndexViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Feedback addnew = new Feedback
                    {
                        FullName = model.feedbackForm.FullName,
                        EmailAddress = model.feedbackForm.EmailAddress,
                        MobileNo = model.feedbackForm.MobileNo,
                        Comment = model.feedbackForm.Comment,
                        IsReply = false,
                        SentDate = DateTime.Now
                    };
                    db.Feedback.AddObject(addnew);
                    db.SaveChanges();                   
                    TempData["message"] = "Your feedback has been submitted successfully. We'll contact you within 5 working day. Thank you.";                  
                    return RedirectToAction("FeedbackSent");
                }
                TempData["messageType"] = "alert-danger";
                TempData["message"] = "Error: Please make sure you enter all fields";
              
                return View(model);
            }
            catch(Exception ex)
            {
                TempData["messageType"] = "alert-danger";
                TempData["message"] = Settings.Default.GenericExceptionMessage;
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                return RedirectToAction("Error404");
            }
        }

        public ActionResult FeedbackSent()
        {
            try
            {
                return View();
            }
            catch(Exception ex)
            {
                TempData["messageType"] = "alert-danger";
                TempData["message"] = Settings.Default.GenericExceptionMessage;
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                return RedirectToAction("Error404");
            }
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

        public ActionResult MoreNews(IndexViewModel model)
        {
            try
            {
                var news = db.News.Where(x => x.IsPublished == true && x.IsDeleted == false).OrderByDescending(x=>x.CreatedDate).ToList();
                model.NewsList = news;
                model.PicturePath = Properties.Settings.Default.FullPhotoPath;
                return View(model);
            }
            catch(Exception ex)
            {
                TempData["messageType"] = "alert-danger";
                TempData["message"] = Settings.Default.GenericExceptionMessage;
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                return RedirectToAction("Error404");
            }
        }
        public ActionResult Error404()
        {
            return View();
        }

        public ActionResult DownloadApk()
        {
            IndexViewModel model = new IndexViewModel();
            model.PicturePath = Properties.Settings.Default.ApkPath;
            model.apkfileName = "app-debug.apk";
            return View(model);
        }

        public ActionResult ViewNews(int Id)
        {
            try
            {
                IndexViewModel model = new IndexViewModel();
                var getNews = db.News.Where(x => x.Id == Id).FirstOrDefault();
                model.news = getNews;
                model.PicturePath = Properties.Settings.Default.FullPhotoPath;
                return View(model);

            }
            catch(Exception ex)
            {
                TempData["messageType"] = "alert-danger";
                TempData["message"] = Settings.Default.GenericExceptionMessage;
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                return RedirectToAction("Error404");
            }
        }

        [HttpPost]
        public ActionResult GetLga(int stateId)
        {
            List<IntegerSelectListItem> ListLga = (from d in db.LGA where d.StateId == stateId && d.IsDeleted == false orderby d.Name select new IntegerSelectListItem { Text = d.Name, Value = d.Id }).ToList();
            return Json(ListLga);
        }

        public ActionResult ViewProject(int Id)
        {
            try
            {
                IndexViewModel model = new IndexViewModel();              
                var getworkflow = db.Workflow.Where(x => x.Id == Id).FirstOrDefault();
                if(getworkflow != null)
                {
                    var getproject = db.ProjectApplication.Where(x => x.WorkFlowId == Id  && x.Status=="Approved" && x.IsDeleted==false).ToList();
                    model.projectList = getproject;
                    model.workflow = getworkflow;                    
                    return View(model);
                }
                else
                {
                    TempData["messageType"] = "alert-danger";
                    TempData["message"] = Settings.Default.GenericExceptionMessage;
                   // Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                    return RedirectToAction("Error404");
                }
               
            }
            catch(Exception ex)
            {
                TempData["messageType"] = "alert-danger";
                TempData["message"] = Settings.Default.GenericExceptionMessage;
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                return RedirectToAction("Error404");
            }
        }

        public ActionResult ViewReport(Guid Id)
        {
            try
            {
                IndexViewModel model = new IndexViewModel();
                var getproject = db.ProjectApplication.Where(x => x.TransactionId == Id).FirstOrDefault();
                var getInspection = db.Inspection.Where(x => x.ProjectId == getproject.Id && x.InspectionStatus == "Approved").ToList();
                model.inspectionlist = getInspection;
                model.project = getproject;
                model.PicturePath = Properties.Settings.Default.FullPhotoPath;
                if (getproject.EnableSum == true)
                {
                    model.EnableSum = true;
                }
                else
                {
                    model.EnableSum = false;
                }
                return View(model);
            }
            catch(Exception ex)
            {
                TempData["messageType"] = "alert-danger";
                TempData["message"] = Settings.Default.GenericExceptionMessage;
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                return RedirectToAction("Error404");
            }
        }


        public ActionResult ProjectType()
        {
            try
            {
                IndexViewModel model = new IndexViewModel();
                var getProject = db.Workflow.Where(x => x.IsDeleted == false).ToList();
                model.ProjectTypes = getProject;
                return View(model);

            }
            catch(Exception ex)
            {
                TempData["messageType"] = "alert-danger";
                TempData["message"] = Settings.Default.GenericExceptionMessage;
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                return RedirectToAction("Error404");
            }
        }
        public ActionResult SupplyList()
        {
            try
            {
                IndexViewModel model = new IndexViewModel();
                var getworkflow = db.Workflow.Where(x => x.Id == Properties.Settings.Default.Supply).FirstOrDefault();
                if (getworkflow != null)
                {
                    var getsupply = db.Supplies.Where(x => x.WorkflowId == Properties.Settings.Default.Supply && x.Status == "Approved").ToList();
                    model.SupplyList = getsupply;

                    model.workflow = getworkflow;
                    return View(model);
                }
                else
                {
                    TempData["messageType"] = "alert-danger";
                    TempData["message"] = Settings.Default.GenericExceptionMessage;
                   
                    return RedirectToAction("Error404");
                }
            }
            catch(Exception ex)
            {
                TempData["messageType"] = "alert-danger";
                TempData["message"] = Settings.Default.GenericExceptionMessage;
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                return RedirectToAction("Error404");
            }
        }
        public ActionResult ViewSupply(Guid Id)
        {
            try
            {
                IndexViewModel model = new IndexViewModel();
                var getsupply = db.Supplies.Where(x => x.TransactionId == Id).FirstOrDefault();
                var getItem = getsupply.SupplyItems.ToList();
                model.supplyItems = getItem;
                model.Supply = getsupply;
                model.PicturePath = Properties.Settings.Default.FullPhotoPath;
                return View(model);
            }
            catch(Exception ex)
            {
                TempData["messageType"] = "alert-danger";
                TempData["message"] = Settings.Default.GenericExceptionMessage;
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                return RedirectToAction("Error404");
            }
        }
        public ActionResult Search(int  Id)
        {
            try
            {
                IndexViewModel model = new IndexViewModel();
                var getworkflow = db.Workflow.Where(x => x.Id == Id).FirstOrDefault();
                if (getworkflow == null)
                {                  
                    return RedirectToAction("Error404");
                }
                model.workflow = getworkflow;
                model.StateList = (from s in db.State where s.IsDeleted == false select new IntegerSelectListItem { Text = s.Name, Value = s.Id }).ToList();
                model.LgaList = (from d in db.LGA select new IntegerSelectListItem { Value = d.Id, Text = d.Name }).ToList();
                return View(model);
            }
            catch(Exception ex)
            {
                TempData["messageType"] = "alert-danger";
                TempData["message"] = Settings.Default.GenericExceptionMessage;
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                return RedirectToAction("Error404");
            }
        }
        public ActionResult ViewResult(IndexViewModel model)
        {
            try
            {
                var getworkflow = db.Workflow.Where(x => x.Id == model.workflow.Id).FirstOrDefault();
                if (getworkflow != null)
                {
                    var getproject = db.ProjectApplication.Where(x => x.WorkFlowId == getworkflow.Id && x.LGA.State.Id==model.serachForm.StateId && x.LGAId==model.serachForm.LgaId && x.IsDeleted == false).ToList();
                    model.projectList = getproject;
                    model.workflow = getworkflow;
                    return View(model);
                }
                else
                {
                    TempData["messageType"] = "alert-danger";
                    TempData["message"] = Settings.Default.GenericExceptionMessage;                   
                    return RedirectToAction("Error404");
                }              
            }
            catch(Exception ex)
            {
                TempData["messageType"] = "alert-danger";
                TempData["message"] = Settings.Default.GenericExceptionMessage;
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                return RedirectToAction("Error404");
            }
        }
    }
}
