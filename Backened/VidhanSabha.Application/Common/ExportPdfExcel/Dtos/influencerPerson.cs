using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VidhanSabha.Application.Common.ExportPdfExcel.Dtos
{
    public sealed class InfluencerExportRow
    {
        public int BoothNumber { get; set; }
        public string Name { get; set; } = "";
        public string Mobile { get; set; } = "";
        public string Cast { get; set; } = "";
        public string Category { get; set; } = "";
        public string Villages { get; set; } = "";
        public string Description { get; set; } = "";
    }

    public sealed class InfluencerExportDef : IExportDefinition<InfluencerExportRow>
    {
        public string ReportTitle => "Influencer List";

        public IReadOnlyList<ExportColumn<InfluencerExportRow>> Columns => new List<ExportColumn<InfluencerExportRow>>
    {
        new() { Header = "Booth Number", Value = r => r.BoothNumber.ToString(), PdfRelativeWidth = 1f, ExcelWidth = 10 },
        new() { Header = "Name", Value = r => r.Name, PdfRelativeWidth = 2f, ExcelWidth = 20 },
        new() { Header = "Mobile", Value = r => r.Mobile, PdfRelativeWidth = 1.5f, ExcelWidth = 15 },
        new() { Header = "Cast", Value = r => r.Cast, PdfRelativeWidth = 1.5f, ExcelWidth = 15 },
        new() { Header = "Category", Value = r => r.Category, PdfRelativeWidth = 1.5f, ExcelWidth = 15 },
        new() { Header = "Villages", Value = r => r.Villages, PdfRelativeWidth = 2.5f, ExcelWidth = 25 },
        new() { Header = "Description", Value = r => r.Description, PdfRelativeWidth = 2f, ExcelWidth = 20 }
    };

        public class InfluencerExportFilter : BaseFilter
        {

            public int? Id { get; set; }
            public string? UserId { get; set; }
            public int? BoothId { get; set; }
            public int? CastId { get; set; }
            public int? CategoryId { get; set; }
            public string? SearchTerm { get; set; }
        }
    }
}
