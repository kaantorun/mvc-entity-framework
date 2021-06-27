using ACuteArtInterface.Helper;
using ACuteArtInterface.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using static ACuteArtInterface.Helper.Validations;

namespace ACuteArtInterface.ViewModels
{
    public class ExhibitionRoomViewModel
    {
        [NotMappedAttribute]
        [MaxFileSize(100 * 1024)]
        [AllowedExtensions(new string[] { ".png", ".jpg", ".bmp", ".tif", ".tiff" })]
        [Display(Name = "On The Map Tab"), DataType(DataType.Upload)]
        public IFormFile MapIconUrlFile { get; set; }

        [NotMappedAttribute]
        [MaxFileSize(100 * 1024)]
        [AllowedExtensions(new string[] { ".png", ".jpg", ".bmp", ".tif", ".tiff" })]
        [Display(Name = "Map Url"), DataType(DataType.Upload)]
        public IFormFile MapUrlFile { get; set; }

        [NotMappedAttribute]
        [MaxFileSize(100 * 1024)]
        [AllowedExtensions(new string[] { ".png", ".jpg", ".bmp", ".tif", ".tiff" })]
        [Display(Name = "Thumb Url (256x256)"), DataType(DataType.Upload)]
        public IFormFile ThumbUrlFile { get; set; }

        [NotMappedAttribute]
        [MaxFileSize(100 * 1024)]
        [AllowedExtensions(new string[] { ".png", ".jpg", ".bmp", ".tif", ".tiff" })]
        [Display(Name = "Icon Url (128x128)"), DataType(DataType.Upload)]
        public IFormFile IconUrlFile { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual ExhibitionModel Exhibition { get; set; }

        [NotMappedAttribute]
        public string TitleEn { get; set; }

        [NotMappedAttribute]
        public string DescriptionEn { get; set; }
    }
}

