using Project.ApiControllers.Contracts.V1.Requests;
using Project.ApiControllers.Contracts.V1.Responses;
using SecurityGuard.Interfaces;
using SecurityGuard.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Security;
using System.Web.Routing;
using Project.DAL;
using Roles = System.Web.Security.Roles;
using System.Web;
using System.IO;
using System.Configuration;
using Newtonsoft.Json;

namespace Project.ApiControllers
{
    public class MobileController : ApiController
    {

        private IAuthenticationService _authenticationService;
        private IMembershipService _membershipService;
        private PROEntities db = new PROEntities();


        public MobileController()
        {
            _membershipService = new MembershipService(Membership.Provider);
            _authenticationService = new AuthenticationService(_membershipService, new FormsAuthenticationService());
}
        [AllowAnonymous]
        [HttpPost]
        //[Route("api/student/names")]
        public LoginResponse Login(LoginRequest request) {
            var resp = new LoginResponse();
            try {
                if (request == null)
                {
                    resp.Message = "Missing Request Parameter"; return resp;
                }
                if (string.IsNullOrEmpty(request.Username))
                {
                    resp.Message = "Username Request Parameter is required"; return resp;
                }
                if (string.IsNullOrEmpty(request.Password))
                {
                    resp.Message = "Password Request Parameter is required"; return resp;
                }

                var result = _authenticationService.LogOn(request.Username, request.Password, false);
                resp.IsSuccessful = result;
                if (result)
                {
                    //var user = _membershipService.GetUser(request.Username);
                    var user = db.Users.FirstOrDefault(x => x.UserName.ToLower() == request.Username.ToLower());
                    resp.Username = request.Username;
                    resp.UserId = user.UserId;
                    resp.Role = Roles.GetRolesForUser(request.Username).ToList();
                    resp.Message = "Login Successful";
                }
                else { resp.Message = "Login Failed"; }
            } catch (Exception ex)
            {
                //TODO:Log Error
                resp.IsSuccessful = false;
                resp.Message = "Server Error";
            }
           
            return resp;
        }

        //Get All the Codelist
        [HttpGet]
        public ContractorsResponse GetContractors() {
            var resp = new ContractorsResponse();
            try {
                var rows = db.Contractor.Where(x => x.IsDeleted == false).Select(x=>new CodeList { Id=x.Id,Name=x.Name}).ToList();
                if (rows != null)
                {
                    resp.Records = rows;
                    resp.Message = "Success";
                    resp.IsSuccessful = true;
                }
            } catch (Exception ex)
            {
                //TODO:Log Error
                resp.IsSuccessful = false;
                resp.Message = "Server Error";
            }
            return resp;

        }

        [HttpGet]
        public LGAsResponse GetLGAs()
        {
            var resp = new LGAsResponse();
            try
            {
                var rows = db.LGA.Where(x => x.IsDeleted == false).Select(x => new CodeList { Id = x.Id, Name =x.State.Name+" - "+ x.Name }).ToList();
                if (rows != null)
                {
                    resp.Records = rows;
                    resp.Message = "Success";
                    resp.IsSuccessful = true;
                }
            }
            catch (Exception ex)
            {
                //TODO:Log Error
                resp.IsSuccessful = false;
                resp.Message = "Server Error";
            }
            return resp;

        }
        [HttpGet]
        public ProjectsResponse GetProjects()
        {
            var resp = new ProjectsResponse();
            try
            {
                var rows = db.ProjectApplication.Where(x => x.IsDeleted == false)
                    .Select(x => new ProjectRecord
                    {
                        ProjectId = x.Id,
                        SerialNo = x.SerialNo,
                        Workflow = x.Workflow.Name,
                        ProjectType = x.ProjectTypeId,
                        Contractor = x.Contractor.Name,
                        Description = x.Description,
                        OwnedBy = x.InspectionUserId
                    }).ToList();
                if (rows != null)
                {
                    resp.Records = rows;
                    resp.Message = "Success";
                    resp.IsSuccessful = true;
                }
                else
                {
                    resp.Records = new List<ProjectRecord>();
                    resp.Message = "No Recores Found";
                    resp.IsSuccessful = true;
                }
            }
            catch (Exception ex)
            {
                //TODO:Log Error
                resp.IsSuccessful = false;
                resp.Message = "Server Error";
            }
            return resp;

        }
        [HttpGet]
        public MilestoneResponse GetMilestone()
        {
            var resp = new MilestoneResponse();
            try
            {
                var rows = db.StageOfCompletion.Where(x => x.IsDeleted == false)
                    .Select(x => new Milestone
                    {
                        ProjectType = x.ProjectTypeId,
                        Percentage = x.Percentage,
                        Description = x.Description
                    }).ToList();
                if (rows != null)
                {
                    resp.Records = rows;
                    resp.Message = "Success";
                    resp.IsSuccessful = true;
                }
                else
                {
                    resp.Records = new List<Milestone>();
                    resp.Message = "No Recores Found";
                    resp.IsSuccessful = true;
                }
            }
            catch (Exception ex)
            {
                //TODO:Log Error
                resp.IsSuccessful = false;
                resp.Message = "Server Error";
            }
            return resp;

        }


