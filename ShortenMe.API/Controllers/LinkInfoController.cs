using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ShortenMe.API.Models;
using ShortenMe.Services.Contracts;
using ShortenMe.Services.Contracts.Models;

namespace ShortenMe.API.Controllers
{
    public class LinkInfoController : ApiController
    {
        private ILinkInfoService linkInfoService;

        public LinkInfoController(ILinkInfoService linkInfoService)
        {
            this.linkInfoService = linkInfoService;
        }

        [HttpPost]
        public IHttpActionResult Post(LinkInfoPostModel model)
        {
            string shortenedLink = linkInfoService.ProcessAndShortenLink(new ProcessAndShortenLinkModel { FullLink = model.FullLink });
            
            return Ok(shortenedLink);
        }

        [HttpGet]
        public IHttpActionResult Get(string shortenedLink)
        {
            string fullLink = linkInfoService.GetFullLink(shortenedLink);

            if (!string.IsNullOrEmpty(fullLink))
                return Ok(fullLink);

            return NotFound();
        }
    }
}
