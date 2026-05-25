using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VidhanSabha.Application.Common.ExportPdfExcel.Dtos
{
    public sealed class SectorWithBoothReportExportRow
    {
        public string MandalName { get; set; } = "";
        public string SectorName { get; set; } = "";
        public string InchargeName { get; set; } = "";
        public string Age { get; set; } = "";
        public string FatherName { get; set; } = "";
        public string CastName { get; set; } = "";
        public string EducationLevel { get; set; } = "";
        public string PhoneNumber { get; set; } = "";
        public string Address { get; set; } = "";
        public string ProfileImage { get; set; } = "";

        public int? BoothNumber { get; set; }
        public string BoothPollingStation { get; set; } = "";
        public string SanyojakName { get; set; } = "";
        public string SanyojakPhone { get; set; } = "";
        public string SanyojakFatherName { get; set; } = "";
        public string SanyojakAge { get; set; } = "";
        public string SanyojakCastName { get; set; } = "";
        public string SanyojakAddress { get; set; } = "";
        public string SanyojakEducation { get; set; } = "";

        public string SectorVillages { get; set; } = "";
        public string BoothVillages { get; set; } = "";
    }

    public sealed class SectorWithBoothReportExportDef : IExportDefinition<SectorWithBoothReportExportRow>
    {
        private readonly string _title;

        public SectorWithBoothReportExportDef(string title) => _title = title;

        public string ReportTitle => _title;

        public IReadOnlyList<ExportColumn<SectorWithBoothReportExportRow>> Columns =>
        [
            new() { Header = "Mandal", Value = r => r.MandalName, ExcelWidth = 25 },
        new() { Header = "Sector", Value = r => r.SectorName, ExcelWidth = 25 },
        new() { Header = "Sector Incharge", Value = r => r.InchargeName, ExcelWidth = 25 },
        new() { Header = "Age", Value = r => r.Age, ExcelWidth = 10 },
        new() { Header = "Father Name", Value = r => r.FatherName, ExcelWidth = 25 },
        new() { Header = "Cast", Value = r => r.CastName, ExcelWidth = 15 },
        new() { Header = "Education", Value = r => r.EducationLevel, ExcelWidth = 20 },
        new() { Header = "Phone", Value = r => r.PhoneNumber, ExcelWidth = 15 },
        new() { Header = "Address", Value = r => r.Address, ExcelWidth = 30 },
        new() { Header = "Booth No", Value = r => r.BoothNumber?.ToString() ?? "", ExcelWidth = 10 },
        new() { Header = "Booth Polling Station", Value = r => r.BoothPollingStation, ExcelWidth = 25 },
        new() { Header = "Sanyojak Name", Value = r => r.SanyojakName, ExcelWidth = 25 },
        new() { Header = "Sanyojak Phone", Value = r => r.SanyojakPhone, ExcelWidth = 15 },
        new() { Header = "Sanyojak Age", Value = r => r.SanyojakAge, ExcelWidth = 10 },
        new() { Header = "Sanyojak Cast", Value = r => r.SanyojakCastName, ExcelWidth = 15 },
        new() { Header = "Sector Villages", Value = r => r.SectorVillages, ExcelWidth = 30 },
        new() { Header = "Booth Villages", Value = r => r.BoothVillages, ExcelWidth = 30 }
        ];
    }

    public class SectorWithBoothReportExportFilter : BaseFilter
    {
        public string? UserId { get; set; }         
        public int? VidhanSabhaId { get; set; }     
        public int? MandalId { get; set; }          
        public int? SectorId { get; set; }          
        public int? BoothId { get; set; }          
        public int? VillageId { get; set; }       
        public int? CastId { get; set; }        
        public string? SearchTerm { get; set; }      
    }
}
