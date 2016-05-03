using ShortenMe.DataAccess.Contracts;
using ShortenMe.DataAccess.Contracts.Models;
using ShortenMe.Services.Contracts;
using ShortenMe.Services.Contracts.Models;
using System;

namespace ShortenMe.Services
{
    public class LinkInfoService : ILinkInfoService
    {
        private ILinkInfoDA linkInfoDA;

        public LinkInfoService(ILinkInfoDA linkInfoDA)
        {
            this.linkInfoDA = linkInfoDA;
        }

        public string GetFullLink(string shortenedLink)
        {
            throw new NotImplementedException();
        }

        public string ProcessAndShortenLink(ProcessAndShortenLinkModel model)
        {
            LinkInfo link = linkInfoDA.GetUniqueByFullLink(model.FullLink);

            if (link == null)
            {
                string shortenedLink = GetShortenedLink(model.FullLink);

                link = new LinkInfo
                {
                    ID = Guid.NewGuid(),
                    FullLink = model.FullLink,
                    ShortenedLink = shortenedLink
                };

                linkInfoDA.Insert(link);
            };

            return link.ShortenedLink;
        }

        private string GetShortenedLink(string url)
        {
            return Guid.NewGuid().ToString("N");
        }
    }
}
