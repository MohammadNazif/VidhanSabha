using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Common.ExportPdfExcel.Dtos;

namespace VidhanSabha.Application.Common.ExportPdfExcel
{
    public interface IExportDefinition<T>
    {
        string ReportTitle { get; }
        IReadOnlyList<ExportColumn<T>> Columns { get; }
    }
}
