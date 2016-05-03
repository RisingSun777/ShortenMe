using System;
using System.Web.Http;
using ShortenMe.API.Models;
using ShortenMe.Services.Contracts;
using ShortenMe.Services.Contracts.Models;
using System.Web.Http.Cors;

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
            if (model != null && Uri.IsWellFormedUriString(model.FullLink, UriKind.Absolute))
            {
                string shortenedLink = linkInfoService.ProcessAndShortenLink(new ProcessAndShortenLinkModel { FullLink = model.FullLink });

                return Ok(shortenedLink);
            }

            return BadRequest("Model is invalid.");
        }

        [HttpGet]
        public IHttpActionResult Get(string shortenedLink)
        {
            if (!string.IsNullOrEmpty(shortenedLink))
            {
                string userAgent = null;

                if (Request != null && Request.Headers != null)
                {
                    var headers = Request.Headers.GetValues("User-Agent");
                    userAgent = string.Join(" ", headers);
                }

                string fullLink = linkInfoService.GetFullLink(shortenedLink, userAgent);

                if (!string.IsNullOrEmpty(fullLink))
                    return Ok(fullLink);
            }

            return NotFound();
        }
    }
}
