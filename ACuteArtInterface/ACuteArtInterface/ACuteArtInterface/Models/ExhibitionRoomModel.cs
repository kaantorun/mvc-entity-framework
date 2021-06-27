using ACuteArtInterface.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ACuteArtInterface.Models
{
    [Table("ACCExhibitionRoom")]
    public class ExhibitionRoomModel : ExhibitionRoomViewModel
    {
        [Key]
        [Column("accexhibition_room_id")]
        public long RoomId { get; set; }

        [Column("accexhibition_room_title")]
        public string Title { get; set; }

        [Column("accexhibition_room_va_guid")]
        public string VaGuid { get; set; }

        [Column("accexhibition_room_icon_url")]
        public string IconUrl { get; set; }

        [Column("accexhibition_room_thumb_url")]
        public string ThumbUrl { get; set; }

        [Column("accexhibition_room_map_url")]
        public string MapUrl { get; set; }

        [Column("accexhibition_room_description")]
        public string Description { get; set; }

        /// <summary>
        /// FK From ACCExhibition
        /// </summary>
        [Display(Name = "Exhibition"), Column("accexhibition_room_exhibition"), Required, ForeignKey("Exhibition")]
        public long ExhibitionId { get; set; }

        [ForeignKey("accexhibition_room_va")]
        [Column("accexhibition_room_va")]
        public long? Va { get; set; }

        [Display(Name = "Order"), Column("accexhibition_room_order"), Required]
        public int Order { get; set; }

        [Display(Name = "Scan Radius"), Column("accexhibition_room_scan_radius"), Required]
        public double ScanRadius { get; set; }

        [Display(Name = "Show On Map"), Column("accexhibition_room_show_on_map"), Required]
        public bool ShowOnMap { get; set; }

        [Display(Name = "On The Map Tab"), Column("accexhibition_room_map_icon_url")]
        public string MapIconUrl { get; set; }

        [Display(Name = "Active"), Column("accexhibition_room_active"), Required]
        public bool Active { get; set; }
    }
}
