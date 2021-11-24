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
    public class DeliverableTypeController : Controller
    {
        private PROEntities db = new PROEntities();
        public ActionResult Index()
        {
            try
            {
                DeliverableTypeViewModel model = new DeliverableTypeViewModel();
                var GetdeliverableType = db.DeliverableType.ToList();
                model.DeliverableTypeList = GetdeliverableType;
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

        public ActionResult NewDeliverableType()
        {
            try
            {
                DeliverableTypeViewModel model = new DeliverableTypeViewModel();
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
        public ActionResult NewDeliverableType(DeliverableTypeViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //check for duplicate
                    var validate = db.DeliverableType.Where(x => x.Name == model.DeliverableTypeform.Name).ToList();
                    if (validate.Any())
                    {
                        TempData["message"] = "The Deliverable Type " + model.DeliverableTypeform.Name + " already exist. Please enter different name";
                        TempData["messageType"] = "danger";
                        return View(model);
                    }
                    DeliverableType add = new DeliverableType
                    {                       
                        Name = model.DeliverableTypeform.Name,
                        ModifiedBy = User.Identity.Name,
                        ModifiedDate = DateTime.Now,
                        IsDeleted = model.DeliverableTypeform.IsDeleted
                    };
                    db.DeliverableType.AddObject(add);
                    db.SaveChanges();
                    TempData["message"] = "" + model.DeliverableTypeform.Name + " has been added successful.";
                    return RedirectToAction("Index");

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


        public ActionResult EditDeliverableType(int Id)
        {
            try
            {
                DeliverableTypeViewModel model = new DeliverableTypeViewModel();
                var getdeliverable = db.DeliverableType.Where(x => x.Id == Id).FirstOrDefault();
                if(getdeliverable==null)
                {
                    TempData["message"] = Settings.Default.GenericExceptionMessage;
                    TempData["messageType"] = "danger";
                    return RedirectToAction("Index");
                }              
                model.DeliverableTypeform = new DeliverableTypeForm();
                model.DeliverableTypeform.Id = Id;
                model.DeliverableTypeform.Name = getdeliverable.Name;
                model.DeliverableTypeform.IsDeleted = getdeliverable.IsDeleted;
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
        public ActionResult EditDeliverableType(DeliverableTypeViewModel model)
        {
            try
            {
                
                var getdeliverable = db.DeliverableType.Where(x => x.Id == model.DeliverableTypeform.Id).FirstOrDefault();
                if (getdeliverable == null)
                {
                    TempData["message"] = Settings.Default.GenericExceptionMessage;
                    TempData["messageType"] = "danger";
                    return RedirectToAction("Index");
                }
                getdeliverable.Name = model.DeliverableTypeform.Name;
                getdeliverable.IsDeleted = model.DeliverableTypeform.IsDeleted;
                getdeliverable.ModifiedBy = User.Identity.Name;
                getdeliverable.ModifiedDate = DateTime.Now;
                db.SaveChanges();
                TempData["message"] = "" + model.DeliverableTypeform.Name + " has been updated successful.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                TempData["message"] = Settings.Default.GenericExceptionMessage;
                TempData["messageType"] = "danger";
                return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
            }
        }

        public ActionResult DeliverableFormatList()
        {
            try
            {
                DeliverableTypeViewModel model = new DeliverableTypeViewModel();
                var Getdeliverableformat = db.DeliverableFormat.ToList();
                model.DeliverableFormatList = Getdeliverableformat;
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

        public ActionResult NewDeliverableFormat()
        {
            try
            {
                DeliverableTypeViewModel model = new DeliverableTypeViewModel();
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
        public ActionResult NewDeliverableFormat(DeliverableTypeViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //check for duplicate
                    var validate = db.DeliverableFormat.Where(x => x.Name == model.DeliverableFormatform.Name).ToList();
                    if (validate.Any())
                    {
                        TempData["message"] = "The Deliverable Format " + model.DeliverableFormatform.Name + " already exist. Please enter different name";
                        TempData["messageType"] = "danger";
                        return View(model);
                    }
                    DeliverableFormat add = new DeliverableFormat
                    {
                        Name = model.DeliverableFormatform.Name,
                        ModifiedBy = User.Identity.Name,
                        ModifiedDate = DateTime.Now,
                        IsDeleted = model.DeliverableFormatform.IsDeleted
                    };
                    db.DeliverableFormat.AddObject(add);
                    db.SaveChanges();
                    TempData["message"] = "" + model.DeliverableFormatform.Name + " has been added successful.";
                    return RedirectToAction("DeliverableFormatList");

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

        public ActionResult EditDeliverableFormat(int Id)
        {
            try
            {
                DeliverableTypeViewModel model = new DeliverableTypeViewModel();
                var getdeliverableformat = db.DeliverableFormat.Where(x => x.Id == Id).FirstOrDefault();
                if (getdeliverableformat == null)
                {
                    TempData["message"] = Settings.Default.GenericExceptionMessage;
                    TempData["messageType"] = "danger";
                    return RedirectToAction("Index");
                }
                model.DeliverableFormatform = new  DeliverableFormatForm();
                model.DeliverableFormatform.Id = Id;
                model.DeliverableFormatform.Name = getdeliverableformat.Name;
                model.DeliverableFormatform.IsDeleted = getdeliverableformat.IsDeleted;
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
        public ActionResult EditDeliverableFormat(DeliverableTypeViewModel model)
        {
            try
            {

                var getdeliverableformat = db.DeliverableFormat.Where(x => x.Id == model.DeliverableFormatform.Id).FirstOrDefault();
                if (getdeliverableformat == null)
                {
                    TempData["message"] = Settings.Default.GenericExceptionMessage;
                    TempData["messageType"] = "danger";
                    return RedirectToAction("Index");
                }
                getdeliverableformat.Name = model.DeliverableFormatform.Name;
                getdeliverableformat.IsDeleted = model.DeliverableFormatform.IsDeleted;
                getdeliverableformat.ModifiedBy = User.Identity.Name;
                getdeliverableformat.ModifiedDate = DateTime.Now;
                db.SaveChanges();
                TempData["message"] = "" + model.DeliverableFormatform.Name + " has been updated successful.";
                return RedirectToAction("DeliverableFormatList");
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                TempData["message"] = Settings.Default.GenericExceptionMessage;
                TempData["messageType"] = "danger";
                return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
            }
        }

        public ActionResult DeliverableTypeFormat(int Id)
        {
            try
            {
                DeliverableTypeViewModel model = new DeliverableTypeViewModel();
                var deliverable = db.DeliverableType.Where(x => x.Id == Id).FirstOrDefault();
                if (null == deliverable)
                {
                    base.TempData["messageType"] = "danger";
                    base.TempData["message"] = "";
                    return RedirectToAction("Index");
                }
                model.deliverableType = deliverable;
                List<int> list = (from x in deliverable.DeliverableFormat select x.Id).ToList<int>();
                model.AvailableDeliverableFormat = (from d in this.db.DeliverableFormat where !list.Contains(d.Id) && d.IsDeleted == false select d).ToList<DeliverableFormat>();
                model.DeliverableFormatList = deliverable.DeliverableFormat.ToList();
                return View(model);
            }
            catch (Exception ex)
            {
                Exception exception = ex;
                base.TempData["messageType"] = "danger";
                base.TempData["message"] = "There is an error in the application. Please try again or contact the system administrator";
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
            }
        }

        [HttpPost]
        public ActionResult DeliverableTypeFormat(DeliverableTypeViewModel model)
        {
            try
            {
                var deliverable = db.DeliverableType.Where(x => x.Id == model.deliverableType.Id).FirstOrDefault();
                if (null == deliverable)
                {
                    base.TempData["messageType"] = "danger";
                    base.TempData["message"] = "";
                    return RedirectToAction("Index");
                }
                model.deliverableType = deliverable;
                List<int> list = (from x in deliverable.DeliverableFormat select x.Id).ToList<int>();
                model.AvailableDeliverableFormat = (from d in this.db.DeliverableFormat where !list.Contains(d.Id) && d.IsDeleted == false select d).ToList<DeliverableFormat>();
                model.DeliverableFormatList = deliverable.DeliverableFormat.ToList();
                if (model.DeliverableFormatId == 0)
                {
                    TempData["messageType"] = "danger";
                    TempData["message"] = "Please select a Deliverable type from the dropdownlist and click on the Add button";
                    return RedirectToAction("DeliverableTypeFormat", "DeliverableType", new { Id = model.deliverableType.Id, area = "Setup" });
                }
                if (!(from x in deliverable.DeliverableFormat where x.Id == model.DeliverableFormatId select x).ToList<DeliverableFormat>().Any<DeliverableFormat>())
                {
                    var getdoc = db.DeliverableFormat.Where(x => x.Id == model.DeliverableFormatId).FirstOrDefault();

                    deliverable.DeliverableFormat.FirstOrDefault<DeliverableFormat>();
                    deliverable.DeliverableFormat.Add(getdoc);
                    this.db.SaveChanges();

                    base.TempData["message"] = "The Document has been added successfully.";
                    RedirectToAction("DeliverableTypeFormat", "DeliverableType", new { Id = model.deliverableType.Id, area = "Setup" });

                    List<int> listed = (from x in deliverable.DeliverableFormat select x.Id).ToList<int>();
                    model.AvailableDeliverableFormat = (from d in this.db.DeliverableFormat where !listed.Contains(d.Id) && d.IsDeleted == false select d).ToList<DeliverableFormat>();
                    model.DeliverableFormatList = deliverable.DeliverableFormat.ToList();
                }
                else
                {
                    base.TempData["messageType"] = "danger";
                    base.TempData["message"] = "The Selected Deliverable format already exist. Please select another Deliverable format";
                    return View(model);
                }
                return View(model);
            }
            catch (Exception ex)
            {
                Exception exception = ex;
                base.TempData["messageType"] = "danger";
                base.TempData["message"] = "There is an error in the application. Please try again or contact the system administrator";
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
            }
        }

        public ActionResult RemoveDelverableTypeFormat(int Id, int DeliverableFormatId)
        {
            ActionResult action;
            try
            {
                DeliverableTypeViewModel model = new DeliverableTypeViewModel();
                var deliverayTypeId = (from d in this.db.DeliverableType where d.Id == Id select d).FirstOrDefault();
                var formatId = (from x in this.db.DeliverableFormat where x.Id == DeliverableFormatId select x).FirstOrDefault();
                deliverayTypeId.DeliverableFormat.Remove(formatId);
                this.db.SaveChanges();
                base.TempData["message"] = "The Deliverable format has been deleted successfully.";
                action = base.RedirectToAction("DeliverableTypeFormat", "DeliverableType", new { Id = Id, area = "Setup" });
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
