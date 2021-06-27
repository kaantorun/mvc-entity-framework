using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ACuteArtInterface.Models
{
    public class Image
    {
        public string path { get; set; }
        public int id { get; set; }
        public string description { get; set; }
        [NotMapped]
        public IFormFile file { get; set; }
    }
}
