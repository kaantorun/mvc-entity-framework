using ACuteArtInterface.Helper;
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
    public class ArtistViewModel
    {
        [NotMappedAttribute]
        [MaxFileSize(100 * 1024)]
        [AllowedExtensions(new string[] { ".png", ".jpg", ".bmp", ".tif", ".tiff" })]
        [Display(Name = "Image Url (512x512)"), DataType(DataType.Upload)]
        public IFormFile ImageUrlFile { get; set; }

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

        [NotMappedAttribute]
        public string NameEn { get; set; }

        [NotMappedAttribute]
        public string FullNameEn { get; set; }

        [NotMappedAttribute]
        public string DescriptionEn { get; set; }
    }
}

