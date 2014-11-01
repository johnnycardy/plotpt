using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Plotter
{
    public class WakeUpController : ApiController
    {
        // GET api/<controller>
        public string Get()
        {
            return "OK";
        }
    }
}