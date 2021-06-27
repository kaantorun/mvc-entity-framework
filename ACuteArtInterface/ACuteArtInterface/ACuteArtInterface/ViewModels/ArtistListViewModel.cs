using ACuteArtInterface.Helper;
using ACuteArtInterface.Models;
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
    public class ArtistListViewModel
    {
        public IList<ArtistModel> Artists { get; set; }
    }
}

