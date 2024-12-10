using MedicalAssistant.Application.Dto;
using MedicalAssistant.Domain.Enums;
using MedicalAssistant.Infrastructure.PDF.Translation;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace MedicalAssistant.Infrastructure.PDF.Reports;

internal sealed class MedicalHistoryReport(
    Languages languages, 
    List<MedicalHistoryDto> medicalHistory) : Translations(languages), IDocument
{
    private static readonly string ImagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", "Logo.svg");

    public void Compose(IDocumentContainer container)
    {
        container.Page(page =>
        {
            // PAGE SETTINGS
            page.Size(PageSizes.A4);
            page.Margin(1.5f, Unit.Centimetre);
            page.DefaultTextStyle(x => x.FontSize(12));
            
            // HEADER
            ComposeHeader(page.Header());

            // PAGE CONTENT
            ComposePageContent(page.Content(), medicalHistory);

            // FOOTER
            ComposeFooter(page.Footer());
        });
    }
    
    
    private void ComposeHeader(IContainer container)
    {
        container.ShowOnce()
            .Height(100)
            .Row(row =>
            {
                row.RelativeItem(2).AlignLeft().Column(column => { });
                row.RelativeItem(2).AlignCenter().AlignMiddle()
                    .Text(Translate(TranslationKeys.MedicalHistoryTitle))
                    .FontColor(Colors.Blue.Accent4).Bold()
                    .FontSize(20);
                row.RelativeItem(2).AlignRight().AlignTop().Column(column =>
                {
                    column.Item().Text(DateTime.Now.ToString("dd.MM.yyyy"))
                        .AlignCenter().FontSize(12).Bold();
                    column.Item().MaxWidth(80).MaxHeight(60).AlignRight()
                        .Svg(SvgImage.FromFile(ImagePath)).FitArea();
                });
            });
    }
    private void ComposeFooter(IContainer container)
    {
        container.AlignCenter()
            .Text(Translate(TranslationKeys.Footer))
            .FontSize(10);
    }
    private void ComposePageContent(IContainer container, List<MedicalHistoryDto> histories)
    {
        int pageCount = histories.Count;
        int currentPage = 1;

        container.Column(column =>
        {
            foreach (var history in histories )
            {
                AddHistory(column, history);
                bool hasMoreContent = currentPage < pageCount;
                if (hasMoreContent)
                {
                    currentPage++;
                    column.Item().PageBreak();
                }
            }
        });
    }

    private void AddHistory(ColumnDescriptor column,MedicalHistoryDto history)
    {
        column.Item()
            .PaddingTop(20).PaddingVertical(14)
            .Text(Translate(TranslationKeys.DiseaseDetails))
            .AlignCenter()
            .FontSize(16).Bold().Underline();
        
        column.Item().PaddingVertical(4).Row(row =>
        {
            row.RelativeItem(1).AlignLeft().Text(Translate(TranslationKeys.DiseaseName)).Bold();
            row.RelativeItem(3).Text(history.DiseaseName);
        });
        
        column.Item().PaddingVertical(4).Row(row =>
        {
            row.RelativeItem(1).Text(Translate(TranslationKeys.Date)).Bold();
            row.RelativeItem(3).Text($"{history.StartDate:dd.MM.yyyy}");
        });

        if (history.SymptomDescription is not null)
        {
            column.Item().PaddingVertical(4).Row(row =>
            {
                row.RelativeItem(1).AlignLeft().Text(Translate(TranslationKeys.SymptomDescription)).Bold();
                row.RelativeItem(3).Text(history.SymptomDescription);
            });
        }
        
        if (history.Notes is not null)
        {
            column.Item().PaddingVertical(4).Row(row =>
            {
                row.RelativeItem(1).AlignLeft().Text(Translate(TranslationKeys.Note)).Bold();
                row.RelativeItem(3).Text(history.Notes);
            });
        }

        if (history.EndDate is not null)
        {
            column.Item().PaddingVertical(4).Row(row =>
            {
                row.RelativeItem(1).Text(Translate(TranslationKeys.EndDate)).Bold();
                row.RelativeItem(3).Text($"{history.EndDate:dd.MM.yyyy}");
            });
        }

        if (history.Stages is not null && 
            history.Stages.Count != 0)
        {
            AddStages(column,history.Stages);
        }
    }

    private void AddStages(ColumnDescriptor column, List<DiseaseStageDto> stages)
    {
        column.Item().PaddingTop(25).PaddingBottom(8)
            .Text(Translate(TranslationKeys.DiseaseStagesTitle))
            .AlignCenter().FontSize(16).Bold().Underline();
        
        // Name
        // Date
        // NOTES
        
        column.Item().Table(table =>
        {
            table.ColumnsDefinition(columns =>
            {
                columns.RelativeColumn(2);
                columns.RelativeColumn(1);
                columns.RelativeColumn(3);
            });

            table.Header(header =>
            {
                header.Cell().Element(CellStyle).Text(Translate(TranslationKeys.StageName)).Bold();
                header.Cell().Element(CellStyle).Text(Translate(TranslationKeys.StartDate)).Bold();
                header.Cell().Element(CellStyle).Text(Translate(TranslationKeys.Note)).Bold();

            });

            foreach (var stage in stages)
            {
                table.Cell().Element(CellStyle).Text(stage.Name);
                table.Cell().Element(CellStyle).Text(stage.Date.ToString("dd.MM.yyyy"));
                table.Cell().Element(CellStyle).Text(stage.Note);
            }
        });
    }
    
    private static IContainer CellStyle(IContainer container) =>
        container.Border(1).BorderColor(Colors.Grey.Lighten2).Padding(5).AlignLeft();

}