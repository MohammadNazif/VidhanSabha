using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Common.ExportPdfExcel;
using VidhanSabha.Application.Common.ExportPdfExcel.Dtos;
using VidhanSabha.Application.Common.multiplefilterparse;

namespace VidhanSabha.Application.Common.ExportPdfExcel.Dtos
{
    // =========================
    // 📦 DTO for Sector Export
    // =========================
    public sealed class SectorReportExportRow
    {
        public int SectorId { get; set; }
        public string SectorName { get; set; } = "";

        // Sector in-charge
        public string? SectorSanyojak { get; set; }
        public string? Mobile { get; set; }
        public string? Cast { get; set; }

        // Villages in this sector
        public List<VillageExpDto> Villages { get; set; } = new();

        // Counts
        public int TotalBooth { get; set; }
        public int TotalVotes { get; set; }
        public int SeniorCitizen { get; set; }
        public int Handicap { get; set; }
        public int DoubleVoter { get; set; }
        public int PravasiVoter { get; set; }
    }

    // =========================
    // 💡 Village DTO
    // =========================
    public class VillageExpDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
    }

    // =========================
    // 📄 Export Definition
    // =========================
    public sealed class SectorReportExportDef : IExportDefinition<SectorReportExportRow>
    {
        public string ReportTitle => "Sector Report";

        public IReadOnlyList<ExportColumn<SectorReportExportRow>> Columns => new List<ExportColumn<SectorReportExportRow>>
        {
           
            new() { Header = "Sector Name",      Value = r => r.SectorName, PdfRelativeWidth = 2.5f, ExcelWidth = 25 },
            new() { Header = "Sector Sanyojak",  Value = r => r.SectorSanyojak ?? "", PdfRelativeWidth = 2f, ExcelWidth = 25 },
            new() { Header = "Mobile",           Value = r => r.Mobile ?? "", PdfRelativeWidth = 1.5f, ExcelWidth = 20 },
            new() { Header = "Cast",             Value = r => r.Cast ?? "", PdfRelativeWidth = 1.5f, ExcelWidth = 20 },
            new() { Header = "Total Booths",     Value = r => r.TotalBooth.ToString(), PdfRelativeWidth = 1f, ExcelWidth = 15 },
            new() { Header = "Total Votes",      Value = r => r.TotalVotes.ToString(), PdfRelativeWidth = 1f, ExcelWidth = 15 },
            new() { Header = "Senior Citizen",   Value = r => r.SeniorCitizen.ToString(), PdfRelativeWidth = 1f, ExcelWidth = 15 },
            new() { Header = "Handicap",         Value = r => r.Handicap.ToString(), PdfRelativeWidth = 1f, ExcelWidth = 15 },
            new() { Header = "Double Voter",     Value = r => r.DoubleVoter.ToString(), PdfRelativeWidth = 1f, ExcelWidth = 15 },
            new() { Header = "Pravasi Voter",    Value = r => r.PravasiVoter.ToString(), PdfRelativeWidth = 1f, ExcelWidth = 15 },
            new() { Header = "Villages",         Value = r => string.Join(", ", r.Villages.Select(v => v.Name)), PdfRelativeWidth = 3f, ExcelWidth = 50 },
        };
    }

    // =========================
    // 💡 Filter for Export
    // =========================
    public class SectorReportFilter : BaseFilter
    {
        public string? UserId { get; set; }
        public int? MandalId { get; set; }
        public int? SectorId { get; set; }
        public int? CastId { get; set; }
        public int? VillageId { get; set; }
    }
}


