using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VidhanSabha.Application.Common.ExportPdfExcel.Dtos
{
    public sealed class seniordisabledExportRow
    {

        public int BoothNumber { get; set; }
        public string Village { get; set; } = "";
        public string Name { get; set; } = "";
        //public string FatherName { get; set; } = "";
        //public string Designation { get; set; } = "";
        public string MobileNumber { get; set; } = "";
        public string Category { get; set; } = "";
        //public string Cast { get; set; } = "";
        public string VoterId { get; set; }

    }
    public sealed class seniordisabledExportDef : IExportDefinition<seniordisabledExportRow>
    {
        public string ReportTitle => "Senior_Disabled List";

        public IReadOnlyList<ExportColumn<seniordisabledExportRow>> Columns =>
        [

        new() { Header = "Booth No.",         Value = r => r.BoothNumber.ToString(), PdfRelativeWidth = 1f, ExcelWidth = 10 },
        new() { Header = "Village",           Value = r => r.Village,            PdfRelativeWidth = 2.5f, ExcelWidth = 25 },
            new() { Header = "Name",            Value = r => r.Name,         PdfRelativeWidth = 2f,   ExcelWidth = 20 },
              
              new() { Header = "MobileNumber",           Value = r => r.MobileNumber,            PdfRelativeWidth = 2.5f, ExcelWidth = 25 },

            new() { Header = "Category",            Value = r => r.Category,         PdfRelativeWidth = 2f,   ExcelWidth = 20 },
        new() { Header = "Voter Id",            Value = r => r.VoterId,         PdfRelativeWidth = 2f,   ExcelWidth = 20 },
       

    ];
    }

    public class seniordisabledFilter : BaseFilter
    {
        public int? BoothId { get; set; }
        public int? VillageId { get; set; }
        public int? CategoryId { get; set; }
        public int? CastId { get; set; }

        public int? TypeId { get; set; }



    }
}
