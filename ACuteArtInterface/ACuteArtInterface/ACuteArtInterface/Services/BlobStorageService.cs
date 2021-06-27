using ACuteArtInterface.Enums;
using ACuteArtInterface.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACuteArtInterface.Services
{
    public class BlobStorageService
    {
        string _accessKey = string.Empty;
        string _containerName = string.Empty;

        public BlobStorageService(string accessKey, string containerName)
        {
            this._accessKey = accessKey;
            this._containerName = containerName;
        }

        public string UploadFileToBlob(AzureBlobFile fileDetail)
        {
            try
            {
                var _task = Task.Run(() => this.UploadFileToBlobAsync(fileDetail.FileBytes, fileDetail.MimeType, fileDetail));
                _task.Wait();
                string fileUrl = _task.Result;
                return fileUrl;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public async void DeleteBlobData(string fileUrl, string path)
        {
            Uri uriObj = new Uri(fileUrl);
            string BlobName = Path.GetFileName(uriObj.LocalPath);

            CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(this._accessKey);
            CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
            CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(this._containerName);

            CloudBlobDirectory blobDirectory = cloudBlobContainer.GetDirectoryReference(path);
            // get block blob refarence    
            CloudBlockBlob blockBlob = blobDirectory.GetBlockBlobReference(BlobName);

            if (await blockBlob.ExistsAsync())
            {
                // delete blob from container        
                await blockBlob.DeleteAsync();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileData"></param>
        /// <param name="fileMimeType"></param>
        /// <param name="fileDetail"></param>
        /// <param name="filePathToDelete"></param>
        /// <returns></returns>
        private async Task<string> UploadFileToBlobAsync(byte[] fileData, string fileMimeType, AzureBlobFile fileDetail)
        {
            try
            {
                CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(this._accessKey);
                CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
                CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(this._containerName);


                string fileName = Helper.Helper.GenerateFileName(fileDetail);

                if (await cloudBlobContainer.CreateIfNotExistsAsync())
                {
                    await cloudBlobContainer.SetPermissionsAsync(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });
                }

                if (fileName != null && fileData != null)
                {
                    CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(fileName);
                    cloudBlockBlob.Properties.ContentType = fileMimeType;
                    await cloudBlockBlob.UploadFromByteArrayAsync(fileData, 0, fileData.Length);
                    return cloudBlockBlob.Uri.AbsoluteUri;
                }
                return "";
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
    }
}
