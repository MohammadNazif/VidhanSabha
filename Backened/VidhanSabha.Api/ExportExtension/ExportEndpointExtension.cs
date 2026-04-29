using System.Security.Claims;
using MediatR;
using VidhanSabha.Application.Common.ExportPdfExcel;
using VidhanSabha.Application.Common.ExportPdfExcel.Query;

namespace VidhanSabha.Api.ExportExtension
{
    public static class ExportEndpointExtensions
    {
        /// <summary>
        /// Call once per list. Adds GET /export/excel and GET /export/pdf.
        /// </summary>
        /// <typeparam name="TRow">   DTO returned by the query             </typeparam>
        /// <typeparam name="TFilter">Query-string filter class             </typeparam>
        /// <param name="group">      The RouteGroupBuilder for this module  </param>
        /// <param name="definition"> Column layout for this list           </param>
        /// <param name="fetchAsync"> Async delegate that returns all rows  </param>
        /// <param name="injectUserId">
        ///     If true, sets filter.UserId from JWT before calling fetchAsync
        /// </param>
        public static RouteGroupBuilder AddExportEndpoints<TRow, TFilter>(
            this RouteGroupBuilder group,
            IExportDefinition<TRow> definition,
            Func<TFilter, CancellationToken, Task<List<TRow>>> fetchAsync,
            bool injectUserId = true)
            where TFilter : class, new()
        {
            group.MapGet("/export/excel", ExportHandler("excel"));
            group.MapGet("/export/pdf", ExportHandler("pdf"));

            return group;

            // local function keeps the two endpoints DRY
            Delegate ExportHandler(string format) =>
                async (
                    [AsParameters] TFilter filter,
                    IMediator mediator,
                    HttpContext ctx,
                    CancellationToken ct) =>
                {
                    // Inject userId from JWT claim into filter if it has the property
                    if (injectUserId)
                    {
                        var userId = ctx.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                        var prop = typeof(TFilter).GetProperty("UserId");
                        prop?.SetValue(filter, userId);
                    }

                    var result = await mediator.Send(
                        new GenericExportQuery<TRow, TFilter>(filter, format, definition, fetchAsync),
                        ct);

                    return Results.File(result.Data, result.ContentType, result.FileName);
                };
        }
    }
}
