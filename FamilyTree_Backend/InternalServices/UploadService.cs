using FamilyTreeBackend.Core.Application.Helpers.ConfigModels;
using FamilyTreeBackend.Core.Application.Interfaces;
using FamilyTreeBackend.Core.Application.Models.FileUpload;
using FamilyTreeBackend.Core.Domain.Constants;
using FamilyTreeBackend.Core.Domain.Enums;
using Imagekit;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyTreeBackend.Infrastructure.Service.InternalServices
{
    public class UploadService : IUploadService
    {
        private List<ServerImagekit> imagekitSources;
        private ILogger<UploadService> _logger;

        public UploadService(IOptions<ImageKitAccounts> imagekit, ILogger<UploadService> logger)
        {
            imagekitSources = new List<ServerImagekit>();
            foreach (var account in imagekit.Value.Accounts) {
                // Imagekit for image upload
                ServerImagekit imagekitAccount = new ServerImagekit(account.PublicKey, account.PrivateKey, account.URLEndpoint);
                imagekitSources.Add(imagekitAccount);
            }

            _logger = logger;
        }

        public async Task<string> UploadImage(UploadSingleFileModel input)
        {
            try {
                ServerImagekit imagekit = imagekitSources[0];
                var file = input.File;

                using (var memoryStream = new MemoryStream())
                {
                    var fileStream = file.OpenReadStream();
                    await fileStream.CopyToAsync(memoryStream);
                    fileStream.Close();
                    ImagekitResponse uploadResult = await imagekit.FileName(file.FileName).UploadAsync(memoryStream.ToArray());
                    memoryStream.Close();
                    return uploadResult.URL;
                }
            } catch (Exception e) {
                _logger.LogWarning(e, LoggingMessages.UploadService_ErrorMessage);
                throw;
            }
        }
    }
}
