using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace WebApi.Server.Models
{
    public class ApiResultModel
    {
        public HttpStatusCode Status { get; set; }
        public string Data { get; set; }
        public string ErrorMessage { get; set; }
    }
}