using FamilyTreeBackend.Core.Application.Helpers.ConfigModels;
using FamilyTreeBackend.Core.Application.Helpers.Exceptions.UploadExceptions;
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

        public UploadService(IOptions<ImageKitAccounts> imagekit, ILogger<UploadService> logger)
        {
            imagekitSources = new List<ServerImagekit>();
            foreach (var account in imagekit.Value.Accounts) {
                // Imagekit for image upload
                ServerImagekit imagekitAccount = new ServerImagekit(account.PublicKey, account.PrivateKey, account.URLEndpoint);
                imagekitSources.Add(imagekitAccount);
            }

        }

        public async Task<string> UploadImage(UploadSingleFileModel input, long imageSizeLimit = 2097152)
        {
            long IMAGE_SIZE_LIMIT = imageSizeLimit; //2,000,000 bytes limit = 2MB

            try {
                ServerImagekit imagekit = imagekitSources[0];
                var file = input.File;

                if (file.Length > imageSizeLimit)
                {
                    throw new FileSizeExceedLimitException(UploadFileExceptionMessages.UploadFileLimitExceeded, file.FileName, file.Length, IMAGE_SIZE_LIMIT);
                }

                using (var memoryStream = new MemoryStream())
                {
                    var fileStream = file.OpenReadStream();
                    await fileStream.CopyToAsync(memoryStream);
                    fileStream.Close();
                    ImagekitResponse uploadResult = await imagekit.FileName(file.FileName).UploadAsync(memoryStream.ToArray());
                    memoryStream.Close();
                    return uploadResult.URL;
                }
            } catch (Exception) {
                throw;
            }
        }

        public async Task<IEnumerable<string>> UploadMutipleImages(
            IEnumerable<UploadSingleFileModel> input, 
            long imageSizeLimit = 2097152) 
        {
            List<string> result = new List<string>();
            List<Task> tasks = new List<Task>();
            foreach (var model in input)
            {
                var task = UploadImage(model, imageSizeLimit);
                tasks.Add(task);
            }
            await Task.WhenAll(tasks);

            foreach(var task in tasks)
            {
                var returnedUrl = ((Task<string>)task).Result;
                result.Add(returnedUrl);
            }
            return result;
        }
    }
}
