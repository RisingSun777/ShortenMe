using ShortenMe.DataAccess.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShortenMe.DataAccess.Contracts.Models;
using NHibernate;
using ShortenMe.DataAccess.DataAccessModels;

namespace ShortenMe.DataAccess
{
    public class LinkInfoDA : BaseDA, ILinkInfoDA
    {
        public LinkInfo[] GetAll()
        {
            IList<LinkInfoDAO> ret = null;

            using (var session = OpenSession())
            {
                ret = session
                    .QueryOver<LinkInfoDAO>()
                    .List();
            }

            return ret
                .Select(a => new LinkInfo
                {
                    ID = a.ID,
                    FullLink = a.FullLink,
                    ShortenedLink = a.ShortenedLink
                }).ToArray();
        }

        public void Insert(LinkInfo linkInfo)
        {
            LinkInfoDAO linkInfoDAO = new LinkInfoDAO()
            {
                ID = linkInfo.ID,
                FullLink = linkInfo.FullLink,
                ShortenedLink = linkInfo.ShortenedLink
            };

            linkInfoDAO.Validate();

            using (var session = OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    session.Save(linkInfoDAO);
                    transaction.Commit();
                }
        }

        public LinkInfo GetUniqueByFullLink(string fullLink)
        {
            LinkInfo ret = null;
            LinkInfoDAO dao = null;

            using (var session = OpenSession())
            {
                dao = session.QueryOver<LinkInfoDAO>()
                    .Where(a => a.FullLink == fullLink)
                    .SingleOrDefault();
            }

            if (dao != null)
                ret = new LinkInfo
                {
                    ID = dao.ID,
                    FullLink = dao.FullLink,
                    ShortenedLink = dao.ShortenedLink
                };

            return ret;
        }

        public void Delete(LinkInfo linkInfo)
        {
            LinkInfoDAO dao = new LinkInfoDAO
            {
                ID = linkInfo.ID,
                FullLink = linkInfo.FullLink,
                ShortenedLink = linkInfo.ShortenedLink
            };

            using (var session = OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                session.Delete(dao);
                transaction.Commit();
            }
        }

        public LinkInfo GetUniqueByShortenedLink(string shortenedLink)
        {
            LinkInfo ret = null;
            LinkInfoDAO dao = null;

            using (var session = OpenSession())
            {
                dao = session.QueryOver<LinkInfoDAO>()
                    .Where(a => a.ShortenedLink == shortenedLink)
                    .SingleOrDefault();
            }

            if (dao != null)
                ret = new LinkInfo
                {
                    ID = dao.ID,
                    FullLink = dao.FullLink,
                    ShortenedLink = dao.ShortenedLink
                };

            return ret;
        }
    }
}
