using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VidhanSabha.Application.Common.ExportPdfExcel.Dtos
{
    public sealed class PravasiVoterExportRow
    {
        public string Name { get; set; } = ""; public string VoterId { get; set; } = "";
        public string Mobile { get; set; } = ""; public int BoothNumber { get; set; }
        public string Village { get; set; } = ""; public string CategoryName { get; set; } = "";
        public string CastName { get; set; } = ""; public string Occupation { get; set; } = "";
        public string CurrentAddress { get; set; } = "";
    }
    public sealed class PravasiVoterExportDef : IExportDefinition<PravasiVoterExportRow>
    {
        public string ReportTitle => "Pravasi Voter List";
        public IReadOnlyList<ExportColumn<PravasiVoterExportRow>> Columns =>
        [
        new() { Header = "Name",        Value = r => r.Name,           PdfRelativeWidth = 2f,   ExcelWidth = 20 },
        new() { Header = "Voter ID",    Value = r => r.VoterId,        PdfRelativeWidth = 1.8f, ExcelWidth = 15 },
        new() { Header = "Mobile",      Value = r => r.Mobile,         PdfRelativeWidth = 1.5f, ExcelWidth = 14 },
        new() { Header = "Booth No.",   Value = r => r.BoothNumber.ToString(), PdfRelativeWidth = 1f, ExcelWidth = 10 },
        new() { Header = "Village",     Value = r => r.Village,        PdfRelativeWidth = 1.8f, ExcelWidth = 16 },
        new() { Header = "Category",    Value = r => r.CategoryName,   PdfRelativeWidth = 1.5f, ExcelWidth = 12 },
        new() { Header = "Caste",       Value = r => r.CastName,       PdfRelativeWidth = 1.5f, ExcelWidth = 12 },
        new() { Header = "Occupation",  Value = r => r.Occupation,     PdfRelativeWidth = 1.5f, ExcelWidth = 14 },
        new() { Header = "Address",     Value = r => r.CurrentAddress, PdfRelativeWidth = 2.5f, ExcelWidth = 22 },
    ];
    }

    public class PravasiVoterFilter : BaseFilter
    {
        public int? BoothId { get; set; }
        public int? VillageId { get; set; }
        public int? CategoryId { get; set; }
        public int? CastId { get; set; }
    }
}
