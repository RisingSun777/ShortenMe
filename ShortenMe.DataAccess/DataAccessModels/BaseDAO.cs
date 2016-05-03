using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShortenMe.DataAccess.DataAccessModels
{
    public abstract class BaseDAO
    {
        private StringBuilder dataAccessValidationMessageBuilder;

        public abstract void Validate();

        protected void AddValidationMessage(string message)
        {
            if (dataAccessValidationMessageBuilder == null)
                dataAccessValidationMessageBuilder = new StringBuilder();

            dataAccessValidationMessageBuilder.AppendLine(message);
        }

        protected string GetValidationMessages()
        {
            if (dataAccessValidationMessageBuilder == null)
                return null;

            return dataAccessValidationMessageBuilder.ToString();
        }
    }

    public class DataAccessValidationException : Exception
    {
        public DataAccessValidationException(string msg) : base(msg) { }
    }
}
