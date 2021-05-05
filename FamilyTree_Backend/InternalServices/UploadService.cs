using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using FamilyTreeBackend.Core.Application.Helpers.ConfigModels;
using FamilyTreeBackend.Core.Application.Interfaces;
using FamilyTreeBackend.Core.Application.Models.FileUpload;
using FamilyTreeBackend.Core.Domain.Constants;
using FamilyTreeBackend.Core.Domain.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyTreeBackend.Infrastructure.Service.InternalServices
{
    public class UploadService : IUploadService
    {
        private List<Cloudinary> cloudinarySources;
        private ILogger<UploadService> _logger;

        public UploadService(IOptions<CloudinaryAccounts> cloudinary, ILogger<UploadService> logger)
        {
            cloudinarySources = new List<Cloudinary>();
            foreach (var account in cloudinary.Value.Accounts) {
                // Cloudinary for image upload
                Account cloudinaryAccount = new Account(account.CloudName, account.APIKey, account.APISecret);
                cloudinarySources.Add(new Cloudinary(cloudinaryAccount));
            }

            _logger = logger;
        }

        public async Task<string> UploadImage(UploadSingleFileModel input)
        {
            try {
                Cloudinary imageCloudinary = cloudinarySources[(int)CloudinaryPurpose.UPLOAD_IMAGE];
                var file = input.File;
                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(file.FileName, file.OpenReadStream()),
                };
                ImageUploadResult uploadResult = await imageCloudinary.UploadAsync(uploadParams);
                return uploadResult.SecureUrl.AbsoluteUri;
            } catch (Exception e) {
                _logger.LogWarning(e, LoggingMessages.UploadService_ErrorMessage);
                throw;
            }
        }
    }
}
