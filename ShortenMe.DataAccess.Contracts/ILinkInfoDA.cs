using ShortenMe.DataAccess.Contracts.Models;
using System;

namespace ShortenMe.DataAccess.Contracts
{
    public interface ILinkInfoDA
    {
        LinkInfo[] GetAll();
        LinkInfo GetUniqueByFullLink(string fullLink);
        void Insert(LinkInfo linkInfo);
        void Delete(LinkInfo linkInfo);
    }
}
