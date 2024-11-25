using MedicalAssistant.Application.Contracts;
using MedicalAssistant.Infrastructure.PDF.Services;
using Microsoft.Extensions.DependencyInjection;
using QuestPDF;
using QuestPDF.Infrastructure;

namespace MedicalAssistant.Infrastructure.PDF;

public static class Extensions
{
    public static IServiceCollection AddPDFServices(this IServiceCollection services)
    {
        Settings.License = LicenseType.Community;
        services.AddScoped<IVisitReportPdfService, VisitReportPdfService>();
        return services;
    }
}