        [HttpPost]
        public ProjectResponse ReportProjectStatus(ReportRequest request) {
            var resp = new ProjectResponse();
            try {
                //checked for redundancy
                var guidTId = request.TransactionId;//Guid.Parse(request.TransactionId);
                var existRow = db.Inspection.FirstOrDefault(x => x.TransactionId == guidTId);
                if (existRow != null) {
                    resp.IsSuccessful = true;
                    resp.Message = "Records Has Been Added Already";
                    return resp;
                }
                var entity = new Inspection();
                entity.TransactionId = guidTId;
                entity.ProjectId = request.ProjectId;
                entity.Location = request.Location;
                entity.Coordinate = request.Coordinate;
                entity.LgaId = request.LGAId;
                entity.StageOfCompletion = request.StageOfCompletion;
                entity.DescriptionOfCompletion = request.DescriptionOfCompletion;
                entity.ProjectQuality = request.ProjectQuality;
                entity.HasDefect = request.HasDefect;
                entity.DescriptionOfDefect = request.DescriptionOfDefect;
                entity.InspectionStatus = request.Status;
                entity.InspectionDate = request.InspectionDate;
                entity.ModifiedBy = request.Modifiedby;
                entity.ModifiedDate = DateTime.Now;

                db.Inspection.AddObject(entity);
                db.SaveChanges();
                resp.IsSuccessful = true;
                resp.Message = "Record Added Successful";
            }
            catch (Exception ex) {
                //TODO:Log Error
                resp.IsSuccessful = false;
                resp.Message = "Server Error";
            }
            return resp;
        }

        [HttpPost]
        public ProjectResponse ReportProjectStatusMultipart()
        {
            var resp = new ProjectResponse();
            var now = DateTime.Now;
            try
            {
                var appSettings = ConfigurationManager.AppSettings;
                var files = SaveFiles;
                if (files == null) {
                    resp.IsSuccessful = false;
                    resp.Message = "Please Upload Images";
                    return resp;
                }
               
                var request = HttpContext.Current.Request.Params;
                //checked for redundancy
                var guidTId = Guid.Parse(request["TransactionId"]);//Guid.Parse(request.TransactionId);
                var existRow = db.Inspection.FirstOrDefault(x => x.TransactionId == guidTId);
                if (existRow != null)
                {
                    resp.IsSuccessful = true;
                    resp.Message = "Records Has Been Added Already";
                    return resp;
                }
                var entity = new Inspection();
                entity.TransactionId = guidTId;
                entity.ProjectId = int.Parse(request["ProjectId"]);
                entity.Location = request["Location"];
                entity.Coordinate = request["Coordinate"];
                entity.LgaId = int.Parse(request["LGAId"]);
                entity.StageOfCompletionId = int.Parse(request["StageOfCompletionId"]);
                entity.StageOfCompletion = request["StageOfCompletion"];
                entity.DescriptionOfCompletion = request["DescriptionOfCompletion"];
                entity.ProjectQuality = request["ProjectQuality"];
                entity.HasDefect = request["HasDefect"]=="Yes"?true:false;
                entity.DescriptionOfDefect = request["DescriptionOfDefect"];
                entity.InspectionStatus = request["Status"];
                entity.InspectionDate = DateTime.Parse(request["InspectionDate"]);
                entity.ModifiedBy = request["Modifiedby"];
                entity.ModifiedDate = now;
                foreach (var file in files)
                {
                    var f = file.Key.Split('.');
                    var id = appSettings[f[0]];
                    var doc = new DocumentInfo
                    {
                        DocumentTypeId = int.Parse(id),
                        Name = f[0],
                        Path = file.Value,
                        IssuedDate = now,
                        ModifiedBy = request["Modifiedby"],
                        ModifiedDate = now
                    };
                    entity.DocumentInfo.Add(doc);
                }

                db.Inspection.AddObject(entity);
                db.SaveChanges();
                resp.IsSuccessful = true;
                resp.Message = "Record Added Successful";
            }
            catch (Exception ex)
            {
                //TODO:Log Error
                resp.IsSuccessful = false;
                resp.Message = "Server Error:"+ex.Message;
            }
            return resp;
        }

