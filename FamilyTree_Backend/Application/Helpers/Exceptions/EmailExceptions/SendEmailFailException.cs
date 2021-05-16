using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyTreeBackend.Core.Application.Helpers.Exceptions.EmailExceptions
{
    [Serializable]
    public class SendEmailFailException : BaseServiceException
    {
        public string FailedEmail { get; }

        public SendEmailFailException(string message, string failedEmail = "") : base(message)
        {
            FailedEmail = failedEmail;
        } 
    }
}
