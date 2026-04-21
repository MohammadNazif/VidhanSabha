using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VidhanSabha.Domain.ValueObjects
{
    public class SeniorDisabledData
    {
        public string Name { get; }
        public string Address { get; }
        public int CategoryId { get; }
        public int CastId { get; }
        public string Mobile { get; }
        public string VoterId { get; }

        public SeniorDisabledData(
            string name, string address,
            int categoryId, int castId,
            string mobile, string voterId)
        {
            Name = name;
            Address = address;
            CategoryId = categoryId;
            CastId = castId;
            Mobile = mobile;
            VoterId = voterId;
        }
    }
}
