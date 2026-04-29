using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace VidhanSabha.Application.Common.ExportPdfExcel.Query
{
    public sealed record GenericExportQuery<TRow, TFilter>(
      TFilter Filter,
      string Format,
      IExportDefinition<TRow> Definition,
      Func<TFilter, CancellationToken, Task<List<TRow>>> FetchAsync
  ) : IRequest<ExportFileResult>;

    public sealed record ExportFileResult(byte[] Data, string ContentType, string FileName);
}
