using Project.Areas.Admin.Models;
using Project.DAL;
using Project.Properties;
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
    [Authorize(Roles = "Administrator")]
    public class FeedbackController : Controller
    {
        //
        // GET: /Admin/Feedback/
        private PROEntities db = new PROEntities();
        public ActionResult Index(string feedtype)
        {
            try
            {


                FeedbackViewModel model = new FeedbackViewModel();
                if(feedtype=="New")
                {
                    var getfeedback = db.Feedback.Where(x=>x.IsReply==false).ToList();
                    model.FeedbackList = getfeedback;
                   
                }
                if (feedtype == "Replied")
                {
                    var getfeedback = db.Feedback.Where(x => x.IsReply == true).ToList();
                    model.FeedbackList = getfeedback;

                }
                if (feedtype == "Deleted")
                {
                    var getfeedback = db.Feedback.Where(x => x.IsDeleted == true).ToList();
                    model.FeedbackList = getfeedback;

                }
                if (feedtype == "Total")
                {
                    var getfeedback = db.Feedback.ToList();
                    model.FeedbackList = getfeedback;

                }
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

        public ActionResult FeedbackInfo(int Id)
        {
            try
            {
                FeedbackViewModel model = new FeedbackViewModel();
                var getfeedback = db.Feedback.Where(x => x.Id == Id).FirstOrDefault();
                model.feedbackForm = new Project.Models.FeedbackForm();
                model.feedbackForm.Id = Id;
                model.feedbackForm.FullName = getfeedback.FullName;
                model.feedbackForm.EmailAddress = getfeedback.EmailAddress;
                model.feedbackForm.MobileNo = getfeedback.MobileNo;
                model.feedbackForm.Comment = getfeedback.Comment;
                model.feedbackForm.IsDeleted = getfeedback.IsDeleted;
                model.feedbackForm.IsRepled = getfeedback.IsReply;
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
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult FeedbackInfo(FeedbackViewModel model)
        {
            try
            {
                var getfeedback = db.Feedback.Where(x => x.Id == model.feedbackForm.Id).FirstOrDefault();
                getfeedback.IsDeleted = model.feedbackForm.IsDeleted;
                getfeedback.IsReply = model.feedbackForm.IsRepled;
                db.SaveChanges();
                TempData["message"] ="Feedback Info saved successfully.";
               
                return RedirectToAction("Index", "Feedback", new {feedtype="New", area = "Admin" });
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
