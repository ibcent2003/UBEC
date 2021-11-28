using Project.Areas.Admin.Models;
using Project.DAL;
using Project.UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Project.Properties;
using System.ComponentModel;
using Project.Models;

namespace Project.Areas.Admin.Controllers
{
    public class SupplyManagementController : Controller
    {
        //
        // GET: /Admin/SupplyManagement/
        private PROEntities db = new PROEntities();
        private ProcessUtility util;

        public SupplyManagementController()
        {
            this.util = new ProcessUtility();

        }
        public ActionResult Index()
        {
            try
            {
                SupplyViewModel model = new SupplyViewModel();
                var getworkflow = db.Workflow.Where(x => x.Id == Properties.Settings.Default.Supply).FirstOrDefault();
                model.workflow = getworkflow;
                var getuser = db.Users.Where(x => x.UserName == User.Identity.Name).FirstOrDefault();
                var getsupply = db.Supplies.Where(x => x.WorkflowId == Properties.Settings.Default.Supply).ToList();
                model.SupplyList = getsupply;
                model.user = getuser;
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


        public ActionResult ApproveSupply(int Id)
        {
            try
            {
                var getSupply = db.Supplies.Where(x => x.Id == Id).FirstOrDefault();
                if (getSupply == null)
                {
                    TempData["message"] = Settings.Default.GenericExceptionMessage;
                    TempData["messageType"] = "danger";
                    return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
                }

               
                SupplyViewModel model = new SupplyViewModel();
                getSupply.Status = "Approved";
                db.SaveChanges();
                TempData["message"] = "Report has been approved successfully.";
                return RedirectToAction("Detail", "SupplyManagement", new { Id = getSupply.TransactionId, area = "Admin" });
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                TempData["message"] = Settings.Default.GenericExceptionMessage;
                TempData["messageType"] = "danger";
                return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
            }
        }

        public ActionResult DisapproveSupply(int Id)
        {
            try
            {
                var getSupply = db.Supplies.Where(x => x.Id == Id).FirstOrDefault();
                if (getSupply == null)
                {
                    TempData["message"] = Settings.Default.GenericExceptionMessage;
                    TempData["messageType"] = "danger";
                    return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
                }


                SupplyViewModel model = new SupplyViewModel();
                getSupply.Status = "Not Submitted";
                db.SaveChanges();
                TempData["message"] = "Report has been Disapproved successfully.";
                return RedirectToAction("Detail", "SupplyManagement", new { Id = getSupply.TransactionId, area = "Admin" });
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
            List<IntegerSelectListItem> ListLga = (from d in db.LGA where d.StateId == stateId && d.IsDeleted == false orderby d.Name select new IntegerSelectListItem { Text = d.Name, Value = d.Id }).ToList();
            return Json(ListLga);
        }
        public ActionResult NewSupply()
        {
            try
            {
                SupplyViewModel model = new SupplyViewModel();
                model.StateList = (from s in db.State where s.IsDeleted == false select new IntegerSelectListItem { Text = s.Name, Value = s.Id }).ToList();
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
        public ActionResult NewSupply(SupplyViewModel model)
        {
            try
            {                
                model.StateList = (from s in db.State where s.IsDeleted == false select new IntegerSelectListItem { Text = s.Name, Value = s.Id }).ToList();
                model.LgaList = (from d in db.LGA select new IntegerSelectListItem { Value = d.Id, Text = d.Name }).ToList();
                model.ContractorList = (from d in db.Contractor select new IntegerSelectListItem { Value = d.Id, Text = d.Name }).ToList();
                if (ModelState.IsValid)
                {
                    var validate = db.Supplies.Where(x => x.SerialNo == model.supplyForm.SerialNo).ToList();
                    if(validate.Any())
                    {
                        TempData["message"] = "The Serial No "+model.supplyForm.SerialNo+" already exist. Please try another Serial No.";
                        TempData["messageType"] = "danger";
                        return View(model);
                    }
                    Supplies addnew = new Supplies
                    {
                        TransactionId = Guid.NewGuid(),
                        SerialNo = model.supplyForm.SerialNo,
                        Status = "Submitted",
                        WorkflowId = Properties.Settings.Default.Supply,
                        Location = model.supplyForm.Location,
                        Coordinate = model.supplyForm.Coordinate,
                        LGAId = model.supplyForm.LGAId,
                        ContractorId = model.supplyForm.ContractorId,
                        VerificationOfficer = model.supplyForm.VerificationOfficer,
                        VerificationDate = model.supplyForm.VerificationDate,
                        Representative = model.supplyForm.RepresentativeName,
                        RepresentativePhoneNumber = model.supplyForm.RepresentativePhoneNumber,
                        RepresentativeDesignation = model.supplyForm.RepresentativeDesignation,
                        ModifiedBy = User.Identity.Name,
                        Modified = DateTime.Now,
                    };
                    db.Supplies.AddObject(addnew);
                    db.SaveChanges();
                    TempData["message"] = "The "+model.supplyForm.SerialNo+" has been submitted successfully.";
                    return RedirectToAction("Index", "SupplyManagement", new {area = "Admin" });

                }
                TempData["message"] = "There is error submitting project information. Please sure you enter all fields with * sign.";
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

        public ActionResult EditSupply(Guid Id)
        {
            try
            {
                SupplyViewModel model = new SupplyViewModel();
                model.StateList = (from s in db.State where s.IsDeleted == false select new IntegerSelectListItem { Text = s.Name, Value = s.Id }).ToList();
                model.LgaList = (from d in db.LGA select new IntegerSelectListItem { Value = d.Id, Text = d.Name }).ToList();
                model.ContractorList = (from d in db.Contractor select new IntegerSelectListItem { Value = d.Id, Text = d.Name }).ToList();

                var getSupply = db.Supplies.Where(x => x.TransactionId == Id).FirstOrDefault();
                if(getSupply== null)
                {
                    TempData["message"] = Settings.Default.GenericExceptionMessage;
                    TempData["messageType"] = "danger";
                    return RedirectToAction("Index");
                }
                model.supplyForm = new SupplyForm();
                model.supplyForm.SerialNo = getSupply.SerialNo;
                model.supplyForm.Location = getSupply.Location;
                model.StateId = getSupply.LGA.StateId;
                model.supplyForm.LGAId = getSupply.LGAId;
                model.supplyForm.ContractorId = getSupply.ContractorId;
                model.supplyForm.Coordinate = getSupply.Coordinate;
                model.supplyForm.RepresentativeName = getSupply.Representative;
                model.supplyForm.RepresentativeDesignation = getSupply.RepresentativeDesignation;
                model.supplyForm.RepresentativePhoneNumber = getSupply.RepresentativePhoneNumber;
                model.supplyForm.VerificationOfficer = getSupply.VerificationOfficer;
                model.supplyForm.VerificationDate = getSupply.VerificationDate;
                model.supplyForm.Id = getSupply.Id;
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
        public ActionResult EditSupply(SupplyViewModel model)
        {
            try
            {
                model.StateList = (from s in db.State where s.IsDeleted == false select new IntegerSelectListItem { Text = s.Name, Value = s.Id }).ToList();
                model.LgaList = (from d in db.LGA select new IntegerSelectListItem { Value = d.Id, Text = d.Name }).ToList();
                model.ContractorList = (from d in db.Contractor select new IntegerSelectListItem { Value = d.Id, Text = d.Name }).ToList();
                if (ModelState.IsValid)
                {
                    var getSupply = db.Supplies.Where(x => x.Id == model.supplyForm.Id).FirstOrDefault();
                    getSupply.SerialNo = model.supplyForm.SerialNo;
                    getSupply.Location = model.supplyForm.Location;
                    getSupply.Coordinate = model.supplyForm.Coordinate;
                    getSupply.LGAId = model.supplyForm.LGAId;
                    getSupply.ContractorId = model.supplyForm.ContractorId;
                    getSupply.VerificationOfficer = model.supplyForm.VerificationOfficer;
                    getSupply.VerificationDate = model.supplyForm.VerificationDate;
                    getSupply.Representative = model.supplyForm.RepresentativeName;
                    getSupply.RepresentativeDesignation = model.supplyForm.RepresentativeDesignation;
                    getSupply.RepresentativePhoneNumber = model.supplyForm.RepresentativePhoneNumber;
                    getSupply.Modified = DateTime.Now;
                    getSupply.ModifiedBy = User.Identity.Name;
                    db.SaveChanges();
                    TempData["message"] = "The " + model.supplyForm.SerialNo + " has been saved successfully.";
                    return RedirectToAction("Index", "SupplyManagement", new { area = "Admin" });
                }
                TempData["message"] = "There is error submitting project information. Please sure you enter all fields with * sign.";
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

        public ActionResult UploadPhoto(Guid Id)
        {
            try
            {
                SupplyViewModel model = new SupplyViewModel();
                var getsupply = db.Supplies.Where(x => x.WorkflowId == Properties.Settings.Default.Supply && x.TransactionId == Id).FirstOrDefault();
                if(getsupply==null)
                {
                    TempData["message"] = Settings.Default.GenericExceptionMessage;
                    TempData["messageType"] = "danger";
                    return RedirectToAction("Index");
                }
                var getworkflow = db.Workflow.Where(x => x.Id == Properties.Settings.Default.Supply).FirstOrDefault();
                model.Supply = getsupply;
                List<int> list = (from x in getsupply.DocumentInfo select x.DocumentTypeId).ToList<int>();
                model.AvailableDocument = (from d in getworkflow.DocumentType where !list.Contains(d.Id) && d.IsDeleted == false select d).ToList<DocumentType>();
                if (model.AvailableDocument.Count == 0)
                {
                    model.AllPhotoUploaded = true;
                }
                else
                {
                    model.AllPhotoUploaded = false;
                }
                model.DocumentInfoList = getsupply.DocumentInfo.ToList<DocumentInfo>();
                model.FullPhotoPath = Properties.Settings.Default.FullPhotoPath;
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
        public ActionResult UploadPhoto(SupplyViewModel model)
        {
            try
            {

                var getsupply = db.Supplies.Where(x => x.WorkflowId == Properties.Settings.Default.Supply && x.TransactionId == model.Supply.TransactionId).FirstOrDefault();
                if (getsupply == null)
                {
                    TempData["message"] = Settings.Default.GenericExceptionMessage;
                    TempData["messageType"] = "danger";
                    return RedirectToAction("Index", "InspDashboard", new { area = "Admin" });
                }
               
                var getworkflow = db.Workflow.Where(x => x.Id == getsupply.WorkflowId).FirstOrDefault();
                List<int> list = (from x in getsupply.DocumentInfo select x.Id).ToList<int>();
                model.AvailableDocument = (from d in getworkflow.DocumentType where !list.Contains(d.Id) && d.IsDeleted == false select d).ToList<DocumentType>();
                model.DocumentInfoList = getsupply.DocumentInfo.ToList<DocumentInfo>();
                model.FullPhotoPath = Properties.Settings.Default.FullPhotoPath;
               
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
                    if (getdoctype == null)
                    {
                        TempData["message"] = Settings.Default.GenericExceptionMessage;
                        TempData["messageType"] = "danger";
                        return RedirectToAction("Index", "InspDashboard", new { area = "Admin" });
                    }
                    DocumentInfo addnew = new DocumentInfo
                    {
                        Name = getdoctype.Name,
                        Path = filename,
                        DocumentTypeId = model.documentForm.DocumentTypeId,
                        Size = model.documentForm.Photo.ContentLength.ToString(),
                        Extension = System.IO.Path.GetExtension(model.documentForm.Photo.FileName),
                        IssuedDate = DateTime.Now,
                        ModifiedBy = User.Identity.Name,
                        ModifiedDate = DateTime.Now
                    };
                    db.DocumentInfo.AddObject(addnew);
                    getsupply.DocumentInfo.Add(addnew);
                    db.SaveChanges();
                    TempData["message"] = "Photo type <b>" + getdoctype.Name.ToUpper() + "</b> has been uploaded successful";
                    return RedirectToAction("UploadPhoto", "SupplyManagement", new { Id = getsupply.TransactionId, area = "Admin" });
                }
                TempData["message"] = "Error uploading photo. Please make sure you select the photo type and click on the browse button to browse a photo from your computer.";
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

        public ActionResult RemovePhoto(int Id, int DocumentId)
        {
            try
            {
                SupplyViewModel model = new SupplyViewModel();
                var getsupply = db.Supplies.Where(x => x.Id == Id).FirstOrDefault();
                if (getsupply == null)
                {
                    TempData["message"] = Settings.Default.GenericExceptionMessage;
                    TempData["messageType"] = "danger";
                    return RedirectToAction("Index", "InspDashboard", new { area = "Admin" });
                }

              
                var getworkflow = db.Workflow.Where(x => x.Id == getsupply.WorkflowId).FirstOrDefault();
                List<int> list = (from x in getsupply.DocumentInfo select x.Id).ToList<int>();
                model.AvailableDocument = (from d in getworkflow.DocumentType where !list.Contains(d.Id) && d.IsDeleted == false select d).ToList<DocumentType>();
                model.DocumentInfoList = getsupply.DocumentInfo.ToList<DocumentInfo>();
                model.FullPhotoPath = Properties.Settings.Default.FullPhotoPath;
                model.Supply = getsupply;
               

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
                getsupply.DocumentInfo.Remove(documentInfo);
                db.SaveChanges();
                TempData["message"] = "The photo has been deleted successful.";

                return RedirectToAction("UploadPhoto", "SupplyManagement", new { Id = getsupply.TransactionId, area = "Admin" });
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                TempData["message"] = Settings.Default.GenericExceptionMessage;
                TempData["messageType"] = "danger";
                return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
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


        public ActionResult ItemList(Guid Id)
        {
            try
            {
                var getsupply = db.Supplies.Where(x => x.TransactionId == Id).FirstOrDefault();
                if (getsupply == null)
                {
                    TempData["message"] = Settings.Default.GenericExceptionMessage;
                    TempData["messageType"] = "danger";
                    return RedirectToAction("Index");
                }
                SupplyViewModel model = new SupplyViewModel();
                var getitems = getsupply.SupplyItems.ToList();
                model.Supply = getsupply;
                model.SupplyItemList = getitems;
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

        public ActionResult NewItems(Guid Id)
        {
            try
            {
                var getsupply = db.Supplies.Where(x => x.TransactionId == Id).FirstOrDefault();
                if (getsupply == null)
                {
                    TempData["message"] = Settings.Default.GenericExceptionMessage;
                    TempData["messageType"] = "danger";
                    return RedirectToAction("Index");
                }
                SupplyViewModel model = new SupplyViewModel();
                model.Supply = getsupply;
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
        public ActionResult NewItems(SupplyViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var getsupply = db.Supplies.Where(x => x.TransactionId == model.Supply.TransactionId).FirstOrDefault();
                    var validate = getsupply.SupplyItems.Where(x => x.SerialNumber == model.SupplyItemsform.SerialNo).ToList();
                    if(validate.Any())
                    {

                        TempData["message"] = "The Serial No " + model.SupplyItemsform.SerialNo + " already exist. Please try another Serial No.";
                        TempData["messageType"] = "danger";
                        return View(model);
                    }
                    SupplyItems addnew = new SupplyItems
                    {
                       SupplyId = getsupply.Id,
                       SerialNumber = model.SupplyItemsform.SerialNo,
                       Description = model.SupplyItemsform.Description,
                       QuantityDelivered = model.SupplyItemsform.QuantityDelivered,
                       QuantityOrdered = model.SupplyItemsform.QuantityOrdered,
                       Remarks = model.SupplyItemsform.Remarks,
                       Modified = DateTime.Now,
                       ModifiedBy = User.Identity.Name,
                    };
                    db.SupplyItems.AddObject(addnew);
                    db.SaveChanges();
                    TempData["message"] = "The " + model.SupplyItemsform.SerialNo + " has been submitted successfully.";
                    return RedirectToAction("ItemList", "SupplyManagement", new {Id=getsupply.TransactionId, area = "Admin" });
                }
                TempData["message"] = "There is error submitting supply item. Please sure you enter all fields.";
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

        public ActionResult EditItem(int Id)
        {
            try
            {
                SupplyViewModel model = new SupplyViewModel();
                var getItems = db.SupplyItems.Where(x => x.Id == Id).FirstOrDefault();
                if(getItems==null)
                {
                    TempData["message"] = Settings.Default.GenericExceptionMessage;
                    TempData["messageType"] = "danger";
                    return RedirectToAction("Index");
                }
                model.SupplyItemsform = new SupplyItemsForm();
                model.SupplyItemsform.SerialNo = getItems.SerialNumber;
                model.SupplyItemsform.Description = getItems.Description;
                model.SupplyItemsform.QuantityDelivered = getItems.QuantityDelivered;
                model.SupplyItemsform.QuantityOrdered = getItems.QuantityOrdered;
                model.SupplyItemsform.Remarks = getItems.Remarks;
                model.SupplyItemsform.Id = getItems.Id;
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
        public ActionResult EditItem(SupplyViewModel model)
        {
            try
            {
                var getItems = db.SupplyItems.Where(x => x.Id == model.SupplyItemsform.Id).FirstOrDefault();
                if (getItems == null)
                {
                    TempData["message"] = Settings.Default.GenericExceptionMessage;
                    TempData["messageType"] = "danger";
                    return RedirectToAction("Index");
                }
                var getsupply = db.Supplies.Where(x => x.Id == getItems.SupplyId).FirstOrDefault();
                if(getsupply==null)
                {
                    TempData["message"] = Settings.Default.GenericExceptionMessage;
                    TempData["messageType"] = "danger";
                    return RedirectToAction("Index");
                }
                if (ModelState.IsValid)
                {
                    getItems.SerialNumber = model.SupplyItemsform.SerialNo;
                    getItems.Description = model.SupplyItemsform.Description;
                    getItems.QuantityDelivered = model.SupplyItemsform.QuantityDelivered;
                    getItems.QuantityOrdered = model.SupplyItemsform.QuantityOrdered;
                    getItems.Remarks = model.SupplyItemsform.Remarks;
                    getItems.ModifiedBy = User.Identity.Name;
                    getItems.Modified = DateTime.Now;
                    db.SaveChanges();
                    TempData["message"] = "The " + model.SupplyItemsform.SerialNo + " has been saved successfully.";
                    return RedirectToAction("ItemList", "SupplyManagement", new { Id = getsupply.TransactionId, area = "Admin" });
                }
                TempData["message"] = "There is error submitting supply item. Please sure you enter all fields.";
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

        public ActionResult RemoveItem(int Id)
        {
            try
            {
                SupplyViewModel model = new SupplyViewModel();
                var getsupplyitem = db.SupplyItems.Where(x => x.Id == Id).FirstOrDefault();
                if (getsupplyitem == null)
                {
                    TempData["message"] = Settings.Default.GenericExceptionMessage;
                    TempData["messageType"] = "danger";
                    return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
                }

                var getsupply = db.Supplies.Where(x => x.Id == getsupplyitem.SupplyId).FirstOrDefault();
                 if(getsupply == null)
                   {
                    TempData["message"] = Settings.Default.GenericExceptionMessage;
                    TempData["messageType"] = "danger";
                    return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
                }
                SupplyItems del = new SupplyItems
                {

                };
                db.SupplyItems.DeleteObject(getsupplyitem);
                db.SaveChanges();
                TempData["message"] = "The item has been deleted successful.";
                return RedirectToAction("ItemList", "SupplyManagement", new { Id = getsupply.TransactionId, area = "Admin" });


            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                TempData["message"] = Settings.Default.GenericExceptionMessage;
                TempData["messageType"] = "danger";
                return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
            }
        }


        public ActionResult SupplyDeliverableList(Guid Id)
        {
            try
            {
                SupplyViewModel model = new SupplyViewModel();
                var getworkflow = db.Workflow.Where(x => x.Id == Properties.Settings.Default.Supply).FirstOrDefault();
                model.workflow = getworkflow;
                var getSupply = db.Supplies.Where(x => x.TransactionId == Id).FirstOrDefault();
                if (getSupply == null)
                {
                    TempData["message"] = Settings.Default.GenericExceptionMessage;
                    TempData["messageType"] = "danger";
                    return RedirectToAction("Index");
                }
                var getdeliverable = getworkflow.DeliverableType.ToList();
                model.deliverableType = getdeliverable;
                model.Supply = getSupply;
                List<int> list = (from x in getworkflow.DeliverableType select x.Id).ToList<int>();


                model.deliverableTypeList = (from s in db.DeliverableType where list.Contains(s.Id) select new IntegerSelectListItem { Text = s.Name, Value = s.Id }).ToList();
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

        public ActionResult AddSupplyDeliverable(Guid Id, int DId)
        {
            try
            {
                SupplyViewModel model = new SupplyViewModel();
                var getSupply = db.Supplies.Where(x => x.TransactionId == Id).FirstOrDefault();
                if (getSupply == null)
                {
                    TempData["message"] = Settings.Default.GenericExceptionMessage;
                    TempData["messageType"] = "danger";
                    return RedirectToAction("Index");
                }
             

                var deliverable = db.DeliverableType.Where(x => x.Id == DId).FirstOrDefault();
                if (deliverable == null)
                {
                    TempData["message"] = Settings.Default.GenericExceptionMessage;
                    TempData["messageType"] = "danger";
                    return RedirectToAction("Index", "InspDashboard", new { area = "Admin" });
                }
                model.deliverable = deliverable;
                model.Supply = getSupply;
              
                var getworkflow = db.Workflow.Where(x => x.Id == getSupply.WorkflowId).FirstOrDefault();
                var getdeliverable = getworkflow.DeliverableType.ToList();
                model.deliverableType = getdeliverable;
               
                model.SupplyDeliverableList = getSupply.SupplyDeliverable.Where(x => x.DeliverableId == DId).ToList();

                List<int> list = (from x in db.SupplyDeliverable where x.SupplyId == getSupply.Id && x.DeliverableId == DId select x.DeliverableFormatId).ToList<int>();
                model.deliverableFormatList = (from d in deliverable.DeliverableFormat where !list.Contains(d.Id) select new IntegerSelectListItem { Value = d.Id, Text = d.Name }).ToList();
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
        public ActionResult AddSupplyDeliverable(SupplyViewModel model)
        {
            try
            {
                var getSupply = db.Supplies.Where(x => x.TransactionId == model.Supply.TransactionId).FirstOrDefault();
                if (getSupply == null)
                {
                    TempData["message"] = Settings.Default.GenericExceptionMessage;
                    TempData["messageType"] = "danger";
                    return RedirectToAction("Index");
                }

                var deliverable = db.DeliverableType.Where(x => x.Id == model.deliverable.Id).FirstOrDefault();
                if (deliverable == null)
                {
                    TempData["message"] = Settings.Default.GenericExceptionMessage;
                    TempData["messageType"] = "danger";
                    return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
                }
                model.deliverable = deliverable;
                model.Supply = getSupply;
               
                var getworkflow = db.Workflow.Where(x => x.Id == getSupply.WorkflowId).FirstOrDefault();
                var getdeliverable = getworkflow.DeliverableType.ToList();
                model.deliverableType = getdeliverable;

                List<int> list = (from x in db.SupplyDeliverable where x.SupplyId == getSupply.Id && x.DeliverableId == model.deliverable.Id select x.DeliverableFormatId).ToList<int>();
                model.deliverableFormatList = (from d in deliverable.DeliverableFormat where !list.Contains(d.Id) select new IntegerSelectListItem { Value = d.Id, Text = d.Name }).ToList();

               
                if (ModelState.IsValid)
                {
                    SupplyDeliverable addnew = new SupplyDeliverable
                    {
                        SupplyId = getSupply.Id,
                        DeliverableId = model.deliverable.Id,
                        DeliverableFormatId = model.projectDeliverableForm.DeliverableFormatId,
                        DeliverableUnit = model.projectDeliverableForm.DeliverableUnit,
                        Remarks = model.projectDeliverableForm.Remarks,
                        CreatedBy = User.Identity.Name,
                        CreatedDate = DateTime.Now
                    };
                    db.SupplyDeliverable.AddObject(addnew);
                    db.SaveChanges();

                    base.TempData["message"] = "The Deliverable has been added successfully.";
                    return RedirectToAction("AddSupplyDeliverable", "SupplyManagement", new { Id = model.Supply.TransactionId, DId = model.deliverable.Id, area = "Admin" });
                }
                TempData["message"] = "ERROR: Please select the deliverable type and enter unit and a remarks.";
                TempData["messageType"] = "danger";
                return RedirectToAction("AddSupplyDeliverable", "Project", new { Id = getSupply.TransactionId, DId = deliverable.Id, area = "Admin" });

            }
            catch (Exception ex)
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
                SupplyViewModel model = new SupplyViewModel();
                
                var deliverable = (from x in db.SupplyDeliverable where x.Id == Id && x.DeliverableId == DId select x).FirstOrDefault();
                var getsupply = (from d in db.Supplies where d.Id == deliverable.SupplyId select d).FirstOrDefault();
                SupplyDeliverable remove = new SupplyDeliverable
                {
                };
                db.SupplyDeliverable.DeleteObject(deliverable);
                db.SaveChanges();
                TempData["message"] = "The Deliverable has been deleted successfully.";
                action = base.RedirectToAction("AddSupplyDeliverable", "SupplyManagement", new { Id = getsupply.TransactionId, DId = DId, area = "Admin" });
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

        public ActionResult Detail(Guid Id)
        {
            try
            {
                SupplyViewModel model = new SupplyViewModel();
                var getsupply = db.Supplies.Where(x => x.WorkflowId == Properties.Settings.Default.Supply && x.TransactionId == Id).FirstOrDefault();
                if (getsupply == null)
                {
                    TempData["message"] = Settings.Default.GenericExceptionMessage;
                    TempData["messageType"] = "danger";
                    return RedirectToAction("Index");
                }
                var getworkflow = db.Workflow.Where(x => x.Id == Properties.Settings.Default.Supply).FirstOrDefault();
                model.Supply = getsupply;
                model.DocumentInfoList = getsupply.DocumentInfo.ToList<DocumentInfo>();
                model.FullPhotoPath = Properties.Settings.Default.FullPhotoPath;
                var getitems = getsupply.SupplyItems.ToList();              
                model.SupplyItemList = getitems;
                var getdeliverable = getworkflow.DeliverableType.ToList();
                model.deliverableType = getdeliverable;

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
