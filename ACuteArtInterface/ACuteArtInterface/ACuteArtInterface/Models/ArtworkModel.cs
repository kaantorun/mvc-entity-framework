using ACuteArtInterface.Helper;
using ACuteArtInterface.ViewModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using static ACuteArtInterface.Helper.Validations;

namespace ACuteArtInterface.Models
{
    [Table("ACCArtwork")]
    public class ArtworkModel : ArtworkViewModel
    {
        [Key]
        [Column("accartwork_id")]
        public long ArtworkId { get; set; }

        [Display(Name = "Name"), Column("accartwork_name")]
        public string Name { get; set; }

        [Display(Name = "Description"), Column("accartwork_description")]
        public string Description { get; set; }

        [Display(Name = "Creation Date"), Column("accartwork_creation")]
        public DateTime Creation { get; set; }

        [Display(Name = "Artist"), Column("accartwork_artist"), ForeignKey("Artist")]
        public long ArtistId { get; set; }

        [Display(Name = "Type"), Column("accartwork_type"), Required]
        public int Type { get; set; }

        [Display(Name = "Source Meta"), Column("accartwork_source_meta")]
        public string SourceMeta { get; set; }

        [Display(Name = "Guid"), Column("accartwork_guid")]
        public string Guid { get; set; }

        [Display(Name = "Object Url (IOS / Android)"), Column("accartwork_object_url")]
        public string ObjectUrl { get; set; }

        [Display(Name = "Image Url (512x512)"), Column("accartwork_image_url")]
        public string ImageUrl { get; set; }

        [Display(Name = "Icon Url (128x128)"), Column("accartwork_icon_url")]
        public string IconUrl { get; set; }

        [Display(Name = "Wp Id"), Column("accartwork_wp_id")]
        public string WpId { get; set; }

        [Display(Name = "Prefab Name"), Column("accartwork_prefab_name")]
        public string PrefabName { get; set; }

        [Display(Name = "Marshal Code"), Column("accartwork_marshal_code")]
        public string MarshalCode { get; set; }

        [Display(Name = "Thumb Url (256x256)"), Column("accartwork_thumb_url")]
        public string ThumbUrl { get; set; }

        [Display(Name = "Signature URL"), Column("accartwork_sign_url")]
        public string SignUrl { get; set; }

        [Display(Name = "Trademark URL"), Column("accartwork_trade_url")]
        public string TradeUrl { get; set; }

        [Display(Name = "Map Icon Url"), Column("accartwork_map_icon_url")]
        public string MapIconUrl { get; set; }

        [Display(Name = "Size Description"), Column("accartwork_size_description")]
        public string SizeDescription { get; set; }

        [Display(Name = "Year"), Column("accartwork_year")]
        public int Year { get; set; }

        [Display(Name = "Edition Size"), Column("accartwork_edition_size")]
        public int EditionSize { get; set; }

        [Display(Name = "Color"), Column("accartwork_color")]
        public string Color { get; set; }

        [Display(Name = "Size"), Column("accartwork_size")]
        public string Size { get; set; }

        [Display(Name = "Ap Size"), Column("accartwork_ap_size"), Required]
        public int ApSize { get; set; }

        [Display(Name = "Preload"), Column("accartwork_preload"), Required]
        public bool Preload { get; set; }

        [Display(Name = "Active"), Column("accartwork_active"), Required]
        public bool Active { get; set; }

        [Column("accartwork_web_picture")]
        public string WebPicture { get; set; }

        [Column("accartwork_web_description")]
        public string WebDescription { get; set; }

        [Column("accartwork_web_intro_picture")]
        public string WebIntroPicture { get; set; }

        [Column("accartwork_web_carrosel")]
        public string WebCarrosel { get; set; }

        [Column("accartwork_hardware_specs")]
        public string HardwareSpecs { get; set; }
    }
}
