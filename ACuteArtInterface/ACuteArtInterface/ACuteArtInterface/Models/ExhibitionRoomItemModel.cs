using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ACuteArtInterface.Models
{
    [Table("ACCExhibitionRoomItem")]
    public class ExhibitionRoomItemModel
    {
        [Key]
        [Column("accexhibition_room_item_id")]
        public long RoomItemId { get; set; }

        /// <summary>
        /// FK From ACCArtwork
        /// </summary>
        [ForeignKey("Artwork")]
        [Column("accexhibition_room_item_artwork")]
        public long ArtworkId { get; set; }

        [Column("accexhibition_room_item_edition_number")]
        public int EditionNumber { get; set; }

        [Column("accexhibition_room_item_scale")]
        public double Scale { get; set; }

        [Column("accexhibition_room_item_offset")]
        public string Offset { get; set; }

        [Column("accexhibition_room_item_axis")]
        public string Axis { get; set; }

        [Column("accexhibition_room_item_rotaton")]
        public double Rotaton { get; set; }

        /// <summary>
        /// FK From ACCExhibitionRoom
        /// </summary>
        [ForeignKey("ExhibitionRoom")]
        [Column("accexhibition_room_item_room")]
        public long RoomId { get; set; }

        [Column("accexhibition_room_item_order")]
        public int Order { get; set; }

        public virtual ExhibitionRoomModel ExhibitionRoom { get; set; }

        public virtual ArtworkModel Artwork { get; set; }
    }
}
