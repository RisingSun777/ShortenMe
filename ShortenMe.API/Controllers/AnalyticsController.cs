using System;
using System.Web.Http;
using ShortenMe.API.Models;
using ShortenMe.Services.Contracts;
using ShortenMe.Services.Contracts.Models;
using System.Web.Http.Cors;

namespace ShortenMe.API.Controllers
{
    public class AnalyticsController : ApiController
    {
        private ILinkInfoService linkInfoService;

        public AnalyticsController(ILinkInfoService linkInfoService)
        {
            this.linkInfoService = linkInfoService;
        }

        [HttpGet]
        public IHttpActionResult Get(string shortenedLink)
        {
            try
            {
                var serviceModel = linkInfoService.GetAnalyticsModel(shortenedLink);

                Models.LinkAnalyticsModel ret = new Models.LinkAnalyticsModel
                {
                    TotalHits = serviceModel.TotalHits,
                    TotalHitsByBrowsers = serviceModel.TotalHitsByBrowsers,
                    TotalHitsInLast7Days = serviceModel.TotalHitsInLast7Days
                };

                return Ok(ret);
            }
            catch { }

            return BadRequest();
        }
    }
}
