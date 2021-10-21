using Project.Areas.Setup.Models;
using Project.DAL;
using Project.Models;
using Project.Properties;
using Project.UI.Models;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project.Areas.Setup.Controllers
{
    public class EmployeeManagementController : Controller
    {
        //
        // GET: /Setup/Employee/
        private PROEntities db = new PROEntities();
        private ProcessUtility util;
       
        public EmployeeManagementController()
        {
            this.util = new ProcessUtility();
           // this.filePath = "~/Content/Backend/Photo/";
        }

        public ActionResult EmployeeTypeList()
        {
            try
            {
                EmployeeTypeViewModel model = new EmployeeTypeViewModel();
                var GetEmployType = db.EmployeeType.ToList();
                model.EmployeeTypeList = GetEmployType;
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

        public ActionResult NewEmployeeType()
        {
            try
            {
                EmployeeTypeViewModel model = new EmployeeTypeViewModel();               
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
        public ActionResult NewEmployeeType(EmployeeTypeViewModel model)
        {
            try
            {                
                if (ModelState.IsValid)
                {

                    var validate = db.EmployeeType.Where(x => x.Name == model.EmployeeTypeForm.Name).ToList();
                    if(validate.Any())
                    {
                        TempData["message"] = "ERROR: The employee type "+model.EmployeeTypeForm.Name+ " already exist. Please enter different name.";
                        TempData["messageType"] = "danger";
                        return View(model);
                    }
                  else
                    {                      
                        EmployeeType addnew = new EmployeeType
                        {
                            Name = model.EmployeeTypeForm.Name,
                            IsDeleted = model.EmployeeTypeForm.IsDeleted,
                            ModifiedBy = User.Identity.Name,
                            ModifiedDate = DateTime.Now
                        };
                        db.EmployeeType.AddObject(addnew);
                        db.SaveChanges();
                        TempData["message"] = "Employee Type has been added successful";
                        return RedirectToAction("EmployeeTypeList");

                    }                   

                }
                TempData["message"] = "There is Error while adding employee type";
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

        public ActionResult EditEmployeeType(int Id)
        {
            try
            {
                EmployeeTypeViewModel model = new EmployeeTypeViewModel();
                var getEmployeeType = db.EmployeeType.Where(x => x.Id == Id).FirstOrDefault();
                if(getEmployeeType != null)
                {
                    model.EmployeeTypeForm = new EmployeeTypeForm();
                    model.EmployeeTypeForm.Name = getEmployeeType.Name;
                    model.EmployeeTypeForm.IsDeleted = getEmployeeType.IsDeleted;
                    model.EmployeeTypeForm.Id = getEmployeeType.Id;
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
        public ActionResult EditEmployeeType(EmployeeTypeViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var getEmployeeType = db.EmployeeType.Where(x => x.Id == model.EmployeeTypeForm.Id).FirstOrDefault();
                    getEmployeeType.Name = model.EmployeeTypeForm.Name;
                    getEmployeeType.IsDeleted = model.EmployeeTypeForm.IsDeleted;
                    getEmployeeType.ModifiedBy = User.Identity.Name;
                    getEmployeeType.ModifiedDate = DateTime.Now;
                    db.SaveChanges();
                    TempData["message"] = "Employee Type has been updated successful";
                    return RedirectToAction("EmployeeTypeList");
                }
                TempData["message"] = "There is Error while adding employee type";
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

        public ActionResult EmployeeList()
        {
            try
            {
                EmployeeTypeViewModel model = new EmployeeTypeViewModel();
                var GetEmployee = db.Employee.ToList();
                model.EmployeeList = GetEmployee;
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

        public ActionResult NewEmployee()
        {
            try
            {
                EmployeeTypeViewModel model = new EmployeeTypeViewModel();
                model.ListEmployeeType = (from s in this.db.EmployeeType where s.IsDeleted == false select new IntegerSelectListItem(){Text = s.Name,Value = s.Id}).ToList<IntegerSelectListItem>();
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
        public ActionResult NewEmployee(EmployeeTypeViewModel model)
        {
            try
            {               
                model.ListEmployeeType = (from s in this.db.EmployeeType where s.IsDeleted == false select new IntegerSelectListItem() { Text = s.Name, Value = s.Id }).ToList<IntegerSelectListItem>();
                if (ModelState.IsValid)
                {
                    string url = Settings.Default.FullPhotoPath;
                    System.IO.Directory.CreateDirectory(url);

                    if (model.employeeForm.Photo != null && model.employeeForm.Photo.ContentLength > 0)
                    {
                        #region upload document

                        int max_upload = 5242880;

                        //string ResumePath = Server.MapPath(filePath);

                        UI.Models.CodeGenerator CodePassport = new UI.Models.CodeGenerator();
                        string EncKey = util.MD5Hash(DateTime.Now.Ticks.ToString());
                        List<Project.DAL.DocumentFormat> Resumetypes = db.DocumentType.FirstOrDefault(x => x.Id == 1).DocumentFormat.ToList();

                        List<string> supportedResume = new List<string>();
                        foreach (var item in Resumetypes)
                        {
                            supportedResume.Add(item.Extension);
                        }
                        var fileResume = System.IO.Path.GetExtension(model.employeeForm.Photo.FileName);
                        if (!supportedResume.Contains(fileResume.ToLower()))
                        {
                            model.ListEmployeeType = (from s in this.db.EmployeeType where s.IsDeleted == false select new IntegerSelectListItem() { Text = s.Name, Value = s.Id }).ToList<IntegerSelectListItem>();
                            TempData["messageType"] = "danger";
                            TempData["message"] = "Invalid type. Only the following type " + String.Join(",", supportedResume) + " are supported for document ";
                            return View(model);

                        }

                        if (model.employeeForm.Photo.ContentLength > max_upload)
                        {
                            model.ListEmployeeType = (from s in this.db.EmployeeType where s.IsDeleted == false select new IntegerSelectListItem() { Text = s.Name, Value = s.Id }).ToList<IntegerSelectListItem>();
                            TempData["messageType"] = "danger";
                            TempData["message"] = "The document uploaded is larger than the 5MB upload limit";
                            return View(model);
                        }
                        #endregion

                        #region save Resume
                        int i = 0;
                        string filename;
                        filename = EncKey + i.ToString() + System.IO.Path.GetExtension(model.employeeForm.Photo.FileName);
                        model.employeeForm.Photo.SaveAs(url + filename);
                        #endregion

                        Employee addnew = new Employee
                        {
                            FirstName = model.employeeForm.FirstName,
                            LastName = model.employeeForm.LastName,
                            MobileNo = model.employeeForm.MobileNo,
                            Email = model.employeeForm.EmailAddress,
                            Address = model.employeeForm.Address,
                            EmployeeTypeId = model.employeeForm.EmployeeTypeId,
                            Photo = filename,
                            ModifiedBy = User.Identity.Name,
                            ModifiedDate = DateTime.Now,
                            IsDeleted = model.employeeForm.IsDeleted
                        };
                        db.Employee.AddObject(addnew);
                        db.SaveChanges();
                        TempData["message"] = "<b>" + model.employeeForm.FirstName + "</b> has been added successful";
                        return RedirectToAction("EmployeeList");
                    }
                    else
                    {
                        Employee addnew = new Employee
                        {
                            FirstName = model.employeeForm.FirstName,
                            LastName = model.employeeForm.LastName,
                            MobileNo = model.employeeForm.MobileNo,
                            //Email = model.employeeForm.EmailAddress,
                            Address = model.employeeForm.Address,
                            EmployeeTypeId = model.employeeForm.EmployeeTypeId,
                            Photo = "passport.jpg",
                            ModifiedBy = User.Identity.Name,
                            ModifiedDate = DateTime.Now,
                            IsDeleted = model.employeeForm.IsDeleted
                        };
                        db.Employee.AddObject(addnew);
                        db.SaveChanges();
                        TempData["message"] = "<b>"+model.employeeForm.FirstName+ "</b> has been added successful";
                        return RedirectToAction("EmployeeList");
                    }
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

        public ActionResult EditEmployee(int Id)
        {
            try
            {
                EmployeeTypeViewModel model = new EmployeeTypeViewModel();
                model.ListEmployeeType = (from s in this.db.EmployeeType where s.IsDeleted == false select new IntegerSelectListItem() { Text = s.Name, Value = s.Id }).ToList<IntegerSelectListItem>();
                var getEmployee = db.Employee.Where(x => x.Id == Id).FirstOrDefault();
                if(getEmployee==null)
                {                    
                    TempData["message"] = "Ops! Something wrong. Please start the process of editting an employee.";
                    TempData["messageType"] = "danger";
                    return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
                }
                model.employeeForm = new EmployeeForm();
                model.employeeForm.Id = Id;
                model.employeeForm.FirstName = getEmployee.FirstName;
                model.employeeForm.LastName = getEmployee.LastName;
                model.employeeForm.MobileNo = getEmployee.MobileNo;
                model.employeeForm.EmailAddress = getEmployee.Email;
                model.UploadedPhoto = getEmployee.Photo;
                model.FullPhotoPath = Properties.Settings.Default.FullPhotoPath;
                model.employeeForm.EmployeeTypeId = getEmployee.EmployeeTypeId;
                model.employeeForm.Address = getEmployee.Address;
                model.employeeForm.IsDeleted = getEmployee.IsDeleted;
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
        public ActionResult EditEmployee(EmployeeTypeViewModel model)
        {
            try
            {
                
                model.ListEmployeeType = (from s in this.db.EmployeeType where s.IsDeleted == false select new IntegerSelectListItem() { Text = s.Name, Value = s.Id }).ToList<IntegerSelectListItem>();
                var getEmployee = db.Employee.Where(x => x.Id == model.employeeForm.Id).FirstOrDefault();
                if (getEmployee == null)
                {                   
                    TempData["message"] = "Ops! Something wrong. Please start the process of editting an employee.";
                    TempData["messageType"] = "danger";
                    return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
                }
                string url = Settings.Default.FullPhotoPath;
                System.IO.Directory.CreateDirectory(url);
                if (model.employeeForm.Photo != null && model.employeeForm.Photo.ContentLength > 0)
                {                   
                    if (getEmployee.Photo != null)
                    {
                        if(getEmployee.Photo=="passport.jpg")
                        {

                        }
                        else
                        {
                            System.IO.FileInfo fi = new System.IO.FileInfo(url + getEmployee.Photo);
                            fi.Delete();
                        }

                    }
                    #region upload document
                    int max_upload = 5242880;
                    UI.Models.CodeGenerator CodePassport = new UI.Models.CodeGenerator();
                    string EncKey = util.MD5Hash(DateTime.Now.Ticks.ToString());
                    List<Project.DAL.DocumentFormat> Resumetypes = db.DocumentType.FirstOrDefault(x => x.Id == 1).DocumentFormat.ToList();
                    List<string> supportedResume = new List<string>();
                    foreach (var item in Resumetypes)
                    {
                        supportedResume.Add(item.Extension);
                    }
                    var fileResume = System.IO.Path.GetExtension(model.employeeForm.Photo.FileName);
                    if (!supportedResume.Contains(fileResume.ToLower()))
                    {
                        model.ListEmployeeType = (from s in this.db.EmployeeType where s.IsDeleted == false select new IntegerSelectListItem() { Text = s.Name, Value = s.Id }).ToList<IntegerSelectListItem>();
                        TempData["messageType"] = "danger";
                        TempData["message"] = "Invalid type. Only the following type " + String.Join(",", supportedResume) + " are supported for document ";
                        return View(model);

                    }
                    if (model.employeeForm.Photo.ContentLength > max_upload)
                    {
                        model.ListEmployeeType = (from s in this.db.EmployeeType where s.IsDeleted == false select new IntegerSelectListItem() { Text = s.Name, Value = s.Id }).ToList<IntegerSelectListItem>();
                        TempData["messageType"] = "danger";
                        TempData["message"] = "The document uploaded is larger than the 5MB upload limit";
                        return View(model);
                    }
                    int pp = 0;
                    string pName;
                    pName = EncKey + pp.ToString() + System.IO.Path.GetExtension(model.employeeForm.Photo.FileName);
                    model.employeeForm.Photo.SaveAs(url + pName);
                    #endregion
                    getEmployee.FirstName = model.employeeForm.FirstName;
                    getEmployee.LastName = model.employeeForm.LastName;
                    getEmployee.MobileNo = model.employeeForm.MobileNo;
                    getEmployee.Email = model.employeeForm.EmailAddress;
                    getEmployee.Address = model.employeeForm.Address;
                    getEmployee.EmployeeTypeId = model.employeeForm.EmployeeTypeId;
                    getEmployee.IsDeleted = model.employeeForm.IsDeleted;
                    getEmployee.ModifiedBy = User.Identity.Name;
                    getEmployee.ModifiedDate = DateTime.Now;
                    getEmployee.Photo = pName;
                    db.Employee.Context.SaveChanges();
                    db.SaveChanges();
                    TempData["message"] = "<b>" + model.employeeForm.FirstName + "</b> information has been updated.";
                    return RedirectToAction("EmployeeList"); 

                }
                else
                {
                    getEmployee.FirstName = model.employeeForm.FirstName;
                    getEmployee.LastName = model.employeeForm.LastName;
                    getEmployee.MobileNo = model.employeeForm.MobileNo;
                    getEmployee.Email = model.employeeForm.EmailAddress;
                    getEmployee.Address = model.employeeForm.Address;
                    getEmployee.EmployeeTypeId = model.employeeForm.EmployeeTypeId;
                    getEmployee.IsDeleted = model.employeeForm.IsDeleted;
                    getEmployee.ModifiedBy = User.Identity.Name;
                    getEmployee.ModifiedDate = DateTime.Now;                    
                    db.Employee.Context.SaveChanges();
                    db.SaveChanges();
                    TempData["message"] = "<b>"+model.employeeForm.FirstName+ "</b> information has been updated.";
                    return RedirectToAction("EmployeeList");
                }

               
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                TempData["message"] = Settings.Default.GenericExceptionMessage;
                TempData["messageType"] = "danger";
                return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
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
                    //throw new Exception("ERROR: System could not generate report.");
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
