using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShortenMe.DataAccess.Contracts.Models
{
    public class DateAccess
    {
        public virtual Guid ID { get; set; }
        public virtual Guid LinkInfoID { get; set; }
        public virtual DateTime DateCreated { get; set; }
        public virtual string UserAgent { get; set; }
    }
}
