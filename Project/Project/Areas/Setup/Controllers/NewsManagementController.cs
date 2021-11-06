using Project.Areas.Setup.Models;
using Project.DAL;
using Project.Models;
using Project.Properties;
using Project.UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project.Areas.Setup.Controllers
{
    public class NewsManagementController : Controller
    {        
        private PROEntities db = new PROEntities();
        private ProcessUtility util;
      
        public NewsManagementController()
        {
            this.util = new ProcessUtility();
           
        }
        public ActionResult Index()
        {
            try
            {
                var rowsToShow = db.News.ToList();
                var viewModel = new NewsManagementViewModel
                {
                    Rows = rowsToShow.OrderByDescending(x => x.ModifiedDate).ToList(),
                };
                return View(viewModel);
            }
            catch (Exception ex)
            {
                TempData["messageType"] = "alert-danger";
                TempData["message"] = "There is an error in the application. Please try again or contact the system administrator";
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
            }
        }

        public ActionResult Create()
        {
            try
            {
                NewsManagementViewModel model = new NewsManagementViewModel();                
                return View(model);
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                TempData["messageType"] = "danger";
                TempData["message"] = Settings.Default.GenericExceptionMessage;
                return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
            }
        }
 
        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Create(NewsManagementViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {


                    var validate = (from m in db.News where m.NewsHeadline == model.newsform.NewsHeadline select m).ToList();
                    if (validate.Any())
                    {
                       
                        TempData["messageType"] = "danger";
                        TempData["message"] = "The News Headline" + model.newsform.NewsHeadline + " already exist. Please try different headline";
                        return View(model);
                    }

                    string url = Settings.Default.FullPhotoPath;
                    System.IO.Directory.CreateDirectory(url);

                    #region upload news photo validation

                    int max_upload = 5242880;

                    UI.Models.CodeGenerator CodePassport = new UI.Models.CodeGenerator();
                    string EncKey = util.MD5Hash(DateTime.Now.Ticks.ToString());
                    List<Project.DAL.DocumentFormat> Resumetypes = db.DocumentType.FirstOrDefault(x => x.DocumentCategoryId == 1).DocumentFormat.ToList();

                    List<string> supportedNewPhoto = new List<string>();
                    foreach (var item in Resumetypes)
                    {
                        supportedNewPhoto.Add(item.Extension);
                    }
                    var newphoto = System.IO.Path.GetExtension(model.newsform.Photo.FileName);
                    if (!supportedNewPhoto.Contains(newphoto.ToLower()))
                    {
                        TempData["messageType"] = "danger";
                        TempData["message"] = "Invalid type. Only the following type " + String.Join(",", supportedNewPhoto) + " are supported for News Photo ";
                        return View(model);

                    }

                    if (model.newsform.Photo.ContentLength > max_upload)
                    {

                        TempData["messageType"] = "danger";
                        TempData["message"] = "The Photo uploaded is larger than the 5MB upload limit";
                        return View(model);
                    }
                    #endregion

                    #region save news photo
                    int i = 0;
                    string filename;
                    filename = EncKey + i.ToString() + System.IO.Path.GetExtension(model.newsform.Photo.FileName);
                    model.newsform.Photo.SaveAs(url + filename);
                    #endregion

                    //add to news table 

                    News add = new News
                        {
                            NewsHeadline = model.newsform.NewsHeadline,
                            NewsContent = model.newsform.NewsContent,                  
                            Photo = filename,
                            CreatedBy = User.Identity.Name,                            
                            CreatedDate = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd")),
                            IsDeleted = model.newsform.IsDeleted,
                            IsPublished = model.newsform.IsPublished,
                            ModifiedBy = User.Identity.Name,
                            ModifiedDate = DateTime.Now,
                            NoOfView = 0
                        };
                        db.News.AddObject(add);
                        db.SaveChanges();
                        TempData["message"] = "<b>" + model.newsform.NewsHeadline + "</b> was Successfully created";
                        return RedirectToAction("Index");

                }
                return View(model);
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                TempData["messageType"] = "danger";
                TempData["message"] = Settings.Default.GenericExceptionMessage;
                return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
            }
        }

        public ActionResult Edit(int Id)
        {
            try
            {
                NewsManagementViewModel model = new NewsManagementViewModel();              
                var GetNews = db.News.Where(x => x.Id == Id).FirstOrDefault();
                model.news = GetNews;
                model.newsform = new  NewsForm();              
                model.newsform.NewsHeadline = model.news.NewsHeadline;              
                model.newsform.NewsContent = model.news.NewsContent;              
                model.newsform.IsPublished = model.news.IsPublished;
                model.newsform.IsDeleted = model.news.IsDeleted;
                model.newsform.Id = Id;
                model.PicturePath = Properties.Settings.Default.FullPhotoPath;
                //model.PicturePath = "/Content/Backend/News/";
                return View(model);
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                TempData["messageType"] = "danger";
                TempData["message"] = Settings.Default.GenericExceptionMessage;
                return RedirectToAction("Index", "Home", new { area = "Admin" });
            }
        }

        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(NewsManagementViewModel model)
        {
            try
            {
                var GetNews = db.News.Where(x => x.Id == model.newsform.Id).FirstOrDefault();              
                model.news = GetNews;
                if (model.newsform.Photo != null && model.newsform.Photo.ContentLength > 0)
                {
                    string url = Settings.Default.FullPhotoPath;
                    System.IO.Directory.CreateDirectory(url);

                    #region upload news photo validation

                    int max_upload = 5242880;

                    UI.Models.CodeGenerator CodePassport = new UI.Models.CodeGenerator();
                    string EncKey = util.MD5Hash(DateTime.Now.Ticks.ToString());
                    List<Project.DAL.DocumentFormat> Resumetypes = db.DocumentType.FirstOrDefault(x => x.DocumentCategoryId == 1).DocumentFormat.ToList();

                    List<string> supportedNewPhoto = new List<string>();
                    foreach (var item in Resumetypes)
                    {
                        supportedNewPhoto.Add(item.Extension);
                    }
                    var newphoto = System.IO.Path.GetExtension(model.newsform.Photo.FileName);
                    if (!supportedNewPhoto.Contains(newphoto.ToLower()))
                    {
                        TempData["messageType"] = "danger";
                        TempData["message"] = "Invalid type. Only the following type " + String.Join(",", supportedNewPhoto) + " are supported for News Photo ";
                        return View(model);

                    }

                    if (model.newsform.Photo.ContentLength > max_upload)
                    {

                        TempData["messageType"] = "danger";
                        TempData["message"] = "The Photo uploaded is larger than the 5MB upload limit";
                        return View(model);
                    }
                    #endregion

                    if (model.news.Photo != null)
                    {
                        System.IO.FileInfo fi = new System.IO.FileInfo(url + model.news.Photo);
                        fi.Delete();
                    }

                    #region save Resume
                    int i = 0;
                    string filename;
                    filename = EncKey + i.ToString() + System.IO.Path.GetExtension(model.newsform.Photo.FileName);
                    model.newsform.Photo.SaveAs(url + filename);
                    
                    #endregion

                    GetNews.NewsHeadline = model.newsform.NewsHeadline;                   
                    GetNews.NewsContent = model.newsform.NewsContent;
                  
                    GetNews.Photo = filename;
                    GetNews.ModifiedBy = User.Identity.Name;
                    GetNews.ModifiedDate = DateTime.Now;
                    GetNews.IsDeleted = model.newsform.IsDeleted;
                    db.SaveChanges();
                    TempData["message"] = "<b>" + model.newsform.NewsHeadline + "</b> was Successfully updated";
                    return RedirectToAction("Index");
                }
                else
                {
                    GetNews.NewsHeadline = model.newsform.NewsHeadline;                   
                    GetNews.NewsContent = model.newsform.NewsContent;
                 
                    GetNews.ModifiedBy = User.Identity.Name;
                    GetNews.ModifiedDate = DateTime.Now;
                    GetNews.IsDeleted = model.newsform.IsDeleted;
                    db.SaveChanges();
                    TempData["message"] = "<b>" + model.newsform.NewsHeadline + "</b> was Successfully updated";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                TempData["messageType"] = "danger";
                TempData["message"] = Settings.Default.GenericExceptionMessage;
                return RedirectToAction("Index", "Home", new { area = "Admin" });
            }
        }

        [Authorize]
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
    }
}
