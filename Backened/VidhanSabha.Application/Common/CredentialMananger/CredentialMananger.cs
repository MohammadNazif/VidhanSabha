using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Common.CredentialMananger.Dto;
using VidhanSabha.Application.Common.CredentialMananger.Interface;
using VidhanSabha.Application.Exceptions;
using VidhanSabha.Domain.Entities.Auth;
using VidhanSabha.Domain.Enums;

namespace VidhanSabha.Application.Common.CredentialMananger
{
    // Application/Services/CredentialManager.cs
    public class CredentialManagerFunc
    {
        private readonly ICredentialRepository _repo;

        public CredentialManagerFunc(ICredentialRepository repo)
        {
            _repo = repo;
        }

        public string GeneratePassword()
        {
            const string upper = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string digits = "0123456789";
            const string special = "@#$!";
            const string all = upper + "abcdefghijklmnopqrstuvwxyz" + digits + special;

            var rng = new Random();
            var chars = new List<char>
        {
            upper[rng.Next(upper.Length)],
            digits[rng.Next(digits.Length)],
            special[rng.Next(special.Length)],
            all[rng.Next(all.Length)],
            all[rng.Next(all.Length)],
            all[rng.Next(all.Length)],
        };

            return new string(chars.OrderBy(_ => rng.Next()).ToArray());
        }

        public async Task<CredentialDto> InsertCredentialAsync(string userId, string mobile, string email, PrabhariRole role)
        {
            var rawPassword = GeneratePassword();

            var login = new Tbl_LoginCredential
            {
                LoginId = $"LGN_{DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()}",
                UserId = userId,
                Username = mobile,
                Password = rawPassword,
                Role = role,
                Mobile = mobile,
                Status = true,
                CreatedAt = DateTime.UtcNow
            };

            var saved = await _repo.InsertAsync(login);

            return new CredentialDto
            {
                LoginId = saved.LoginId,
                UserId = saved.UserId,
                Username = saved.Username,
                Password = rawPassword,   // raw — send to user via SMS/email
                Role = saved.Role
            };

           
        }
        public async Task<CredentialDto> UpdateCredentialAsync(string userId, string mobile)
        {
            // ✅ Step 1 — Get existing record
            var login = await _repo.GetByUserIdAsync(userId);

            if (login == null)
                throw new NotFoundException("Login Credential Not Found");

            // ✅ Step 2 — Update fields
               login.Username = mobile;
               login.Mobile = mobile;

            // (optional if you want username = mobile always)
          // add if you have column

            // ✅ Step 3 — Save proper entity
            var saved = await _repo.UpdateAsync(login);

            return new CredentialDto
            {
                LoginId = saved.LoginId,
                UserId = saved.UserId,
                Username = saved.Username,
                Password = null, // don’t return password on update
                Role = saved.Role
            };
        }
        public async Task<Tbl_LoginCredential> Delete(string userId)
        {
            var data =  await _repo.GetByUserIdAsync(userId);
            if (data == null)
            {
                throw new NotFoundException("Login Credential Not Found");
            }
             data.Status = false;

              await  _repo.SoftDeleteAsync(data);

            return data;
            
        }
    }
}
