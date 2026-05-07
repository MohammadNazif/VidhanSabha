using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Common.Dtos;
using VidhanSabha.Application.Common.multiplefilterparse;

namespace VidhanSabha.Application.Pannels.Admin.Sector.DTOs
{
    public class SectorQueryParams:BaseQueryParams
    {
        public int? Id { get; set;}
        public int? MandalId { get; set;}
        public string? MandalIds { get; set;}
        public int? SectorId { get; set;}
        public string? SectorIds { get; set;}
        public int? BoothId { get; set;}
        public  string? BoothIds { get; set;}
        public int? VillageId { get; set;}
        public string? VillageIds { get; set;}
        public int? CastId { get; set;}
        public string? CastIds { get; set;}


        public List<int> GetMandalIds() => FilterParse.ParseIds(MandalIds);
        public List<int> GetVillageIds() => FilterParse.ParseIds(VillageIds);
        public List<int> GetCastIds() => FilterParse.ParseIds(CastIds);
        public List<int> GetSectorIds() => FilterParse.ParseIds(SectorIds);
    }

    public class SectorVillageQueryParams
    {
        public string? UserId { get; set; }


    }
    }
