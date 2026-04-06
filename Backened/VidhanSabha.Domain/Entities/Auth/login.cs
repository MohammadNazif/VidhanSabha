using System;

namespace VidhanSabha.Domain.Entities.Auth
{
    public class Tbl_Login
    {
        // ── Private backing fields ──────────────────────────────
        private string _mobileNumber;
        private string _password;
        private string _role;

        // ── Private constructor (forces use of factory method) ──
        private Tbl_Login() { }

        // ── Properties ──────────────────────────────────────────
        public int UserId { get; private set; }
        public string MobileNumber { get => _mobileNumber; private set => _mobileNumber = value; }
        public string Password { get => _password; private set => _password = value; }
        public string Role { get => _role; private set => _role = value; }
        public bool Status { get; private set; } = false;
        public DateTime CreatedAt { get; private set; }

        // ── Factory Method (only valid way to create a user) ────
        public static Tbl_Login Create(string mobileNumber, string plainPassword, string role)
        {
            ValidateMobile(mobileNumber);
            ValidatePassword(plainPassword);
            ValidateRole(role);

            return new Tbl_Login
            {
                _mobileNumber = mobileNumber.Trim(),
                _password = plainPassword, 
                _role = role.Trim(),
                Status = true,
                CreatedAt = DateTime.UtcNow
            };
        }

        // ── Domain Behaviors ────────────────────────────────────

        // Verify login attempt
        public bool VerifyPassword(string plainPassword)
        {
            if (string.IsNullOrWhiteSpace(plainPassword))
                throw new ArgumentException("Password cannot be empty.");

            return plainPassword == _password;
        }

        // Activate account
        public void Activate()
        {
            if (Status)
                throw new InvalidOperationException("Account is already active.");
            Status = true;
        }

        // Deactivate account
        public void Deactivate()
        {
            if (!Status)
                throw new InvalidOperationException("Account is already inactive.");
            Status = false;
        }

        // Change password
        public void ChangePassword(string oldPassword, string newPassword)
        {
            if (!VerifyPassword(oldPassword))
                throw new UnauthorizedAccessException("Current password is incorrect.");

            ValidatePassword(newPassword);
            _password = newPassword; // Store as plain text
        }

        // Update role
        public void ChangeRole(string newRole)
        {
            ValidateRole(newRole);
            _role = newRole.Trim();
        }

        // Check if account can login
        public bool CanLogin() => Status;

        // ── Private Helpers ─────────────────────────────────────

        private static void ValidateMobile(string mobile)
        {
            if (string.IsNullOrWhiteSpace(mobile))
                throw new ArgumentException("Mobile number is required.");

            if (mobile.Trim().Length > 15)
                throw new ArgumentException("Mobile number cannot exceed 15 characters.");

            if (!System.Text.RegularExpressions.Regex.IsMatch(mobile.Trim(), @"^\d+$"))
                throw new ArgumentException("Mobile number must contain digits only.");
        }

        private static void ValidatePassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Password is required.");

            if (password.Length < 6)
                throw new ArgumentException("Password must be at least 6 characters.");
        }

        private static void ValidateRole(string role)
        {
            var allowed = new[] { "Admin", "User", "Manager" };

            if (string.IsNullOrWhiteSpace(role))
                throw new ArgumentException("Role is required.");

            if (!allowed.Contains(role.Trim()))
                throw new ArgumentException($"Role must be one of: {string.Join(", ", allowed)}");
        }
    }
}