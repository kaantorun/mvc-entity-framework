using ACuteArtInterface.Helper;
using ACuteArtInterface.ViewModels;
using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static ACuteArtInterface.Helper.Validations;

namespace ACuteArtInterface.Models
{
    [Table("ACCArtist")]
    public class ArtistModel : ArtistViewModel
    {
        [Key]
        [Column("accartist_id")]
        public long ArtistId { get; set; }

        [Column("accartist_name")]
        public string Name { get; set; }

        [Column("accartist_creation")]
        public DateTime CreationTime { get; set; }

        [Column("accartist_guid")]
        public string Guid { get; set; }

        [Column("accartist_description")]
        public string Description { get; set; }

        [Display(Name = "Image Url (512x512)"), Column("accartist_img_url")]
        public string ImageUrl { get; set; }

        [Column("accartist_fullname")]
        public string FullName { get; set; }

        [Display(Name = "Order"), Column("accartist_order"), Required]
        public int Order { get; set; }

        [Display(Name = "Thumb Url (256x256)"), Column("accartist_thumb_url")]
        public string ThumbUrl { get; set; }

        [Display(Name = "Icon Url (128x128)"), Column("accartist_icon_url")]
        public string IconUrl { get; set; }

        [Column("accartist_active")]
        public bool Active { get; set; }
    }
}
