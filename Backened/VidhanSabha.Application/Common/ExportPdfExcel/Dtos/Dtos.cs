using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VidhanSabha.Application.Common.ExportPdfExcel.Dtos
{
    public sealed class ExportColumn<T>
    {
        public string Header { get; init; } = "";
        public Func<T, string> Value { get; init; } = _ => "";
        public float PdfRelativeWidth { get; init; } = 1f;   
        public float ExcelWidth { get; init; } = 15f;  
    }
}
