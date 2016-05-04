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
            const int reportingDateRange = 7;
            DateTime reportTime = dateTimeProvider.Now().Date.AddDays(-reportingDateRange + 1);

            DateAccess[] recordsInLast7Days = dateAccessDA.GetWithinTimestamp(shortenedLink, reportTime);
            var groupedByDateRecords = recordsInLast7Days.GroupBy(a => a.DateCreated.Date);

            Tuple<DateTime, int>[] ret = new Tuple<DateTime, int>[reportingDateRange];

            for (int i = 0; i < reportingDateRange; ++i)
            {
                DateTime time = reportTime.AddDays(i);
                var searchedDateRecords = groupedByDateRecords.SingleOrDefault(a => a.Key == time);

                ret[i] = new Tuple<DateTime, int>(
                    time,
                    searchedDateRecords == null ? 0 : searchedDateRecords.Count()
                );
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
