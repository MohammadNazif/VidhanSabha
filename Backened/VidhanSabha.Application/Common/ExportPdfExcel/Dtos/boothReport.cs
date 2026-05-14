using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VidhanSabha.Application.Common.ExportPdfExcel.Dtos
{
    // =========================
    // 📦 DTO for Booth Export
    // =========================
    public sealed class BoothReportExportRow
    {
        // Booth info
        public int BoothNumber { get; set; }
        public string MandalName { get; set; } = "";
        public string SectorName { get; set; } = "";
        public string PollingStation { get; set; } = "";

        // Booth in-charge (Sanyojak)
        public string? BoothAdhyaksh { get; set; }
        public string? Mobile { get; set; }
        public string? Cast { get; set; }

        // Villages under this booth
        public List<VillageExpResponseDto> Villages { get; set; } = new();

        // Counts
        public int TotalVotes { get; set; }
        public int SeniorCitizen { get; set; }
        public int Handicap { get; set; }
        public int DoubleVotes { get; set; }
        public int Pravasi { get; set; }

    }

    // =========================
    // 📄 Export Definition
    // =========================
    public sealed class BoothReportExportDef : IExportDefinition<BoothReportExportRow>
    {
        public string ReportTitle => "Booth Report";

        public IReadOnlyList<ExportColumn<BoothReportExportRow>> Columns => new List<ExportColumn<BoothReportExportRow>>
        {
            new() { Header = "Booth Number",      Value = r => r.BoothNumber.ToString(), PdfRelativeWidth = 1f, ExcelWidth = 10 },
            //new() { Header = "Mandal Name",       Value = r => r.MandalName, PdfRelativeWidth = 2f, ExcelWidth = 25 },
            //new() { Header = "Sector Name",       Value = r => r.SectorName, PdfRelativeWidth = 2f, ExcelWidth = 25 },
            new() { Header = "Polling Station",   Value = r => r.PollingStation, PdfRelativeWidth = 2f, ExcelWidth = 25 },
            new() { Header = "Booth Adhyaksh",    Value = r => r.BoothAdhyaksh ?? "", PdfRelativeWidth = 1.5f, ExcelWidth = 20 },
            new() { Header = "Mobile",            Value = r => r.Mobile ?? "", PdfRelativeWidth = 1.5f, ExcelWidth = 20 },
            new() { Header = "Cast",              Value = r => r.Cast ?? "", PdfRelativeWidth = 1.5f, ExcelWidth = 20 },
            new() { Header = "Total Votes",       Value = r => r.TotalVotes.ToString(), PdfRelativeWidth = 1f, ExcelWidth = 15 },
            new() { Header = "Senior Citizen",    Value = r => r.SeniorCitizen.ToString(), PdfRelativeWidth = 1f, ExcelWidth = 15 },
            new() { Header = "Handicap",          Value = r => r.Handicap.ToString(), PdfRelativeWidth = 1f, ExcelWidth = 15 },
            new() { Header = "Double Votes",      Value = r => r.DoubleVotes.ToString(), PdfRelativeWidth = 1f, ExcelWidth = 15 },
            new() { Header = "Pravasi",           Value = r => r.Pravasi.ToString(), PdfRelativeWidth = 1f, ExcelWidth = 15 },
            new() { Header = "Villages",          Value = r => string.Join(", ", r.Villages.Select(v => v.VillageName)), PdfRelativeWidth = 3f, ExcelWidth = 50 },
        };
    }

    // =========================
    // 💡 Filter for Export
    // =========================
    public class BoothReportFilter : BaseFilter
    {
        public string? UserId { get; set; }
        public int? MandalId { get; set; }
        public int? SectorId { get; set; }
        public int? BoothId { get; set; }
    }

    // =========================
    // 💡 Village DTO
    // =========================
    public class VillageExpResponseDto
    {
        public int VillageId { get; set; }
        public string? VillageName { get; set; }
        public bool HasAnshik { get; set; }
    }
}