using System;
using System.Collections.Generic;

namespace VidhanSabha.Application.Common.ExportPdfExcel.Dtos
{
    // =========================
    // 📦 DTO for Mandal Export
    // =========================
    public sealed class MandalExportRow
    {
        public int Id { get; set; }
       
        public string Name { get; set; } = "";

        // Sanyojak Info

        public string? MandalSanyojak { get; set; }

        public string? CastName { get; set; }

        public string? Contact { get; set; }
        public string? FatherName { get; set; }

    }

    // =========================
    // 📄 Export Definition
    // =========================
    public sealed class MandalExportDef : IExportDefinition<MandalExportRow>
    {
        public string ReportTitle => "Mandal Report";

        public IReadOnlyList<ExportColumn<MandalExportRow>> Columns => new List<ExportColumn<MandalExportRow>>
        {
          
            new() { Header = "Mandal Name",     Value = r => r.Name, PdfRelativeWidth = 2f, ExcelWidth = 25 },
            new() { Header = "Sanyojak Name",   Value = r => r.MandalSanyojak ?? "", PdfRelativeWidth = 2f, ExcelWidth = 25 },
            new() { Header = "Father Name",     Value = r => r.FatherName ?? "", PdfRelativeWidth = 2f, ExcelWidth = 25 },
            new() { Header = "Cast Name",       Value = r => r.CastName ?? "", PdfRelativeWidth = 2f, ExcelWidth = 25 },
            new() { Header = "Contact",         Value = r => r.Contact ?? "", PdfRelativeWidth = 2f, ExcelWidth = 20 },
          
    
        };
    }

    // =========================
    // 💡 Filter for Export
    // =========================
    public class MandalFilter : BaseFilter
    {
        public string? UserId { get; set; }
        public int? Id { get; set; }
        public int? VidhanId { get; set; }
        public string? SearchTerm { get; set; }
    }
}