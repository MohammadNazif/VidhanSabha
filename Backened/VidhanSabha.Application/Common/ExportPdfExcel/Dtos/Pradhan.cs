namespace VidhanSabha.Application.Common.ExportPdfExcel.Dtos
{
    public sealed class PradhanExportRow
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Designation { get; set; } = "";
        public string Gender { get; set; } = "";
        public string Villages { get; set; } = "";
        public string Contact { get; set; } = "";
    }

    public sealed class PradhanExportDef : IExportDefinition<PradhanExportRow>
    {
        public string ReportTitle => "Pradhan List";

        public IReadOnlyList<ExportColumn<PradhanExportRow>> Columns => new List<ExportColumn<PradhanExportRow>>
        {
           
            new() { Header = "Name",         Value = r => r.Name,        PdfRelativeWidth = 2f, ExcelWidth = 20 },
            new() { Header = "Designation",  Value = r => r.Designation, PdfRelativeWidth = 2f, ExcelWidth = 20 },
            new() { Header = "Contact",      Value = r => r.Contact,     PdfRelativeWidth = 1.5f, ExcelWidth = 15 },
            new() { Header = "Gender",       Value = r => r.Gender,      PdfRelativeWidth = 1f, ExcelWidth = 10 },
            new() { Header = "Villages",     Value = r => r.Villages,    PdfRelativeWidth = 2.5f, ExcelWidth = 25 },
          
        };
    }

    public class PradhanExportFilter : BaseFilter
    {
        public int? Id { get; set; }
        public string? UserId { get; set; }
        public int? BoothId { get; set; }
        public int? SectorId { get; set; }
        public int? MandalId { get; set; }
        public string? SearchTerm { get; set; }
    }
}