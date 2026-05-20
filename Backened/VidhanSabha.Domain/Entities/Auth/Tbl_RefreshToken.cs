using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VidhanSabha.Domain.Entities.Auth
{
    public class Tbl_RefreshToken
    {
        public int Id { get; set; }
        public string Token { get; set; }

        public string UserId { get; set; }
        public string DeviceType { get; set; }       
        public DateTime ExpiresAt { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsRevoked { get; set; }

        public Tbl_LoginCredential User { get; set; }
        public bool IsActive => !IsRevoked && DateTime.UtcNow < ExpiresAt;
    }
}
