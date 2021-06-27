using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ACuteArtInterface.Models
{
    [Table("ACCExhibitionArtwork")]
    public class ExhibitionArtworkModel
    {
        [Key]
        [Column("accexhibition_art_id")]
        public long ArtId { get; set; }

        /// <summary>
        /// FK From ACCArtwork
        /// </summary>
        [ForeignKey("accexhibition_art_artwork_id")]
        [Column("accexhibition_art_artwork_id")]
        public long ArtworkId { get; set; }

        [Column("accexhibition_art_order")]
        public int Order { get; set; }

        [Column("accexhibition_art_active")]
        public bool Active { get; set; }

        /// <summary>
        /// FK From ACCExhibition
        /// </summary>
        [ForeignKey("Exhibition")]
        [Column("accexhibition_art_exhibition_id")]
        public long ExhibitionId { get; set; }

        public virtual ExhibitionModel Exhibition { get; set; }

        public virtual ArtworkModel Artwork { get; set; }
    }
}