        private List<KeyValuePair<string, string>> SaveFiles
        {
            get
            {
                List<KeyValuePair<string, string>> resp = new List<KeyValuePair<string, string>>();
                var count = HttpContext.Current.Request.Files.Count;
                if (count == 0)
                    return null;

                var files = HttpContext.Current.Request.Files;
                
                for (var i=0; i<files.Count;i++)
                {
                    var file = files[i];
                    //var field = files[i];
                    if (file != null && file.ContentLength > 0)
                    {
                        var fArr = file.FileName.Split('.');
                        var fileName = Guid.NewGuid().ToString()+"."+ fArr[1];//Path.GetFileName(file.FileName);

                        var defaultPath = Properties.Settings.Default.FullPhotoPath;
                       //var path = Path.Combine("c:\\uploads\\");
                        var dir = Path.GetDirectoryName(defaultPath);

                        if (!Directory.Exists(dir))
                            Directory.CreateDirectory(dir);
                        var filepath = Path.Combine(dir, fileName);

                        file.SaveAs(filepath);
                       // var fileNameArr = fileName.Split('_');
                        resp.Add(new KeyValuePair<string, string>(file.FileName, fileName));
                    }
                }
                return resp;
            }
        }

        [HttpGet]
        public ReportRequest Report() {
            return db.Inspection.Select(x => new ReportRequest {
                Id = x.Id,
                ProjectId = x.ProjectId,
                TransactionId = x.TransactionId,
                Location = x.Location,
                Coordinate = x.Coordinate,
                LGAId = x.LgaId,
                StageOfCompletion = x.StageOfCompletion,
                DescriptionOfCompletion = x.DescriptionOfCompletion,
                HasDefect = x.HasDefect,
                DescriptionOfDefect = x.DescriptionOfDefect,
                Modifiedby = x.ModifiedBy,
                ProjectQuality = x.ProjectQuality,
                Status = x.InspectionStatus,
                InspectionDate = x.InspectionDate
            }).FirstOrDefault();
        }


        ////Supplies Verification 

        [HttpPost]
        public SupplyResponse ReportSupplyStatusMultipart()
        {
            var resp = new SupplyResponse();
            var now = DateTime.Now;
            try
            {
                var appSettings = ConfigurationManager.AppSettings;
                var files = SaveFiles;
                if (files == null)
                {
                    resp.IsSuccessful = false;
                    resp.Message = "Please Upload Images";
                    return resp;
                }

                var request = HttpContext.Current.Request.Params;
                List<Contracts.V1.Responses.SupplyItem> items = JsonConvert.DeserializeObject<List<Contracts.V1.Responses.SupplyItem>>(request["itemList"]);

                //checked for redundancy
                var guidTId = Guid.Parse(request["TransactionId"]);//Guid.Parse(request.TransactionId);
                var existRow = db.Inspection.FirstOrDefault(x => x.TransactionId == guidTId);
                if (existRow != null)
                {
                    resp.IsSuccessful = true;
                    resp.Message = "Records Has Been Added Already";
                    return resp;
                }
                var entity = new Supplies();
                entity.TransactionId = guidTId;
                entity.Representative = request["Representative"];
                entity.RepresentativePhoneNumber = request["RepresentativePhone"];
                entity.RepresentativeDesignation = request["RepresentativeDesg"];
                entity.Coordinate = request["Coordinate"];
                entity.LGAId = int.Parse(request["LGAId"]);
                entity.VerificationOfficer = request["VerificationOfficer"];
                entity.VerificationDate = DateTime.Parse(request["VerificationDate"]);
                entity.ContractorId = int.Parse(request["Contractor"]);
                entity.Location = request["Location"];
                entity.ModifiedBy = request["Modifiedby"];
                entity.Status = request["Status"];
                entity.Modified = now;
                entity.WorkflowId = 3;

                //List<Milestone> items = request["Items"];


                foreach (var file in files)
                {
                    var f = file.Key.Split('.');
                    var id = appSettings[f[0]];
                    var doc = new DocumentInfo
                    {
                        DocumentTypeId = int.Parse(id),
                        Name = f[0],
                        Path = file.Value,
                        IssuedDate = now,
                        ModifiedBy = request["Modifiedby"],
                        ModifiedDate = now
                    };
                    entity.DocumentInfo.Add(doc);
                }
                foreach (var item in items) {
                    var row = new DAL.SupplyItems
                    {
                        Description = item.description,
                        ModifiedBy = item.modifiedBy,
                        QuantityDelivered = int.Parse(item.quantityDelivered),
                        QuantityOrdered = int.Parse(item.quantityOrdered),
                        Remarks = item.remarks,
                        SerialNumber = item.serialNo,
                        Modified = now
                    };
                    entity.SupplyItems.Add(row);
                }

                db.Supplies.AddObject(entity);
               db.SaveChanges();
                resp.IsSuccessful = true;
                resp.Message = "Record Added Successful";
            }
            catch (Exception ex)
            {
                //TODO:Log Error
                resp.IsSuccessful = false;
                resp.Message = "Server Error:" + ex.Message;
            }
            return resp;
        }


    }
}