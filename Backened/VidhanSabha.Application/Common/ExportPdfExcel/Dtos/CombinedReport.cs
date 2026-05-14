using System;
using System.Collections.Generic;
using global::VidhanSabha.Application.Pannels.Admin.Mandal.DTOs;
using VidhanSabha.Application.Common.multiplefilterparse;

namespace VidhanSabha.Application.Common.ExportPdfExcel.Dtos
{
    // =========================
    // 📦 Full Mandal-Sector-Booth Export DTO
    // =========================
    public sealed class CombinedReportExportRow
    {
        public string MandalName { get; set; } = "";
        public int MandalId { get; set; }
        
        // Sector
        public string SectorName { get; set; } = "";
        public string? SectorPhone { get; set; }
        public string? SectorInchargeName { get; set; }
        public string? SectorFatherName { get; set; }

        // Booth
        public int? BoothNumber { get; set; }
        public string? PollingStationName { get; set; }

        // Sanyojak
        public string? SanyojakName { get; set; }
        public string? SanyojakPhone { get; set; }
        public string? SanyojakFatherName { get; set; }
        public int? SanyojakAge { get; set; }
        public string? SanyojakCaste { get; set; }
        public string? SanyojakAddress { get; set; }
        public string? SanyojakEducation { get; set; }
        public string? SanyojakProfile { get; set; }

        // Villages (comma-separated)
        public string? VillageNames { get; set; }
    }

    // =========================
    // 📄 Export Definition
    // =========================
    public sealed class CombinedReportExportDef : IExportDefinition<CombinedReportExportRow>
    {
        public string ReportTitle => "Combined Mandal Report";

        public IReadOnlyList<ExportColumn<CombinedReportExportRow>> Columns => new List<ExportColumn<CombinedReportExportRow>>
        {
            new() { Header = "Mandal Name", Value = r => r.MandalName, PdfRelativeWidth = 2f, ExcelWidth = 25 },
            new() { Header = "Sector Name", Value = r => r.SectorName, PdfRelativeWidth = 2f, ExcelWidth = 25 },
            new() { Header = "Sector Phone", Value = r => r.SectorPhone ?? "", PdfRelativeWidth = 1.5f, ExcelWidth = 15 },
            new() { Header = "Sector Incharge", Value = r => r.SectorInchargeName ?? "", PdfRelativeWidth = 2f, ExcelWidth = 20 },
            new() { Header = "Sector Father Name", Value = r => r.SectorFatherName ?? "", PdfRelativeWidth = 2f, ExcelWidth = 20 },
            new() { Header = "Booth Number", Value = r => r.BoothNumber?.ToString() ?? "", PdfRelativeWidth = 1f, ExcelWidth = 10 },
            new() { Header = "Polling Station", Value = r => r.PollingStationName ?? "", PdfRelativeWidth = 2f, ExcelWidth = 25 },
            new() { Header = "Sanyojak Name", Value = r => r.SanyojakName ?? "", PdfRelativeWidth = 2f, ExcelWidth = 25 },
            new() { Header = "Sanyojak Phone", Value = r => r.SanyojakPhone ?? "", PdfRelativeWidth = 2f, ExcelWidth = 15 },
            new() { Header = "Sanyojak Father", Value = r => r.SanyojakFatherName ?? "", PdfRelativeWidth = 2f, ExcelWidth = 20 },
            new() { Header = "Sanyojak Age", Value = r => r.SanyojakAge?.ToString() ?? "", PdfRelativeWidth = 1f, ExcelWidth = 10 },
            new() { Header = "Sanyojak Caste", Value = r => r.SanyojakCaste ?? "", PdfRelativeWidth = 2f, ExcelWidth = 20 },
            new() { Header = "Sanyojak Address", Value = r => r.SanyojakAddress ?? "", PdfRelativeWidth = 3f, ExcelWidth = 40 },
            new() { Header = "Sanyojak Education", Value = r => r.SanyojakEducation ?? "", PdfRelativeWidth = 2f, ExcelWidth = 25 },
            new() { Header = "Villages", Value = r => r.VillageNames ?? "", PdfRelativeWidth = 3f, ExcelWidth = 40 },
        };
    }

    public class CombinedReportFilter  
    {
        public string? UserId { get; set; } = "";
        public int? Id { get; set; }
        public string? SectorIds { get; set; } = "";
        public string? MandalIds { get; set; } = "";
        public string? CastIds { get; set; } = "";
        public string? VillageIds { get; set; } = "";
        public string? SearchTerm { get; set; } = "";

        public List<int> GetVillageIds() => FilterParse.ParseIds(VillageIds);
        public List<int> GetMandalIds() => FilterParse.ParseIds(MandalIds);
        public List<int> GetCastIds() => FilterParse.ParseIds(CastIds);
        public List<int> GetSectorIds() => FilterParse.ParseIds(SectorIds);
    }
}
