using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VidhanSabha.Application.Common.ExportPdfExcel.Dtos
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    namespace VidhanSabha.Application.Common.ExportPdfExcel.Dtos
    {
        public sealed class MandalReportExportRow
        {
            public int MandalId { get; set; }
            public string MandalName { get; set; } = "";
            public int TotalSectors { get; set; }
            public int TotalBooths { get; set; }
            public int TotalVotes { get; set; }
            public int SeniorCitizen { get; set; }
            public int Handicap { get; set; }
            public int DoubleVotes { get; set; }
            public int Pravasi { get; set; }
            public int EffectivePerson { get; set; }
        }

        public sealed class MandalReportExportDef : IExportDefinition<MandalReportExportRow>
        {
            public string ReportTitle => "Mandal Report";

            public IReadOnlyList<ExportColumn<MandalReportExportRow>> Columns => new List<ExportColumn<MandalReportExportRow>>
    {
        new() { Header = "Mandal Id",       Value = r => r.MandalId.ToString(),  PdfRelativeWidth = 1f, ExcelWidth = 10 },
        new() { Header = "Mandal Name",     Value = r => r.MandalName,           PdfRelativeWidth = 2.5f, ExcelWidth = 25 },
        new() { Header = "Total Sectors",   Value = r => r.TotalSectors.ToString(), PdfRelativeWidth = 1f, ExcelWidth = 15 },
        new() { Header = "Total Booths",    Value = r => r.TotalBooths.ToString(), PdfRelativeWidth = 1f, ExcelWidth = 15 },
        new() { Header = "Total Votes",     Value = r => r.TotalVotes.ToString(), PdfRelativeWidth = 1f, ExcelWidth = 15 },
        new() { Header = "Senior Citizen",  Value = r => r.SeniorCitizen.ToString(), PdfRelativeWidth = 1f, ExcelWidth = 15 },
        new() { Header = "Handicap",        Value = r => r.Handicap.ToString(), PdfRelativeWidth = 1f, ExcelWidth = 15 },
        new() { Header = "Double Votes",    Value = r => r.DoubleVotes.ToString(), PdfRelativeWidth = 1f, ExcelWidth = 15 },
        new() { Header = "Pravasi",         Value = r => r.Pravasi.ToString(), PdfRelativeWidth = 1f, ExcelWidth = 15 },
        new() { Header = "Effective Person",Value = r => r.EffectivePerson.ToString(), PdfRelativeWidth = 1f, ExcelWidth = 15 },
    };
        }

        public class MandalReportFilter : BaseFilter
        {
            public int? MandalId { get; set; }
            public int? SectorId { get; set; }
            public int? BoothId { get; set; }
        }
    }

}
