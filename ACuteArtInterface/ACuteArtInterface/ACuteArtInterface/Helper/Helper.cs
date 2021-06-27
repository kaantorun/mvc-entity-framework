using ACuteArtInterface.Enums;
using ACuteArtInterface.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ACuteArtInterface.Helper
{
    public static class Helper
    {
        public static List<TypeItem> GetTypeItems()
        {
            List<TypeItem> typeList = new List<TypeItem>();
            typeList.Add(new TypeItem { TypeId = 0, Description = "AR" });
            typeList.Add(new TypeItem { TypeId = 1, Description = "Image" });
            typeList.Add(new TypeItem { TypeId = 2, Description = "Video" });
            typeList.Add(new TypeItem { TypeId = 3, Description = "VT" });

            return typeList;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetStringByLanguage(string value, int limit)
        {
            string valueKeyLanguage = value;
            string usCultureOpenKey = "<en-us>";
            string usCultureCloseKey = "</en-us>";

            if (!string.IsNullOrEmpty(value) && value.Contains(usCultureOpenKey))
            {
                valueKeyLanguage = value.Substring(0, value.IndexOf(usCultureCloseKey)).Replace(usCultureOpenKey, "");
            }

            if (!string.IsNullOrEmpty(value) && limit > 0 && valueKeyLanguage.Length > limit)
            {
                valueKeyLanguage = valueKeyLanguage.Substring(0, limit);
            }

            return valueKeyLanguage;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string PrepareStringToUpload(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                value = value.Replace(" ", "_").ToLower();
            }

            return value;
        }

        /// <summary>
        /// Extension for 'Object' that copies the properties to a destination object.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="destination">The destination.</param>
        public static void CopyProperties(this object source, object destination)
        {
            // If any this null throw an exception
            if (source == null || destination == null)
                throw new Exception("Source or/and Destination Objects are null");
            // Getting the Types of the objects
            Type typeDest = destination.GetType();
            Type typeSrc = source.GetType();

            // Iterate the Properties of the source instance and  
            // populate them from their desination counterparts  
            PropertyInfo[] srcProps = typeSrc.GetProperties();
            foreach (PropertyInfo srcProp in srcProps)
            {
                if (!srcProp.CanRead)
                {
                    continue;
                }
                PropertyInfo targetProperty = typeDest.GetProperty(srcProp.Name);
                if (targetProperty == null)
                {
                    continue;
                }
                if (!targetProperty.CanWrite)
                {
                    continue;
                }
                if (targetProperty.GetSetMethod(true) != null && targetProperty.GetSetMethod(true).IsPrivate)
                {
                    continue;
                }

                if ((targetProperty.GetSetMethod().Attributes & MethodAttributes.Static) != 0)
                {
                    continue;
                }
                if (!targetProperty.PropertyType.IsAssignableFrom(srcProp.PropertyType))
                {
                    continue;
                }

                // Passed all tests, lets set the value
                targetProperty.SetValue(destination, srcProp.GetValue(source, null), null);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetPropertyValue(this object source, string key)
        {
            // If any this null throw an exception
            if (source == null)
                throw new Exception("Source Object is null");
            // Getting the Types of the objects

            Type typeSrc = source.GetType();

            // Iterate the Properties of the source instance and  
            // populate them from their desination counterparts  
            PropertyInfo[] srcProps = typeSrc.GetProperties();

            PropertyInfo srcProp = typeSrc.GetProperty(key);
            // Passed all tests, lets set the value
            return srcProp.GetValue(source) != null ? srcProp.GetValue(source).ToString() : string.Empty;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void UpdateProperty(this object source, string key, string value)
        {
            // If any this null throw an exception
            if (source == null)
                throw new Exception("Source Object is null");
            // Getting the Types of the objects

            Type typeSrc = source.GetType();

            // Iterate the Properties of the source instance and  
            // populate them from their desination counterparts  
            PropertyInfo[] srcProps = typeSrc.GetProperties();

            PropertyInfo srcProp = typeSrc.GetProperty(key);
            // Passed all tests, lets set the value
            srcProp.SetValue(source, value, null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileDetail"></param>
        /// <returns></returns>
        public static string GenerateFilePath(AzureBlobFile fileDetail)
        {
            StringBuilder strBuilder = new StringBuilder();

            //add header
            strBuilder.Append(fileDetail.InterfaceFileType.ToString().ToLower());

            //add file type
            strBuilder.Append("/").Append(fileDetail.FileType.ToString().ToLower());

            //add first part of the url
            strBuilder.Append("/").Append(fileDetail.AfterHeaderFirst).Append("/");

            return strBuilder.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileDetail"></param>
        /// <returns></returns>
        public static string GenerateFileName(AzureBlobFile fileDetail)
        {
            StringBuilder strBuilder = new StringBuilder(GenerateFilePath(fileDetail));

            if (fileDetail.InterfaceFileType == InterfaceFileTypes.Artwork)
            {
                strBuilder.Append(fileDetail.AfterHeaderSecond).Append("_");
            }

            //add file guid
            strBuilder.Append(fileDetail.FileGuid);

            //add file extension
            if (fileDetail.FileExtension == FileExtensions.mp4 || fileDetail.FileExtension == FileExtensions.png)
            {
                strBuilder.Append(".");
            }
            else
            {
                strBuilder.Append("_");
            }

            strBuilder.Append(fileDetail.FileExtension.ToString().ToLower());

            return strBuilder.ToString();
        }
    }

    public static class StringExtension
    {
        // This is the extension method.
        // The first parameter takes the "this" modifier
        // and specifies the type for which the method is defined.
        public static string GetStringByLanguage(this string value, int limit)
        {
            string valueKeyLanguage = value;
            string usCultureOpenKey = "<en-us>";
            string usCultureCloseKey = "</en-us>";

            if (!string.IsNullOrEmpty(value) && value.Contains(usCultureOpenKey))
            {
                valueKeyLanguage = value.Substring(0, value.IndexOf(usCultureCloseKey)).Replace(usCultureOpenKey, "");
            }

            if (!string.IsNullOrEmpty(value) && limit > 0 && valueKeyLanguage.Length > limit)
            {
                valueKeyLanguage = valueKeyLanguage.Substring(0, limit);
            }

            return valueKeyLanguage;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string PrepareStringToUpload(this string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                value = value.Replace(" ", "_").ToLower();
            }

            return value;
        }
    }

    public static class FormFileExtensions
    {
        public static async Task<byte[]> GetBytes(this IFormFile formFile)
        {
            using (var memoryStream = new MemoryStream())
            {
                await formFile.CopyToAsync(memoryStream);
                return memoryStream.ToArray();
            }
        }
    }
}
