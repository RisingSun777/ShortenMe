using ShortenMe.DataAccess.Contracts.Models;
using System;

namespace ShortenMe.DataAccess.Contracts
{
    public interface IDateAccessDA
    {
        void Insert(DateAccess input);
        void Delete(DateAccess input);
        DateAccess GetByID(Guid id);
        DateAccess[] GetByLinkInfoID(Guid linkInfoID);

        int GetTotalHits(string shortenedLink);
        DateAccess[] GetWithinTimestamp(string shortenedLink, DateTime timestamp);
    }
}
