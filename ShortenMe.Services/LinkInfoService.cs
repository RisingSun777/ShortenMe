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
        private IDateAccessDA dateAccessDA;
        private IDateTimeProvider dateTimeProvider;

        public LinkInfoService(ILinkInfoDA linkInfoDA, IDateAccessDA dateAccessDA, IDateTimeProvider dateTimeProvider)
        {
            this.linkInfoDA = linkInfoDA;
            this.dateAccessDA = dateAccessDA;
            this.dateTimeProvider = dateTimeProvider;
        }

        public string GetFullLink(string shortenedLink)
        {
            LinkInfo link = linkInfoDA.GetUniqueByShortenedLink(shortenedLink);

            if (link != null)
            {
                DateAccess dateAccess = new DateAccess()
                {
                    ID = Guid.NewGuid(),
                    LinkInfoID = link.ID,
                    DateCreated = dateTimeProvider.Now()
                };

                dateAccessDA.Insert(dateAccess);

                return link.FullLink;
            }

            return null;
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
