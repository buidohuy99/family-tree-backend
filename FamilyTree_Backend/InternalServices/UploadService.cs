using FamilyTreeBackend.Core.Application.Helpers.ConfigModels;
using FamilyTreeBackend.Core.Application.Helpers.Exceptions.UploadExceptions;
using FamilyTreeBackend.Core.Application.Interfaces;
using FamilyTreeBackend.Core.Application.Models.FileUpload;
using FamilyTreeBackend.Core.Domain.Constants;
using FamilyTreeBackend.Core.Domain.Enums;
using Imagekit;
using Microsoft.AspNetCore.Http;
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

        public async Task<string> UploadSingleImage(UploadSingleFileModel input)
        {
            const long IMAGE_SIZE_LIMIT = 2097152; //2,000,000 bytes limit = 2MB

            var file = input.File;
            if (file.Length > IMAGE_SIZE_LIMIT)
            {
                throw new FileSizeExceedLimitException(UploadFileExceptionMessages.UploadFileLimitExceeded, file.FileName, file.Length, 
                    IMAGE_SIZE_LIMIT);
            }
            try {
                return await UploadImage(input.File);
            } catch (Exception) {
                throw;
            }
        }

        public async Task<IEnumerable<string>> UploadMutipleImages(
            UploadMutipleFilesModel input) 
        {
            //allow maybe 10MB?
            const long IMAGE_SIZE_LIMIT = 10485760;
            List<string> result = new List<string>();
            
            //check if the list has any picture that is oversized
            foreach (var file in input.Files)
            {
                if (file.Length > IMAGE_SIZE_LIMIT)
                {
                    throw new FileSizeExceedLimitException(
                        UploadFileExceptionMessages.UploadFileLimitExceeded, 
                        file.FileName, file.Length, IMAGE_SIZE_LIMIT);
                }
            }

            List<Task> tasks = new List<Task>();
            try
            {
                foreach (var file in input.Files)
                {
                    var task = UploadImage(file);
                    tasks.Add(task);
                }
                await Task.WhenAll(tasks);

                foreach (var task in tasks)
                {
                    var returnedUrl = ((Task<string>)task).Result;
                    result.Add(returnedUrl);
                }
                return result;
            }
            catch(Exception)
            {
                throw;
            }
            
        }

        private async Task<string> UploadImage(IFormFile file)
        {
            ServerImagekit imagekit = imagekitSources[0];
            using (var memoryStream = new MemoryStream())
            {
                var fileStream = file.OpenReadStream();
                await fileStream.CopyToAsync(memoryStream);
                fileStream.Close();
                ImagekitResponse uploadResult = await imagekit.FileName(file.FileName).UploadAsync(memoryStream.ToArray());
                memoryStream.Close();
                return uploadResult.URL;
            }
        }
    }
}
