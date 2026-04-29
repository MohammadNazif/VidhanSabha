// ExportHandlerRegistration.cs
using System.Reflection;
using MediatR;
using VidhanSabha.Application.Common.ExportPdfExcel;
using VidhanSabha.Application.Common.ExportPdfExcel.Dtos;
using VidhanSabha.Application.Common.ExportPdfExcel.Query;

public static class ExportHandlerRegistration
{
    /// <summary>
    /// Scans the given assemblies for all IExportDefinition<T> implementations
    /// and registers their GenericExportHandler automatically.
    /// </summary>
    public static IServiceCollection AddAllExportHandlers(
        this IServiceCollection services,
        params Assembly[] assemblies)
    {
        var exportDefType = typeof(IExportDefinition<>);

        foreach (var assembly in assemblies)
        {
            var exportDefs = assembly.GetTypes()
                .Where(t => !t.IsAbstract && !t.IsInterface)
                .SelectMany(t => t.GetInterfaces()
                    .Where(i => i.IsGenericType &&
                                i.GetGenericTypeDefinition() == exportDefType)
                    .Select(i => new
                    {
                        RowType = i.GetGenericArguments()[0],  // TRow
                        FilterType = (Type?)null                   // resolved below
                    }))
                .ToList();

            // Match each TRow to its TFilter via the registered endpoint delegates
            // Instead — scan for all GenericExportQuery<,> usages via ExportEndpoint registrations
            // Simplest: scan for all concrete IExportDefinition<TRow> → pair with all BaseFilter subclasses

            var filterTypes = assemblies
                .SelectMany(a => a.GetTypes())
                .Where(t => !t.IsAbstract && t.IsSubclassOf(typeof(BaseFilter)))
                .ToList();

            foreach (var def in exportDefs)
            {
                foreach (var filterType in filterTypes)
                {
                    var queryType = typeof(GenericExportQuery<,>)
                        .MakeGenericType(def.RowType, filterType);

                    var handlerType = typeof(GenericExportHandler<,>)
                        .MakeGenericType(def.RowType, filterType);

                    var serviceType = typeof(IRequestHandler<,>)
                        .MakeGenericType(queryType, typeof(ExportFileResult));

                    services.AddTransient(serviceType, handlerType);
                }
            }
        }

        return services;
    }
}