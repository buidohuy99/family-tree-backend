using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FamilyTreeBackend.Infrastructure.Service.InternalServices.EmailTemplates
{
    public static class EmailTemplatesManager
    {
        public const string ResetPassword = "Reset Password";
        public const string ConfirmEmail = "Confirm Email";

        private static readonly Dictionary<string, string> EmailPaths = new Dictionary<string, string>()
        {
            { ResetPassword, @"ResetPassword.html" },
            { ConfirmEmail, @"ConfirmEmail.html" }
        };
        
        public static string GetEmailCotent(string templateName, params object?[] args)
        {
            if (!EmailPaths.ContainsKey(templateName))
            {
                return null;
            }
            var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\EmailTemplates\" + EmailPaths[templateName];
            try
            {
                using (StreamReader SourceReader = File.OpenText(path))
                {

                    string body = @"{0}";
                    body = string.Format(body, SourceReader.ReadToEnd());
                    return string.Format(body, args);
                }
            }
            catch(Exception e)
            {
                return null;
            }
            
        }
    }
}
