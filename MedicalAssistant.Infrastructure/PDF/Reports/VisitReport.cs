using MedicalAssistant.Application.Dto;
using MedicalAssistant.Domain.Enums;
using MedicalAssistant.Infrastructure.PDF.Translation;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace MedicalAssistant.Infrastructure.PDF.Reports;

internal sealed class VisitReport(
    Languages languages,
    List<VisitWithRecommendationsDto> visits)
    : Translations(languages), IDocument
{
    private static readonly string ImagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", "Logo.svg");

    public void Compose(IDocumentContainer container)
    {
          container.Page(page =>
        {
            page.Size(PageSizes.A4);
            page.Margin(2, Unit.Centimetre);
            page.DefaultTextStyle(x => x.FontSize(12));

            page.Header()
                .ShowOnce()
                .Height(100)
                .Padding(10)
                .Row(row =>
                {
                    row.RelativeItem(2).AlignLeft().Column(column => { });
                    row.RelativeItem(2).AlignCenter().Text(Translate(TranslationKeys.VisitReportTitle)).FontColor(Colors.Blue.Darken1).Bold()
                        .FontSize(20);
                    row.RelativeItem(2).AlignRight().Column(column =>
                    {
                        column.Item().Text(DateTime.Now.ToString("dd.MM.yyyy")).AlignLeft().FontSize(12).Bold();
                        column.Item().AlignLeft().Height(48).Width(64).Svg(SvgImage.FromFile(ImagePath));
                    });
                });

            int pageCount = visits.Count;
            int currentPage = 1;
            page.Content()
                .Column(column =>
                {
                    foreach (var visit in visits)
                    {
                        column.Item().PaddingTop(20).PaddingVertical(14).Text(Translate(TranslationKeys.VisitDetails)).AlignCenter()
                            .FontSize(16).Bold().Underline();

                        column.Item().PaddingVertical(4).Row(row =>
                        {
                            row.RelativeItem(1).AlignLeft().Text(Translate(TranslationKeys.Doctor)).Bold();
                            row.RelativeItem(3).Text(visit.DoctorName);
                        });

                        column.Item().PaddingVertical(4).Row(row =>
                        {
                            row.RelativeItem(1).Text(Translate(TranslationKeys.Type)).Bold();
                            row.RelativeItem(3).Text($"{visit.VisitType}");
                        });

                        column.Item().PaddingVertical(4).Row(row =>
                        {
                            row.RelativeItem(1).Text(Translate(TranslationKeys.Date)).Bold();
                            row.RelativeItem(3).Text($"{visit.Date:HH:mm  dd.MM.yyyy}");
                        });

                        column.Item().PaddingVertical(4).Row(row =>
                        {
                            row.RelativeItem(1).Text(Translate(TranslationKeys.Description)).Bold();
                            row.RelativeItem(3).Text(visit.VisitDescription == null ? string.Empty : visit.VisitDescription);
                        });

                        if (visit.Recommendations is not null &&
                                visit.Recommendations.Any())
                        {
                            column.Item().PaddingTop(25)
                                .PaddingBottom(8)
                                .Text(Translate(TranslationKeys.MedicalRecommendations))
                                .AlignCenter()
                                .FontSize(16)
                                .Bold()
                                .Underline();

                            column.Item().Table(table =>
                            {
                                table.ColumnsDefinition(columns =>
                                {
                                    columns.RelativeColumn(2);
                                    columns.RelativeColumn(1);
                                    columns.RelativeColumn(2);
                                    columns.RelativeColumn(2);
                                    columns.RelativeColumn(2);
                                    columns.RelativeColumn(2);
                                });

                                table.Header(header =>
                                {
                                    header.Cell().Element(CellStyle).Text(Translate(TranslationKeys.Medicine)).Bold();
                                    header.Cell().Element(CellStyle).Text(Translate(TranslationKeys.Quantity)).Bold();
                                    header.Cell().Element(CellStyle).Text(Translate(TranslationKeys.TimeOfDay)).Bold();
                                    header.Cell().Element(CellStyle).Text(Translate(TranslationKeys.Note)).Bold();
                                    header.Cell().Element(CellStyle).Text(Translate(TranslationKeys.StartDate)).Bold();
                                    header.Cell().Element(CellStyle).Text(Translate(TranslationKeys.EndDate)).Bold();
                                });

                                foreach (var recommendation in visit.Recommendations)
                                {
                                    table.Cell().Element(CellStyle).Text(recommendation.MedicineName);
                                    table.Cell().Element(CellStyle).Text(recommendation.MedicineQuantity.ToString());
                                    table.Cell().Element(CellStyle)
                                        .Text(string.Join(", ", recommendation.MedicineTimeOfDay));
                                    table.Cell().Element(CellStyle).Text(recommendation.ExtraNote);
                                    table.Cell().Element(CellStyle).Text($"{recommendation.BeginDate:dd.MM.yyyy}");
                                    table.Cell().Element(CellStyle).Text($"{recommendation.EndDate:dd.MM.yyyy}");
                                }
                            });
                        }

                        bool hasMoreContent = currentPage < pageCount;
                        if (hasMoreContent)
                        {
                            currentPage++;
                            column.Item().PageBreak();
                        }
                    }
                });
            page.Footer()
                .AlignCenter()
                .Text(Translate(TranslationKeys.Footer))
                .FontSize(10);
        });
    }
    
    private static IContainer CellStyle(IContainer container) =>
        container.Border(1).BorderColor(Colors.Grey.Lighten2).Padding(5).AlignLeft();
}