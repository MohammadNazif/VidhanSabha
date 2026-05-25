using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VidhanSabha.Application.Common.ExportPdfExcel.Dtos
{
    public sealed class sahmatExportRow
    {

        public int BoothNumber { get; set; }
        public string Village { get; set; } = "";
        public string Name { get; set; } = "";
        //public string FatherName { get; set; } = "";

        public string MobileNumber { get; set; } = "";

        public string Age { get; set; } = "";
        public string Party { get; set; } = "";
        public string Occupation { get; set; } = "";
        public string VoterId { get; set; } = "";

        public string Status { get; set; } = "";

    }
    public sealed class sahmatExportDef : IExportDefinition<sahmatExportRow>
    {
        private string _title;

        public sahmatExportDef(string title)
        {
            _title = title;
        }
        public string ReportTitle => _title;

        public IReadOnlyList<ExportColumn<sahmatExportRow>> Columns =>
        [

        new() { Header = "Booth No.",         Value = r => r.BoothNumber.ToString(), PdfRelativeWidth = 1f, ExcelWidth = 10 },
        new() { Header = "Village",           Value = r => r.Village,            PdfRelativeWidth = 2.5f, ExcelWidth = 25 },
            new() { Header = "Name",            Value = r => r.Name,         PdfRelativeWidth = 2f,   ExcelWidth = 20 },
              new() { Header = "MobileNumber",           Value = r => r.MobileNumber,            PdfRelativeWidth = 2.5f, ExcelWidth = 25 },
             new() { Header = "Age",    Value = r => r.Age,      PdfRelativeWidth = 1.8f, ExcelWidth = 18 },
            new() { Header = "Party",            Value = r => r.Party,         PdfRelativeWidth = 2f,   ExcelWidth = 20 },
        new() { Header = "Occupation",            Value = r => r.Occupation,         PdfRelativeWidth = 2f,   ExcelWidth = 20 },
        new() { Header = "Votter Id",   Value = r => r.VoterId, PdfRelativeWidth = 2.5f, ExcelWidth = 25 },
              new() { Header = "Status",   Value = r => r.Status,     PdfRelativeWidth = 2f,   ExcelWidth = 20 },

    ];
    }

    public class sahmatFilter : BaseFilter
    {
        public int? BoothId { get; set; }
        public int? VillageId { get; set; }
        public int? CategoryId { get; set; }
        public int? CastId { get; set; }

       public string? Type { get; set; }



    }
}
