using System.Drawing;
using System.Reflection.Metadata;
using ClosedXML.Excel;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace VidhanSabha.Application.Common.ExportPdfExcel.Service;

public static class ExportService
{
    // ─────────────────────────────────────────────────────────────────────────
    //  EXCEL
    // ─────────────────────────────────────────────────────────────────────────
    public static byte[] ToExcel<T>(
        IReadOnlyList<T> rows,
        IExportDefinition<T> def)
    {
        using var wb = new XLWorkbook();
        var ws = wb.AddWorksheet(def.ReportTitle.Truncate(31)); // Excel sheet name limit

        // ── Header ───────────────────────────────────────────────────────────
        for (int c = 0; c < def.Columns.Count; c++)
        {
            var cell = ws.Cell(1, c + 1);
            cell.Value = def.Columns[c].Header;
            cell.Style.Font.Bold = true;
            cell.Style.Font.FontColor = XLColor.White;
            cell.Style.Fill.BackgroundColor = XLColor.FromArgb(31, 78, 121);
            cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            cell.Style.Border.BottomBorder = XLBorderStyleValues.Medium;
        }

        // ── Data rows ─────────────────────────────────────────────────────────
        for (int r = 0; r < rows.Count; r++)
        {
            for (int c = 0; c < def.Columns.Count; c++)
                ws.Cell(r + 2, c + 1).Value = def.Columns[c].Value(rows[r]);

            if (r % 2 == 1)
                ws.Row(r + 2).Style.Fill.BackgroundColor = XLColor.FromArgb(242, 242, 242);
        }

        // ── Polish ────────────────────────────────────────────────────────────
        for (int c = 0; c < def.Columns.Count; c++)
        {
            var col = ws.Column(c + 1);
            col.Width = def.Columns[c].ExcelWidth;
        }

        ws.RangeUsed()!.SetAutoFilter();
        ws.SheetView.FreezeRows(1);

        // ── Summary row ───────────────────────────────────────────────────────
        int lastRow = rows.Count + 2;
        ws.Cell(lastRow, 1).Value = $"Total: {rows.Count} records";
        ws.Cell(lastRow, 1).Style.Font.Bold = true;
        ws.Cell(lastRow, 1).Style.Fill.BackgroundColor = XLColor.FromArgb(31, 78, 121);
        ws.Cell(lastRow, 1).Style.Font.FontColor = XLColor.White;
        ws.Range(lastRow, 1, lastRow, def.Columns.Count).Merge();

        using var ms = new MemoryStream();
        wb.SaveAs(ms);
        return ms.ToArray();
    }

    // ─────────────────────────────────────────────────────────────────────────
    //  PDF
    // ─────────────────────────────────────────────────────────────────────────
    public static byte[] ToPdf<T>(
        IReadOnlyList<T> rows,
        IExportDefinition<T> def)
    {
        QuestPDF.Settings.License = LicenseType.Community;

        return QuestPDF.Fluent.Document.Create(c => c.Page(page =>
        {
            page.Size(PageSizes.A4.Landscape());
            page.Margin(1, Unit.Centimetre);
            page.DefaultTextStyle(t => t.FontSize(8));

            // ── Header ────────────────────────────────────────────────────────
            page.Header().Column(col =>
            {
                col.Item().Text(def.ReportTitle)
                    .Bold().FontSize(14).AlignCenter();
                col.Item().Text($"Generated: {DateTime.Now:dd-MM-yyyy HH:mm}  |  Total: {rows.Count} records")
                    .FontSize(8).FontColor(Colors.Grey.Darken1).AlignCenter();
                col.Item().PaddingBottom(6);
            });

            // ── Table ─────────────────────────────────────────────────────────
            page.Content().Table(table =>
            {

                table.ColumnsDefinition(colDef =>
                {
                    colDef.ConstantColumn(25);
                    foreach (var col in def.Columns)
                        colDef.RelativeColumn(col.PdfRelativeWidth);
                });

                // Header row
                table.Header(header =>
                {
                    header.Cell()
                   .Background(Colors.Blue.Darken3)
                    .Padding(5)
                    .Text("Sr.")
                  .Bold()
                  .FontColor(Colors.White)
                 .FontSize(9)
                  .AlignCenter();
                    foreach (var col in def.Columns)
                    {
                        header.Cell()
                            .Background(Colors.Blue.Darken3)
                            .Padding(4)
                            .Text(col.Header)
                            .Bold().FontColor(Colors.White).FontSize(8);
                    }
                });

                // Data rows
                for (int i = 0; i < rows.Count; i++)
                {
                    var row = rows[i];
                    var bg = i % 2 == 0 ? Colors.White : Colors.Grey.Lighten4;
                    table.Cell()
                    .Background(bg)
                    .BorderBottom(0.5f, Unit.Point)
                    .BorderColor(Colors.Grey.Lighten2)
                    .Padding(3)
                    .Text((i + 1).ToString())
                    .FontSize(7.5f)
                    .AlignCenter();
                    foreach (var col in def.Columns)
                    {
                        table.Cell()
                            .Background(bg)
                            .BorderBottom(0.5f, Unit.Point)
                            .BorderColor(Colors.Grey.Lighten2)
                            .Padding(3)
                            .Text(col.Value(row))
                            .FontSize(7.5f);
                    }
                }
            });

            // ── Footer ────────────────────────────────────────────────────────
            page.Footer().Row(row =>
            {
                row.RelativeItem()
                    .Text($"Total Records: {rows.Count}")
                    .FontSize(8);
                row.RelativeItem()
                    .AlignRight()
                    .Text(t =>
                    {
                        t.Span("Page ").FontSize(8);
                        t.CurrentPageNumber().FontSize(8);
                        t.Span(" of ").FontSize(8);
                        t.TotalPages().FontSize(8);
                    });
            });
        })).GeneratePdf();
    }
}

// ── String helper ─────────────────────────────────────────────────────────────
file static class StringExtensions
{
    public static string Truncate(this string s, int max) =>
        s.Length <= max ? s : s[..max];
}