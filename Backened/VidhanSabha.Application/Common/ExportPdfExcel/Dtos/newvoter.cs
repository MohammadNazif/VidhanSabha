using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VidhanSabha.Application.Common.ExportPdfExcel.Dtos
{
    public sealed class newvoterExportRow
    {

        public int BoothNumber { get; set; }
        public string Village { get; set; } = "";
        public string VoterName { get; set; } = "";
        public string FatherName { get; set; } = "";

        public string MobileNumber { get; set; } = "";
        public string Category { get; set; } = "";
        public string Cast { get; set; } = "";

        public string DOB { get; set; } = "";
        public string Age { get; set; } = "";
        public string VoterId { get; set; } = "";
    

    }
    public sealed class newvoterExportDef : IExportDefinition<newvoterExportRow>
    {
        public string ReportTitle => "New Voter List";

        public IReadOnlyList<ExportColumn<newvoterExportRow>> Columns =>
        [

        new() { Header = "Booth No.",         Value = r => r.BoothNumber.ToString(), PdfRelativeWidth = 1f, ExcelWidth = 10 },
        new() { Header = "Village",           Value = r => r.Village,            PdfRelativeWidth = 2.5f, ExcelWidth = 25 },
            new() { Header = "Voter Name",            Value = r => r.VoterName,         PdfRelativeWidth = 2f,   ExcelWidth = 20 },
        new() { Header = "Father Name",            Value = r => r.FatherName,         PdfRelativeWidth = 2f,   ExcelWidth = 20 },
              new() { Header = "MobileNumber",           Value = r => r.MobileNumber,            PdfRelativeWidth = 2.5f, ExcelWidth = 25 },
            new() { Header = "Category",            Value = r => r.Category,         PdfRelativeWidth = 2f,   ExcelWidth = 20 },
        new() { Header = "Cast",            Value = r => r.Cast,         PdfRelativeWidth = 2f,   ExcelWidth = 20 },
        new() { Header = "DOB",   Value = r => r.DOB,     PdfRelativeWidth = 2f,   ExcelWidth = 20 },
        new() { Header = "Age",    Value = r => r.Age,      PdfRelativeWidth = 1.8f, ExcelWidth = 18 },

        new() { Header = "Votter Id",   Value = r => r.VoterId, PdfRelativeWidth = 2.5f, ExcelWidth = 25 },

    ];
    }

    public class newvoterFilter : BaseFilter
    {
        public int? BoothId { get; set; }
        public int? VillageId { get; set; }
        public int? CategoryId { get; set; }
        public int? CastId { get; set; }
    }
}
