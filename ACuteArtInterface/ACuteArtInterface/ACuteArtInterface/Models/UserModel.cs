using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ACuteArtInterface.Models
{
    [Table("ACCUser")]
    public class UserModel
    {
        [Key]
        [Column("accuser_id")]
        public long UserId { get; set; }

        [Column("accuser_name")]
        public string Name { get; set; }

        [Column("accuser_guid")]
        public string Guid { get; set; }

        [Column("accuser_creationtime")]
        public DateTime CreationTime { get; set; }

        [Column("accuser_email")]
        public string Email { get; set; }

        [Column("accuser_device_id")]
        public string DeviceId { get; set; }

        [Column("accuser_unique_identifier")]
        public string UniqueIdentifier { get; set; }

        [Column("accuser_push_id")]
        public string PushId { get; set; }

        [Column("accuser_wpid")]
        public string WpId { get; set; }

        [Column("accuser_lastname")]
        public string LastName { get; set; }

        [Column("accuser_image_url")]
        public string ImageUrl { get; set; }

        [Column("accuser_role")]
        public string Role { get; set; }

        [Column("accuser_profile_crop")]
        public string ProfileCrop { get; set; }

        [Column("accuser_password")]
        public string Password { get; set; }

        [Column("accuser_blocked")]
        public bool Blocked { get; set; }

        [NotMappedAttribute]
        public bool Selected { get; set; }

        [NotMappedAttribute]
        public long ExhibitionId { get; set; }

    }
}
