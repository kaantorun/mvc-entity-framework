using ACuteArtInterface.Enums;
using ACuteArtInterface.Helper;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ACuteArtInterface.Models
{
    public class AzureBlobFile
    {
        public string AfterHeaderFirst { get; set; }

        public string AfterHeaderSecond { get; set; }

        public FileTypes FileType { get; set; }

        public string FileGuid { get; set; }

        public FileExtensions FileExtension { get; set; }

        public InterfaceFileTypes InterfaceFileType { get; set; }

        public byte[] FileBytes { get; set; }

        public string MimeType { get; set; }

        public string AccessKey { get; set; }

        public string ContainerName { get; set; }

        public string FilePath { get; set; }

        public string PropertyName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="interfaceFileType"></param>
        /// <param name="file"></param>
        /// <param name="fileBytes"></param>
        /// <param name="headerFirst"></param>
        /// <param name="headerSecond"></param>
        /// <param name="azureStorageAccessKey"></param>
        /// <param name="azureContainerName"></param>
        public AzureBlobFile(InterfaceFileTypes interfaceFileType, IFormFile file, byte[] fileBytes, string headerFirst, string headerSecond, string azureStorageAccessKey, string azureContainerName)
        {
            string filePropertyName = file.Name.Replace("File", "");

            if (filePropertyName == "Ios")
            {
                FileExtension = FileExtensions.sec_ios;
                FileType = FileTypes.asset;
            }
            else if (filePropertyName == "Android")
            {
                FileExtension = FileExtensions.sec_android;
                FileType = FileTypes.asset;
            }
            else if (filePropertyName.ToLower() == "introurl" || filePropertyName.ToLower() == "howto")
            {
                FileExtension = FileExtensions.mp4;
                FileType = FileTypes.video;
            }
            else
            {
                FileExtension = FileExtensions.png;
                FileType = FileTypes.img;
            }

            FileGuid = Guid.NewGuid().ToString();
            AfterHeaderFirst = Helper.Helper.GetStringByLanguage(headerFirst, 0).PrepareStringToUpload();
            AfterHeaderSecond = Helper.Helper.PrepareStringToUpload(headerSecond);
            InterfaceFileType = interfaceFileType;
            FileBytes = fileBytes;
            MimeType = file.ContentType;
            AccessKey = azureStorageAccessKey;
            ContainerName = azureContainerName;
            PropertyName = (filePropertyName == "Ios" || filePropertyName == "Android" ? "ObjectUrl" : filePropertyName);

            FilePath = Helper.Helper.GenerateFilePath(this);
        }
    }
}
