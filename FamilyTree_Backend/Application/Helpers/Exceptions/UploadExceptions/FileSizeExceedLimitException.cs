using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyTreeBackend.Core.Application.Helpers.Exceptions.UploadExceptions
{
    public class FileSizeExceedLimitException : UploadException
    {
        public long FileSizeInBytes { get; private set; }
        public long LimitInBytes { get; private set; }
        public FileSizeExceedLimitException(string message, string fileName, long fileSizeInBytes, long limitInBytes) : base(message, fileName)
        {
            FileSizeInBytes = fileSizeInBytes;
            LimitInBytes = limitInBytes;
        }
    }
}
