
using ACuteArtInterface.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ACuteArtInterface.Models
{
    public class ArtworkExhibitionCheckModel
    {
        public long ArtworkId { get; set; }
        public string Name { get; set; }
        public List<SelectListItem> drpSubjects { get; set; }

        [Display(Name = "Artworks")]
        public long[] SubjectsIds { get; set; }
    }
}