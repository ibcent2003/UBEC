using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.ApiControllers.Contracts.V1.Requests
{
    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }

    }
}