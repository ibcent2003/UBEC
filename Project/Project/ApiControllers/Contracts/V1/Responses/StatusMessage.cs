using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.ApiControllers.Contracts.V1.Responses
{
    public class StatusMessage
    {
        public bool IsSuccessful { get; set; } = false;
        public string Message { get; set; } = string.Empty;
    }
}