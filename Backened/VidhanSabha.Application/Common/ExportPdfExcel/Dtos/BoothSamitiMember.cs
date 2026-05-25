using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VidhanSabha.Application.Common.ExportPdfExcel.Dtos
{
    public sealed class BoothSamitiMemberExportRow
    {
        public string Name { get; set; } = "";
        public string Age { get; set; } = "";
        public string Contact { get; set; } = "";
        public string Category { get; set; } = "";
        public string Caste { get; set; } = "";
        public string Designation { get; set; } = "";
        public string Occupation { get; set; } = "";
    }

    public sealed class BoothSamitiMemberExportDef : IExportDefinition<BoothSamitiMemberExportRow>
    {
        private readonly string _title;

        public BoothSamitiMemberExportDef(string title)
        {
            _title = title;
        }

        public string ReportTitle => _title;

        public IReadOnlyList<ExportColumn<BoothSamitiMemberExportRow>> Columns =>
        [
        new() { Header = "Name",        Value = r => r.Name,        PdfRelativeWidth = 2f, ExcelWidth = 25 },
        new() { Header = "Age",         Value = r => r.Age,         PdfRelativeWidth = 1f, ExcelWidth = 10 },
        new() { Header = "Contact",     Value = r => r.Contact,     PdfRelativeWidth = 2f, ExcelWidth = 20 },
        new() { Header = "Category",    Value = r => r.Category,    PdfRelativeWidth = 2f, ExcelWidth = 20 },
        new() { Header = "Caste",       Value = r => r.Caste,       PdfRelativeWidth = 2f, ExcelWidth = 20 },
        new() { Header = "Designation", Value = r => r.Designation, PdfRelativeWidth = 2f, ExcelWidth = 20 },
        new() { Header = "Occupation",  Value = r => r.Occupation,  PdfRelativeWidth = 2f, ExcelWidth = 20 }
        ];
    }

    public class BoothSamitiMemberFilter : BaseFilter
    {
        public int? BoothMemId { get; set; }       // Similar to Sahmat’s BoothId
           // To filter by user if needed
        public string? Role { get; set; }          // Optional: "Member" or "Sanyojak"
        public string? Search { get; set; }        // Search by name, contact, etc.
    }
}
