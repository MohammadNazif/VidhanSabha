using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VidhanSabha.Application.Common.ExportPdfExcel.Dtos
{
    public sealed class doublevoterExportRow
    {
       
        public int BoothNumber { get; set; }
        public string Village { get; set; } = "";
        public string VoterName { get; set; } = "";
        public string FatherName { get; set; } = "";

        public string VoterId { get; set; } = "";
        public string CurrentAddress { get; set; } = "";
        public string Description { get; set; } = "";
    
    }
    public sealed class doublevoterExportDef : IExportDefinition<doublevoterExportRow>
    {
        public string ReportTitle => "Double Voter List";

        public IReadOnlyList<ExportColumn<doublevoterExportRow>> Columns =>
        [
            
        new() { Header = "Booth No.",         Value = r => r.BoothNumber.ToString(), PdfRelativeWidth = 1f, ExcelWidth = 10 },
        new() { Header = "Village",           Value = r => r.Village,            PdfRelativeWidth = 2.5f, ExcelWidth = 25 },
            new() { Header = "Voter Name",            Value = r => r.VoterName,         PdfRelativeWidth = 2f,   ExcelWidth = 20 },
        new() { Header = "Father Name",            Value = r => r.FatherName,         PdfRelativeWidth = 2f,   ExcelWidth = 20 },
        new() { Header = "Votter Id",   Value = r => r.VoterId, PdfRelativeWidth = 2.5f, ExcelWidth = 25 },
        new() { Header = "Current Address",   Value = r => r.CurrentAddress,     PdfRelativeWidth = 2f,   ExcelWidth = 20 },
        new() { Header = "Description",    Value = r => r.Description,      PdfRelativeWidth = 1.8f, ExcelWidth = 18 },
    ];
    }

    public class doublevoterFilter : BaseFilter
    {
        public int? BoothId { get; set; }
        public int? VillageId { get; set; }
        public int? CategoryId { get; set; }
        public int? CastId { get; set; }
    }
}
