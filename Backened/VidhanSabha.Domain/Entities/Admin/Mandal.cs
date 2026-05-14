using VidhanSabha.Domain.Entities.Common;

namespace VidhanSabha.Domain.Entities.Admin
{
    public class Tbl_Mandal
    {
        private string _name;
        private Tbl_Mandal() { }

        public int Id { get; private set; }
        public int? VidhanId { get; private set; }
        public string Name { get => _name; private set => _name = value; }
        public bool Status { get; private set; }
        public bool IsMandalSanyojak { get; private set; }

        public ICollection<Tbl_Sector>? Sectors { get; private set; }
        public Tbl_MandalSanyojak? Sanyojak { get; private set; }

        public static Tbl_Mandal Create(int? vidhanId, string name)
            => Create(vidhanId, name, false, null);

        public static Tbl_Mandal Create(
            int? vidhanId,
            string name,
            bool isMandalSanyojak,
            Tbl_MandalSanyojak? sanyojak)
        {
            ValidateVidhanId(vidhanId);
            ValidateName(name);

            return new Tbl_Mandal
            {
                VidhanId = vidhanId,
                _name = name.Trim(),
                Status = true,
                IsMandalSanyojak = isMandalSanyojak,
                Sanyojak = sanyojak
            };
        }

        // FIX 1: single method owns all three mutations — no private-set bypass needed
        public void Update(string name, bool isMandalSanyojak, Tbl_MandalSanyojak? sanyojak)
        {
            ValidateName(name);
            _name = name.Trim();
            Status = true;
            IsMandalSanyojak = isMandalSanyojak;
            Sanyojak = sanyojak;
        }

        public bool IsActive() => Status;

        public void Activate()
        {
            if (Status) throw new InvalidOperationException("Mandal is already active.");
            Status = true;
        }

        public void Deactivate()
        {
            if (!Status) throw new InvalidOperationException("Mandal is already inactive.");
            Status = false;
        }

        private static void ValidateVidhanId(int? vidhanId)
        {
            if (vidhanId is null or <= 0)
                throw new ArgumentException("VidhanId must be greater than 0.");
        }

        private static void ValidateName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Mandal name is required.");
            if (name.Trim().Length > 255)
                throw new ArgumentException("Mandal name cannot exceed 255 characters.");
        }

        public class Tbl_MandalSanyojak
        {
            public int Id { get; private set; }
            public string InchargeName { get; private set; }
            public int Age { get; private set; }
            public string? UserId { get; set; }
            public string? Contact { get; set; }
            public int MandalId { get; private set; }
            public string FatherName { get; private set; }
            public int CategoryId { get; private set; }
            public int CastId { get; private set; }
            public string? EducationLevel { get; private set; }
            public string PhoneNumber { get; private set; }
            public string? Address { get; private set; }
            public string? ProfileImagePath { get; private set; }
            public Tbl_Cast? Cast { get; private set; }
            public Tbl_Mandal? Mandal { get; set; }
            public bool Status { get; private set; } = true;

            private Tbl_MandalSanyojak() { }

            public static Tbl_MandalSanyojak Create(
                string userId, string inchargeName, int age, string fatherName, string contact,
                int categoryId, int castId, string? educationLevel,
                string phoneNumber, string? address, string? profileImagePath)
                => new()
                {
                    UserId = userId,
                    InchargeName = inchargeName,
                    Age = age,
                    FatherName = fatherName,
                    Contact = contact,
                    CategoryId = categoryId,
                    CastId = castId,
                    EducationLevel = educationLevel,
                    PhoneNumber = phoneNumber,
                    Address = address,
                    ProfileImagePath = profileImagePath
                };

            public void SetImage(string path) => ProfileImagePath = path;

            public void UpdateProfile(
                string inchargeName, int age, string fatherName,
                int categoryId, int castId, string? educationLevel,
                string phoneNumber, string? address, string? profileImagePath)
            {
                if (string.IsNullOrWhiteSpace(inchargeName))
                    throw new ArgumentException("Name required");
                if (age <= 0)
                    throw new ArgumentException("Invalid age");

                InchargeName = inchargeName;
                Age = age;
                FatherName = fatherName;
                CategoryId = categoryId;
                CastId = castId;
                EducationLevel = educationLevel;
                PhoneNumber = phoneNumber;
                Address = address;
                ProfileImagePath = profileImagePath;
            }

            public void Delete() => Status = false;
        }
    }
}