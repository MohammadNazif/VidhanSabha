using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VidhanSabha.Application.Common.ExportPdfExcel.Dtos
{
    public sealed class BDCExportRow
    {
        public string Block { get; set; } = "";
        public string Name { get; set; } = "";
        public string WardNumber { get; set; } = "";
        public string Category { get; set; } = "";
        public string Cast { get; set; } = "";
        public string Party { get; set; } = "";
        public string Mobile { get; set; } = "";
        public string Villages { get; set; } = "";
    }

    public sealed class BDCExportDef : IExportDefinition<BDCExportRow>
    {
        public string ReportTitle => "BDC List";

        public IReadOnlyList<ExportColumn<BDCExportRow>> Columns => new List<ExportColumn<BDCExportRow>>
    {
        new() { Header = "Block", Value = r => r.Block, PdfRelativeWidth = 2f, ExcelWidth = 20 },
        new() { Header = "Name", Value = r => r.Name, PdfRelativeWidth = 2f, ExcelWidth = 20 },
        new() { Header = "Ward Number", Value = r => r.WardNumber, PdfRelativeWidth = 1.5f, ExcelWidth = 15 },
        new() { Header = "Category", Value = r => r.Category, PdfRelativeWidth = 1.5f, ExcelWidth = 15 },
        new() { Header = "Cast", Value = r => r.Cast, PdfRelativeWidth = 1.5f, ExcelWidth = 15 },
        new() { Header = "Party", Value = r => r.Party, PdfRelativeWidth = 1.5f, ExcelWidth = 15 },
        new() { Header = "Mobile", Value = r => r.Mobile, PdfRelativeWidth = 1.5f, ExcelWidth = 15 },
        new() { Header = "Villages", Value = r => r.Villages, PdfRelativeWidth = 2.5f, ExcelWidth = 25 }
    };
        public class BDCExportFilter : BaseFilter
        {
           
            public int? Id { get; set; }        
            public string? UserId { get; set; }
            public int? PartyId { get; set; }
            public int? CategoryId { get; set; }
            public int? CastId { get; set; }
            public string? SearchTerm { get; set; }
        }

    }
}
