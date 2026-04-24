
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VidhanSabha.Domain.Entities.Admin;
using VidhanSabha.Domain.Entities.SuperAdmin;
using VidhanSabha.Domain.Enums;

namespace VidhanSabha.Domain.Entities.Auth
{
  

    [Table("Tbl_LoginCredential")]
    public class Tbl_LoginCredential
    {
        [Key]
        [Column("login_id")]
        public string LoginId { get; set; }


        [Column("user_id")]
        public string UserId { get; set; }

        [Column("username")]
        public string Username { get; set; }

        [Column("password")]
        public string Password { get; set; }

        [Column("role")]
        public PrabhariRole Role { get; set; }

        [Column("mobile")]
        public string Mobile { get; set; }

        [Column("Status")]
        public bool Status { get; set; } = true;

      

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public bool CanLogin() => Status;


    }
}
