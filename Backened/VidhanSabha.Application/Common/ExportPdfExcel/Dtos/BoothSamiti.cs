using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VidhanSabha.Application.Common.ExportPdfExcel.Dtos
{
    public sealed class BoothSamitiExportRow
    {
        public int BoothNo { get; set; }
        public string PollingStation { get; set; } = "";
        public string BoothAdhyaksh { get; set; } = "";
        public string CastName { get; set; } = "";
        public string CategoryName { get; set; } = "";
        public string Age { get; set; } = "";
        public string Contact { get; set; } = "";
        public int TotalMember { get; set; }
        public string Village { get; set; } = "";
        public string Designation { get; set; } = "Adhyakshya"; // default
    }

    public sealed class BoothSamitiExportDef : IExportDefinition<BoothSamitiExportRow>
    {
        private readonly string _title;

        public BoothSamitiExportDef(string title)
        {
            _title = title;
        }

        public string ReportTitle => _title;

        public IReadOnlyList<ExportColumn<BoothSamitiExportRow>> Columns =>
        [
            new() { Header = "Booth No.",       Value = r => r.BoothNo.ToString(), PdfRelativeWidth = 1f, ExcelWidth = 10 },
        new() { Header = "Polling Station", Value = r => r.PollingStation,     PdfRelativeWidth = 2f, ExcelWidth = 25 },
        new() { Header = "Booth Adhyaksh",  Value = r => r.BoothAdhyaksh,      PdfRelativeWidth = 2f, ExcelWidth = 25 },
        new() { Header = "Cast Name",       Value = r => r.CastName,           PdfRelativeWidth = 2f, ExcelWidth = 20 },
        new() { Header = "Category Name",   Value = r => r.CategoryName,       PdfRelativeWidth = 2f, ExcelWidth = 20 },
        new() { Header = "Age",             Value = r => r.Age,                PdfRelativeWidth = 1f, ExcelWidth = 10 },
        new() { Header = "Contact",         Value = r => r.Contact,            PdfRelativeWidth = 2f, ExcelWidth = 20 },
        new() { Header = "Total Member",    Value = r => r.TotalMember.ToString(), PdfRelativeWidth = 1f, ExcelWidth = 10 },
        new() { Header = "Village",         Value = r => r.Village,            PdfRelativeWidth = 2f, ExcelWidth = 25 },
        new() { Header = "Designation",     Value = r => r.Designation,        PdfRelativeWidth = 1f, ExcelWidth = 15 }
        ];
    }

    public class BoothSamitiFilter : BaseFilter
    {
        public int? Id { get; set; }
        public int? BoothId { get; set; }
        public string UserId { get; set; }
        public string? Search { get; set; }
    }
}
