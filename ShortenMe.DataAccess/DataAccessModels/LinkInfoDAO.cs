using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShortenMe.DataAccess.DataAccessModels
{
    public class LinkInfoDAO : BaseDAO
    {
        public virtual Guid ID { get; set; }
        public virtual string FullLink { get; set; }
        public virtual string ShortenedLink { get; set; }
        
        public override void Validate()
        {
            if (ID == Guid.Empty)
                AddValidationMessage("ID must not be empty.");

            if (!Uri.IsWellFormedUriString(FullLink, UriKind.Absolute))
                AddValidationMessage("Full link does not contain correct URI format.");

            if (string.IsNullOrEmpty(ShortenedLink))
                AddValidationMessage("Shortened link must contain value.");

            string messages = GetValidationMessages();
            if (!string.IsNullOrEmpty(messages))
                throw new DataAccessValidationException(messages);
        }
    }
}
