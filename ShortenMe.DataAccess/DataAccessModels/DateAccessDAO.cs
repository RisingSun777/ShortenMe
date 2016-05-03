using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShortenMe.DataAccess.DataAccessModels
{
    public class DateAccessDAO : BaseDAO
    {
        public virtual Guid ID { get; set; }
        public virtual Guid LinkInfoID { get; set; }
        public virtual DateTime DateCreated { get; set; }

        public override void Validate()
        {
            if (ID == Guid.Empty)
                AddValidationMessage("ID must not be empty.");

            if (LinkInfoID == Guid.Empty)
                AddValidationMessage("LinkInfoID must not be empty.");

            if (DateCreated == DateTime.MinValue)
                AddValidationMessage("DateCreated must have a valid value.");

            string messages = GetValidationMessages();
            if (!string.IsNullOrEmpty(messages))
                throw new DataAccessValidationException(messages);
        }
    }
}
