using ShortenMe.DataAccess.Contracts.Models;
using System;

namespace ShortenMe.DataAccess.Contracts
{
    public interface ILinkInfoDA
    {
        LinkInfo[] GetAll();
        LinkInfo GetUniqueByFullLink(string fullLink);
        LinkInfo GetUniqueByShortenedLink(string shortenedLink);
        void Insert(LinkInfo linkInfo);
        void Delete(LinkInfo linkInfo);
    }
}
