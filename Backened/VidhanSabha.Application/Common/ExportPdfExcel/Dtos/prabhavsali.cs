using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VidhanSabha.Application.Common.ExportPdfExcel.Dtos
{
    public sealed class prabhavsaliExportRow
    {

        public int BoothNumber { get; set; }
        public string Village { get; set; } = "";
        public string Name { get; set; } = "";
        //public string FatherName { get; set; } = "";
        public string Designation { get; set; } = "";
        public string MobileNumber { get; set; } = "";
        public string Category { get; set; } = "";
        public string Cast { get; set; } = "";
        public string Description { get; set; } = "";

    }
    public sealed class prabhavsaliExportDef : IExportDefinition<prabhavsaliExportRow>
    {
        public string ReportTitle => "Prabhavsali List";

        public IReadOnlyList<ExportColumn<prabhavsaliExportRow>> Columns =>
        [

        new() { Header = "Booth No.",         Value = r => r.BoothNumber.ToString(), PdfRelativeWidth = 1f, ExcelWidth = 10 },
        new() { Header = "Village",           Value = r => r.Village,            PdfRelativeWidth = 2.5f, ExcelWidth = 25 },
            new() { Header = "Name",            Value = r => r.Name,         PdfRelativeWidth = 2f,   ExcelWidth = 20 },
               new() { Header = "Designation",    Value = r => r.Designation,      PdfRelativeWidth = 1.8f, ExcelWidth = 18 },
              new() { Header = "MobileNumber",           Value = r => r.MobileNumber,            PdfRelativeWidth = 2.5f, ExcelWidth = 25 },
          
            new() { Header = "Category",            Value = r => r.Category,         PdfRelativeWidth = 2f,   ExcelWidth = 20 },
        new() { Header = "Cast",            Value = r => r.Cast,         PdfRelativeWidth = 2f,   ExcelWidth = 20 },
        new() { Header = "Description",   Value = r => r.Description, PdfRelativeWidth = 2.5f, ExcelWidth = 25 },

    ];
    }

    public class prabhavsaliFilter : BaseFilter
    {
        public int? BoothId { get; set; }
        public int? VillageId { get; set; }
        public int? CategoryId { get; set; }
        public int? CastId { get; set; }

        public int? designationId { get; set; }



    }
}
