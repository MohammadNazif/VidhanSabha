using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VidhanSabha.Application.Common.ExportPdfExcel.Dtos
{
    public sealed class BlockExportRow
    {
        public string BlockName { get; set; } = "";
        public string BlockPramukh { get; set; } = "";
        public string Party { get; set; } = "";
        public string Mobile { get; set; } = "";
        public string Address { get; set; } = "";
        public string Category { get; set; } = "";
        public string Cast { get; set; } = "";
        public string Occupation { get; set; } = "";
      
    }

    public sealed class BlockExportDef : IExportDefinition<BlockExportRow>
    {
        public string ReportTitle => "Block List";

        public IReadOnlyList<ExportColumn<BlockExportRow>> Columns => new List<ExportColumn<BlockExportRow>>
    {
        new() { Header = "Block Name", Value = r => r.BlockName, PdfRelativeWidth = 2f, ExcelWidth = 20 },
        new() { Header = "Block Pramukh", Value = r => r.BlockPramukh, PdfRelativeWidth = 2f, ExcelWidth = 20 },
        new() { Header = "Party", Value = r => r.Party, PdfRelativeWidth = 1.5f, ExcelWidth = 15 },
        new() { Header = "Mobile", Value = r => r.Mobile, PdfRelativeWidth = 1.5f, ExcelWidth = 15 },
        new() { Header = "Address", Value = r => r.Address, PdfRelativeWidth = 2f, ExcelWidth = 25 },
        new() { Header = "Category", Value = r => r.Category, PdfRelativeWidth = 1.5f, ExcelWidth = 15 },
        new() { Header = "Cast", Value = r => r.Cast, PdfRelativeWidth = 1.5f, ExcelWidth = 15 },
        new() { Header = "Occupation", Value = r => r.Occupation, PdfRelativeWidth = 1.5f, ExcelWidth = 15 },
       
    };

        public class BlockExportFilter : BaseFilter
        {
           
            public int? Id { get; set; }

            public string? UserId { get; set; }

            public int? CastId { get; set; }

            public int? CategoryId { get; set; }

            public int? PartyId { get; set; }

            public string? SearchTerm { get; set; }
        }

    }
}
