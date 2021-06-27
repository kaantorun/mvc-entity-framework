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
    public class AssetViewModel
    {
        [NotMappedAttribute]
        [MaxFileSize(100 * 1024)]
        [AllowedExtensions(new string[] { ".png", ".jpg", ".bmp", ".tif", ".tiff" })]
        [Display(Name = "Intro Image Url"), DataType(DataType.Upload)]
        public IFormFile AssetValueFile { get; set; }
    }
}

