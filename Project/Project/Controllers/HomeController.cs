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
            return View();
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


        public ActionResult ViewProject(int Id)
        {
            try
            {
                IndexViewModel model = new IndexViewModel();
                var getworkflow = db.Workflow.Where(x => x.Id == Id).FirstOrDefault();
                if(getworkflow != null)
                {
                    var getproject = db.ProjectApplication.Where(x => x.WorkFlowId == Id && x.IsDeleted==false).ToList();
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
                var getInspection = db.Inspection.Where(x => x.ProjectId == getproject.Id && x.InspectionStatus=="Approved").ToList();
                model.inspectionlist = getInspection;
                model.project = getproject;
                model.PicturePath = Properties.Settings.Default.FullPhotoPath;
                //if(getproject.EnableSum==true)
                //{
                //    model.EnableSum = true;
                //}
                //else
                //{
                //    model.EnableSum = false;
                //}
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
    }
}
