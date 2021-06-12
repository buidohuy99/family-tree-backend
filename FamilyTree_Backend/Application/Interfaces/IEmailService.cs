using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyTreeBackend.Core.Application.Interfaces
{
    public interface IEmailService
    {
        public Task SendEmailAsync(string email, string subject, string body);

        public Task SendResetPasswordEmail(string email, string resetPasswordUrl);
    }
}
