using ACuteArtInterface.Helper;
using ACuteArtInterface.ViewModels;
using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static ACuteArtInterface.Helper.Validations;

namespace ACuteArtInterface.Models
{
    [Table("ACCCopy")]
    public class AssetModel : AssetViewModel
    {
        [Key]
        [Column("acccopy_id")]
        public long AssetId { get; set; }

        [Column("acccopy_key")]
        public string AssetKey { get; set; }

        [Display(Name = "Hero Intro Image Url"), Column("acccopy_value")]
        public string AssetValue { get; set; }
    }
}
