using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using VidhanSabha.Application.Common.ExportPdfExcel.Service;

namespace VidhanSabha.Application.Common.ExportPdfExcel.Query
{
    public sealed class GenericExportHandler<TRow, TFilter>
     : IRequestHandler<GenericExportQuery<TRow, TFilter>, ExportFileResult>
    {
        public async Task<ExportFileResult> Handle(
            GenericExportQuery<TRow, TFilter> request,
            CancellationToken ct)
        {
            // 1. Fetch all rows — no pagination
            var rows = await request.FetchAsync(request.Filter, ct);

            // 2. Build file
            var stamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            var safeName = request.Definition.ReportTitle.Replace(" ", "_");

            return request.Format.ToLowerInvariant() switch
            {
                "pdf" => new ExportFileResult(
                    ExportService.ToPdf(rows, request.Definition),
                    "application/pdf",
                    $"{safeName}_{stamp}.pdf"),

                _ => new ExportFileResult(
                    ExportService.ToExcel(rows, request.Definition),
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    $"{safeName}_{stamp}.xlsx"),
            };
        }
    }
}
