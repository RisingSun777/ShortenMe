using ShortenMe.DataAccess.Contracts;
using ShortenMe.DataAccess.Contracts.Models;
using ShortenMe.Services.Contracts;
using ShortenMe.Services.Contracts.Models;
using System;
using System.Linq;

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

        public string GetFullLink(string shortenedLink, string userAgent)
        {
            LinkInfo link = linkInfoDA.GetUniqueByShortenedLink(shortenedLink);

            if (link != null)
            {
                DateAccess dateAccess = new DateAccess()
                {
                    ID = Guid.NewGuid(),
                    LinkInfoID = link.ID,
                    DateCreated = dateTimeProvider.Now(),
                    UserAgent = userAgent
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
            return Guid.NewGuid().ToString("N").Substring(0, 6);
        }

        private Tuple<DateTime, int>[] GetHitsInLast7Days(string shortenedLink)
        {
            DateAccess[] recordsInLast7Days = dateAccessDA.GetWithinTimestamp(shortenedLink, dateTimeProvider.Now().AddDays(-7));
            var groupedByDateRecords = recordsInLast7Days.GroupBy(a => a.DateCreated.Date);

            int length = groupedByDateRecords.Count();
            Tuple<DateTime, int>[] ret = new Tuple<DateTime, int>[length];

            for (int i = 0; i < length; ++i)
            {
                ret[i] = new Tuple<DateTime, int>(
                    groupedByDateRecords.ElementAt(i).Key, 
                    groupedByDateRecords.ElementAt(i).Count());
            }

            return ret;
        }

        public LinkAnalyticsModel GetAnalyticsModel(string shortenedLink)
        {
            LinkAnalyticsModel model = new LinkAnalyticsModel();

            model.TotalHits = dateAccessDA.GetTotalHits(shortenedLink);
            model.TotalHitsInLast7Days = GetHitsInLast7Days(shortenedLink);
            
            return model;
        }
    }
}
