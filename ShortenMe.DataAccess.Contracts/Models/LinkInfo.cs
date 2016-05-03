using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShortenMe.DataAccess.Contracts.Models
{
    public class LinkInfo
    {
        public virtual Guid ID { get; set; }
        public virtual string FullLink { get; set; }
        public virtual string ShortenedLink { get; set; }
    }
}
