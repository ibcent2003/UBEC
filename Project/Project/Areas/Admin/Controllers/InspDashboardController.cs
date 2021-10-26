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
    [Authorize(Roles = "Inspection Officer")]
    public class InspDashboardController : Controller
    {       
        // GET: /Admin/InspDashboard/
        private PROEntities db = new PROEntities();
        private IMembershipService membershipService;
        public InspDashboardController()
        {
            this.membershipService = new MembershipService(Membership.Provider);
        }

        public ActionResult Index()
        {
            return View();
        }

    }
}
