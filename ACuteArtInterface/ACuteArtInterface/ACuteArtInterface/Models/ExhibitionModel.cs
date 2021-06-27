using ACuteArtInterface.Models;
using ACuteArtInterface.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ACuteArtInterface.Models
{
    [Table("ACCExhibition")]
    public class ExhibitionModel : ExhibitionViewModel
    {
        [Key]
        [Column("accexhibition_id")]
        public long ExhibitionId { get; set; }

        [Display(Name = "Owner"), Column("accexhibition_owner"), Required]
        public long OwnerId { get; set; }

        [Column("accexhibition_title")]
        public string Title { get; set; }

        [Column("accexhibition_map_url")]
        public string MapUrl { get; set; }

        [Column("accexhibition_sponsor_name")]
        public string SponsorName { get; set; }

        [Column("accexhibition_sponsor_images")]
        public string SponsorImages { get; set; }

        [Column("accexhibition_description")]
        public string Description { get; set; }

        [Display(Name = "Geo Fenced"), Column("accexhibition_geofenced"), Required]
        public bool GeoFenced { get; set; }

        [Display(Name = "Latitude"), Column("accexhibition_latitude"), Required]
        public double Latitude { get; set; }

        [Display(Name = "Longitude"), Column("accexhibition_longitude"), Required]
        public double Longitude { get; set; }

        [Display(Name = "Radius"), Column("accexhibition_radius"), Required]
        public double Radius { get; set; }

        [Column("accexhibition_metadata")]
        public string MetaData { get; set; }

        [Column("accexhibition_created")]
        public DateTime? Created { get; set; }

        [Display(Name = "Start Date"), Column("accexhibition_startdate"), Required]
        public DateTime StartDate { get; set; }

        [Display(Name = "End Date"), Column("accexhibition_enddate"), Required]
        public DateTime EndDate { get; set; }

        [Display(Name = "Active"), Column("accexhibition_active"), Required]
        public bool Active { get; set; }

        [Column("accexhibition_guid")]
        public string Guid { get; set; }

        [Display(Name = "Order"), Column("accexhibition_order"), Required]
        public int Order { get; set; }

        [Column("accexhibition_icon_url")]
        public string IconUrl { get; set; }

        [Column("accexhibition_thumb_url")]
        public string ThumbUrl { get; set; }

        [Column("accexhibition_main_map_url")]
        public string MainMapUrl { get; set; }

        [Display(Name = "Instruction URL"), Column("accexhibition_intro_url")]
        public string IntroUrl { get; set; }

        [Display(Name = "Use Gps"), Column("accexhibition_use_gps"), Required]
        public bool UseGps { get; set; }

        [Display(Name = "Scan Radius"), Column("accexhibition_scan_radius"), Required]
        public double ScanRadius { get; set; }

        [Display(Name = "Show Radius"), Column("accexhibition_show_radius"), Required]
        public double ShowRadius { get; set; }

        [Display(Name = "View Radius"), Column("accexhibition_view_radius"), Required]
        public double ViewRadius { get; set; }

        [Column("accexhibition_howto")]
        public string Howto { get; set; }
    }
}
