using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VidhanSabha.Application.Common.ExportPdfExcel.Dtos
{
    public sealed class SectorExportRow
    {
        public string MandalName { get; set; } = "";
        public string SectorName { get; set; } = "";
        public string Village { get; set; } = "";
        public string SectorSanyojak { get; set; } = "";
        public string ContactNumber { get; set; } = "";
        public string Age { get; set; } = "";
        public string CastName { get; set; } = "";
    }
    public sealed class SectorExportDef : IExportDefinition<SectorExportRow>
    {
        public string ReportTitle => "Booth List";

        public IReadOnlyList<ExportColumn<SectorExportRow>> Columns =>
        [
            new() { Header = "Mandal",        Value = r => r.MandalName,         PdfRelativeWidth = 2f,   ExcelWidth = 20 },
        new() { Header = "Sector Name",       Value = r => r.SectorName,            PdfRelativeWidth = 2.5f, ExcelWidth = 25 },
        new() { Header = "Sector Sanyojak",   Value = r => r.SectorSanyojak,     PdfRelativeWidth = 2f,   ExcelWidth = 20 },
       new() { Header = "Age",                Value = r => r.Age,           PdfRelativeWidth = 1.5f, ExcelWidth = 15 },
        new() { Header = "Contact Number",    Value = r => r.ContactNumber,      PdfRelativeWidth = 1.8f, ExcelWidth = 18 },
        new() { Header = "Cast",              Value = r => r.CastName,           PdfRelativeWidth = 1.5f, ExcelWidth = 15 },
        new() { Header = "Villages",          Value = r => r.SectorName,            PdfRelativeWidth = 2.5f, ExcelWidth = 25 },
    ];
    }

    public class SectorFilter : BaseFilter
    {
        public int? BoothId { get; set; }
        public int? VillageId { get; set; }
        public int? CategoryId { get; set; }
        public int? CastId { get; set; }
    }
}
