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
    [Authorize(Roles = "Administrator, Inspection Officer")]
    public class DashboardController : Controller
    {
        //
        // GET: /Admin/Dashboard/
        private PROEntities db = new PROEntities();
        private IMembershipService membershipService;
        public DashboardController()
        {

            this.membershipService = new MembershipService(Membership.Provider);

        }
        public ActionResult Index()
        {
           try
            {
                DashboardViewModel model = new DashboardViewModel();
                string username = Membership.GetUser().UserName;
                string[] roles = System.Web.Security.Roles.GetRolesForUser(User.Identity.Name);
                if (Roles.GetRolesForUser(User.Identity.Name).Contains("Administrator"))
                {
                    return View(model);
                }
                else if (Roles.GetRolesForUser(User.Identity.Name).Contains("Inspection Officer"))
                {
                    return RedirectToAction("Index", "InspDashboard", new { area = "Admin" });
                }
                TempData["message"] = Settings.Default.GenericExceptionMessage;
                TempData["messageType"] = "danger";
                return RedirectToAction("Login", "SGAccount", new { area = "" });
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
