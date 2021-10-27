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
                    //var user = _membershipService.GetUser(request.username);
                    resp.Username = request.Username;
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
                var rows = db.Workflow.Where(x => x.IsDeleted == false).Select(x => new CodeList { Id = x.Id, Name = x.Name }).ToList();
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

        [HttpPost]
        public ProjectResponse ReportProjectStatus(ProjectRequest request) {
            var resp = new ProjectResponse();
            try {
                //checked for redundancy
                var guidTId = request.TransactionId;//Guid.Parse(request.TransactionId);
                var existRow = db.ProjectApplication.FirstOrDefault(x => x.TransactionId == guidTId);
                if (existRow != null) {
                    resp.IsSuccessful = true;
                    resp.Message = "Records Has Been Added Already";
                    return resp;
                }
                var entity = new ProjectApplication();
                entity.TransactionId = guidTId;
                entity.WorkFlowId = request.workflowId;
                entity.SerialNo = request.SerialNo;
                entity.ContractorId = request.ContractorId;
                entity.Description = request.Description;
                entity.Location = request.Location;
                entity.Coordinate = request.Coordinate;
                entity.LGAId = request.LGAId;
                entity.StageOfCompletion = request.StageOfCompletion;
                entity.DescriptionOfCompletion = request.DescriptionOfCompletion;
                entity.ProjectQuality = request.ProjectQuality;
                entity.HasDefect = request.HasDefect;
                entity.DescriptionOfDefect = request.DescriptionOfDefect;
                entity.ContractSum = 0m;
                entity.Status = request.Status;
                entity.ModifiedBy = request.Modifiedby;
                entity.ModifiedDate = DateTime.Now;

                db.ProjectApplication.AddObject(entity);
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

        [HttpGet]
        public ProjectRequest Project() {
            return db.ProjectApplication.Select(x => new ProjectRequest {
                Id = x.Id,
                ContractorId = x.ContractorId,
                TransactionId = x.TransactionId,
                workflowId = x.WorkFlowId,
                SerialNo = x.SerialNo,
                Description = x.Description,
                Location = x.Location,
                Coordinate = x.Coordinate,
                LGAId = x.LGAId,
                StageOfCompletion = x.StageOfCompletion,
                DescriptionOfCompletion = x.DescriptionOfCompletion,
                HasDefect = x.HasDefect,
                DescriptionOfDefect = x.DescriptionOfDefect,
                ContractSum = x.ContractSum,
                Modifiedby = x.ModifiedBy,
                ProjectQuality = x.ProjectQuality,
                Status = x.Status
            }).FirstOrDefault();
        }

        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}