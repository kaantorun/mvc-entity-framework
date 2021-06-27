using ACuteArtInterface.Helper;
using ACuteArtInterface.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using static ACuteArtInterface.Helper.Validations;

namespace ACuteArtInterface.ViewModels
{
    public class ExhibitionViewModel
    {
        [NotMappedAttribute]
        [MaxFileSize(100 * 1024)]
        [AllowedExtensions(new string[] { ".png", ".jpg", ".bmp", ".tif", ".tiff" })]
        [Display(Name = "Map Image URL"), DataType(DataType.Upload)]
        public IFormFile MainMapUrlFile { get; set; }

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

        [NotMappedAttribute]
        [Display(Name = "Instruction URL"), DataType(DataType.Upload)]
        public IFormFile IntroUrlFile { get; set; }

        [NotMappedAttribute]
        [Display(Name = "How To"), DataType(DataType.Upload)]
        public IFormFile HowtoFile { get; set; }

        [NotMappedAttribute]
        public virtual List<UserModel> Users { get; set; }

        [NotMappedAttribute]
        public virtual List<ExhibitionArtworkModel> ExhibitionArtworks { get; set; }

        [NotMappedAttribute]
        public virtual ArtworkModel Artwork { get; set; }

        public virtual UserModel Owner { get; set; }

        public virtual ICollection<ExhibitionArtworkModel> Artworks { get; set; }

        [NotMappedAttribute]
        public string DescriptionEn { get; set; }

        [NotMappedAttribute]
        public IEnumerable<SelectListItem> drpExhibitionArtwork { get; set; }

        [NotMappedAttribute]
        [Display(Name = "Exhibitions")]
        public long[] ExhibitionArtworkIds { get; set; }
    }
}

