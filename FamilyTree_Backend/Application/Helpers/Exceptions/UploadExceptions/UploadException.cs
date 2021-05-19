using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyTreeBackend.Core.Application.Helpers.Exceptions.UploadExceptions
{
    public class UploadException : BaseServiceException
    {
        public string FileName { get; private set; }
        public UploadException(string message, string fileName) : base(message)
        {
            FileName = fileName;
        }
    }
}
