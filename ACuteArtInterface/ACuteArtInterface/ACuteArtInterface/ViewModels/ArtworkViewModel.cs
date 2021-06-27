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
    public class ArtworkViewModel
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
        [MaxFileSize(100 * 1024)]
        [AllowedExtensions(new string[] { ".png", ".jpg", ".bmp", ".tif", ".tiff" })]
        [Display(Name = "Trademark URL"), DataType(DataType.Upload)]
        public IFormFile TradeUrlFile { get; set; }

        [NotMappedAttribute]
        [MaxFileSize(100 * 1024)]
        [AllowedExtensions(new string[] { ".png", ".jpg", ".bmp", ".tif", ".tiff" })]
        [Display(Name = "Signature URL"), DataType(DataType.Upload)]
        public IFormFile SignUrlFile { get; set; }

        [NotMappedAttribute]
        [Display(Name = "Ios Url"),  DataType(DataType.Upload)]
        public IFormFile IosFile { get; set; }

        [NotMappedAttribute]
        [Display(Name = "Android Url"),  DataType(DataType.Upload)]
        public IFormFile AndroidFile { get; set; }

        [NotMappedAttribute]
        public string NameEn { get; set; }

        [NotMappedAttribute]
        public string DescriptionEn { get; set; }

        [NotMappedAttribute]
        public string ArtistNameEn { get; set; }

        [NotMappedAttribute]
        public string ColorEn { get; set; }

        [NotMappedAttribute]
        public virtual TypeItem TypeItem { get; set; }

        [NotMappedAttribute]
        public virtual bool Checked { get; set; }

        public virtual ArtistModel Artist { get; set; }
    }
}

