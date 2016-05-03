using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShortenMe.Services.Contracts
{
    public interface IDateTimeProvider
    {
        DateTime Now();
    }
}
