using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.ApiControllers.Contracts.V1.Responses
{
    public class ContractorsResponse: StatusMessage
    {
        
        public List<CodeList> Records { get; set; }
    }
}