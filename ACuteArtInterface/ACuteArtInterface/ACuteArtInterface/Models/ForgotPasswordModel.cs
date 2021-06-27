using System.ComponentModel.DataAnnotations;

namespace ACuteArtInterface.Models
{
    public class ForgotPasswordModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
