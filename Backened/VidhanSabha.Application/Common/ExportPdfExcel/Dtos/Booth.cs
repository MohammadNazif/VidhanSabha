using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VidhanSabha.Application.Common.ExportPdfExcel.Dtos
{
    public sealed class BoothExportRow
    {
        public string MandalName { get; set; } = "";
        public string SectorName { get; set; } = "";
        public int BoothNumber { get; set; }
        public string Village { get; set; } = "";
        public string PollingStationName { get; set; } = "";
        public string BoothAathyaksh { get; set; } = "";
        public string ContactNumber { get; set; } = "";
        public string CastName { get; set; } = "";
    }
    public sealed class BoothExportDef : IExportDefinition<BoothExportRow>
    {
        public string ReportTitle => "Booth List";

        public IReadOnlyList<ExportColumn<BoothExportRow>> Columns =>
        [
            new() { Header = "Mandal",            Value = r => r.MandalName,         PdfRelativeWidth = 2f,   ExcelWidth = 20 },
        new() { Header = "Sector",            Value = r => r.SectorName,         PdfRelativeWidth = 2f,   ExcelWidth = 20 },
        new() { Header = "Booth No.",         Value = r => r.BoothNumber.ToString(), PdfRelativeWidth = 1f, ExcelWidth = 10 },
        new() { Header = "Village",           Value = r => r.Village,            PdfRelativeWidth = 2.5f, ExcelWidth = 25 },
        new() { Header = "Polling Station",   Value = r => r.PollingStationName, PdfRelativeWidth = 2.5f, ExcelWidth = 25 },
        new() { Header = "Booth Aathyaksh",   Value = r => r.BoothAathyaksh,     PdfRelativeWidth = 2f,   ExcelWidth = 20 },
        new() { Header = "Contact Number",    Value = r => r.ContactNumber,      PdfRelativeWidth = 1.8f, ExcelWidth = 18 },
        new() { Header = "Cast",              Value = r => r.CastName,           PdfRelativeWidth = 1.5f, ExcelWidth = 15 },
    ];
    }

    public class BoothFilter : BaseFilter
    {
        public int? BoothId { get; set; }
        public int? VillageId { get; set; }
        public int? CategoryId { get; set; }
        public int? CastId { get; set; }
    }
}
