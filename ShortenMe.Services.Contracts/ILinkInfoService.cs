using ShortenMe.Services.Contracts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShortenMe.Services.Contracts
{
    public interface ILinkInfoService
    {
        string ProcessAndShortenLink(ProcessAndShortenLinkModel model);
        string GetFullLink(string shortenedLink, string userAgent);
        LinkAnalyticsModel GetAnalyticsModel(string shortenedLink);
    }
}
