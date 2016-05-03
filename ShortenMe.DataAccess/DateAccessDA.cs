using ShortenMe.DataAccess.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShortenMe.DataAccess.Contracts.Models;
using NHibernate;
using ShortenMe.DataAccess.DataAccessModels;
using NHibernate.Criterion;

namespace ShortenMe.DataAccess
{
    public class DateAccessDA : BaseDA, IDateAccessDA
    {
        public void Delete(DateAccess input)
        {
            DateAccessDAO dao = new DateAccessDAO
            {
                ID = input.ID
            };

            using (var session = OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                session.Delete(dao);
                transaction.Commit();
            }
        }

        public DateAccess GetByID(Guid id)
        {
            DateAccess ret = null;
            DateAccessDAO dao = null;

            using (var session = OpenSession())
            {
                dao = session.QueryOver<DateAccessDAO>()
                    .Where(a => a.ID == id)
                    .SingleOrDefault();
            }

            if (dao != null)
                ret = new DateAccess
                {
                    ID = dao.ID,
                    LinkInfoID = dao.LinkInfoID,
                    DateCreated = dao.DateCreated
                };

            return ret;
        }

        public DateAccess[] GetByLinkInfoID(Guid linkInfoID)
        {
            DateAccess[] ret = null;
            IList<DateAccessDAO> daoList = null;

            using (var session = OpenSession())
            {
                daoList = session.QueryOver<DateAccessDAO>()
                    .Where(a => a.LinkInfoID == linkInfoID)
                    .List();
            }

            if (daoList != null)
                ret = daoList.Select(a => new DateAccess
                {
                    ID = a.ID,
                    LinkInfoID = a.LinkInfoID,
                    DateCreated = a.DateCreated
                }).ToArray();

            return ret;
        }

        public DateAccess[] GetWithinTimestamp(string shortenedLink, DateTime timestamp)
        {
            IList<DateAccessDAO> dao = null;

            using (var session = OpenSession())
            {
                Guid linkInfoID = session.QueryOver<LinkInfoDAO>()
                    .Where(a => a.ShortenedLink == shortenedLink)
                    .Select(a => a.ID)
                    .SingleOrDefault<Guid>();

                dao = session.QueryOver<DateAccessDAO>()
                    .Where(a => a.LinkInfoID == linkInfoID && a.DateCreated >= timestamp)
                    .List();
            };

            return dao.Select(a => new DateAccess
            {
                ID = a.ID,
                DateCreated = a.DateCreated,
                LinkInfoID = a.LinkInfoID,
                UserAgent = a.UserAgent
            }).ToArray();
        }

        public int GetTotalHits(string shortenedLink)
        {
            int ret = 0;

            using (var session = OpenSession())
            {
                Guid linkInfoID = session.QueryOver<LinkInfoDAO>()
                    .Where(a => a.ShortenedLink == shortenedLink)
                    .Select(a => a.ID)
                    .SingleOrDefault<Guid>();

                ret = session.QueryOver<DateAccessDAO>()
                    .Where(a => a.LinkInfoID == linkInfoID)
                    .RowCount();
            };

            return ret;
        }

        public void Insert(DateAccess input)
        {
            DateAccessDAO dao = new DateAccessDAO()
            {
                ID = input.ID,
                LinkInfoID = input.LinkInfoID,
                DateCreated = input.DateCreated
            };

            dao.Validate();

            using (var session = OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                session.Save(dao);
                transaction.Commit();
            }
        }
    }
}
