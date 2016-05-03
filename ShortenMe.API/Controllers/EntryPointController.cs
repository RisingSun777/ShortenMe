using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ShortenMe.API.Controllers
{
    public class EntryPointController : ApiController
    {
        [HttpGet]
        public IHttpActionResult Index()
        {
            return Ok("First success");
        }
    }
}
