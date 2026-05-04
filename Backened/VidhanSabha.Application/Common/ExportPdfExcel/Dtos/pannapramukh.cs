using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VidhanSabha.Application.Common.ExportPdfExcel.Dtos
{
    public sealed class PannaPramukhExportRow
    {

        public int BoothNumber { get; set; }
        public string Village { get; set; } = "";
        public string PannaPramukh { get; set; } = "";
        //public string FatherName { get; set; } = "";
        //public string Designation { get; set; } = "";
        public string PannaNo { get; set; } = "";
        //public string Category { get; set; } = "";
        public string Cast { get; set; } = "";
        public string VoterId { get; set; } = "";
        public string Address { get; set; } = "";
        public string MobileNumber { get; set; } = "";

    }
    public sealed class PannaPramukhExportDef : IExportDefinition<PannaPramukhExportRow>
    {
        public string ReportTitle => "PannaPramukh List";

        public IReadOnlyList<ExportColumn<PannaPramukhExportRow>> Columns =>
        [

        new() { Header = "Booth No.",         Value = r => r.BoothNumber.ToString(), PdfRelativeWidth = 1f, ExcelWidth = 10 },
        new() { Header = "Village",           Value = r => r.Village,            PdfRelativeWidth = 2.5f, ExcelWidth = 25 },
            new() { Header = "Panna Pramukh",            Value = r => r.PannaPramukh,         PdfRelativeWidth = 2f,   ExcelWidth = 20 },
              new() { Header = "PannaNumber",           Value = r => r.PannaNo,            PdfRelativeWidth = 2.5f, ExcelWidth = 25 },
            
            new() { Header = "Cast",            Value = r => r.Cast,         PdfRelativeWidth = 2f,   ExcelWidth = 20 },
        new() { Header = "Voter Id",            Value = r => r.VoterId,         PdfRelativeWidth = 2f,   ExcelWidth = 20 },

              new() { Header = "Address",           Value = r => r.Address,            PdfRelativeWidth = 2.5f, ExcelWidth = 25 },  new() { Header = "MobileNumber",           Value = r => r.MobileNumber,            PdfRelativeWidth = 2.5f, ExcelWidth = 25 },

    ];
    }

    public class PannaPramukhFilter : BaseFilter
    {
        public int? BoothId { get; set; }
        public int? VillageId { get; set; }
        public int? CategoryId { get; set; }
        public int? CastId { get; set; }



    }
}
