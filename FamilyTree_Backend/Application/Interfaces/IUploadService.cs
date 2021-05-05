using FamilyTreeBackend.Core.Application.Models.FileUpload;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyTreeBackend.Core.Application.Interfaces
{
    public interface IUploadService
    {
        public Task<string> UploadImage(UploadSingleFileModel input);
    }
}
