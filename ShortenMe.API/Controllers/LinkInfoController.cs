using System;
using System.Web.Http;
using ShortenMe.API.Models;
using ShortenMe.Services.Contracts;
using ShortenMe.Services.Contracts.Models;
using System.Web.Http.Cors;

namespace ShortenMe.API.Controllers
{
    //[EnableCors("*", "*", "*")]
    public class LinkInfoController : ApiController
    {
        private ILinkInfoService linkInfoService;

        public LinkInfoController(ILinkInfoService linkInfoService)
        {
            this.linkInfoService = linkInfoService;
        }

        [HttpPost]
        //[EnableCors("*", "*", "*")]
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
        //[EnableCors("*", "*", "*")]
        public IHttpActionResult Get(string shortenedLink)
        {
            if (!string.IsNullOrEmpty(shortenedLink))
            {
                string fullLink = linkInfoService.GetFullLink(shortenedLink);

                if (!string.IsNullOrEmpty(fullLink))
                    return Ok(fullLink);
            }

            return NotFound();
        }
    }
}
