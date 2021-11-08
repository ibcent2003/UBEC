using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.ApiControllers.Contracts.V1.Responses
{
    public class LoginResponse: StatusMessage
    {
        public List<string> Role { get; set; } = new List<string>();
        public string Username { get; set; } = string.Empty;
        public Guid? UserId { get; set; }
    }
}