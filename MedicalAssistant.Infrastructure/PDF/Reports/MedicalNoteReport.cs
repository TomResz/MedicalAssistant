using MedicalAssistant.Application.Dto;
using MedicalAssistant.Domain.Enums;
using MedicalAssistant.Infrastructure.PDF.Translation;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace MedicalAssistant.Infrastructure.PDF.Reports;

internal sealed class MedicalNoteReport(Languages language, List<MedicalNoteDto> notes) : Translations(language), IDocument
{
    private static readonly string
        ImagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", "Logo.svg");

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
            ComposePageContent(page.Content(), notes);

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
                    .Text(Translate(TranslationKeys.NoteReportTitle))
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

    private void ComposePageContent(IContainer container, List<MedicalNoteDto> notes)
    {
        container.Column(column => { AddNotes(column, notes); });
    }

    private void AddNotes(ColumnDescriptor column, List<MedicalNoteDto> notes)
    {
        column.Item().PaddingTop(25).PaddingBottom(8)
            .Text(Translate(TranslationKeys.Notes))
            .AlignCenter().FontSize(16).Bold().Underline();

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
                header.Cell().Element(CellStyle).Text(Translate(TranslationKeys.Tags)).Bold();
                header.Cell().Element(CellStyle).Text(Translate(TranslationKeys.Date)).Bold();
                header.Cell().Element(CellStyle).Text(Translate(TranslationKeys.Note)).Bold();
            });

            foreach (var note in notes)
            {
                table.Cell().Element(CellStyle).Text(string.Join(", ", note.Tags));
                table.Cell().Element(CellStyle).Text(note.CreatedAt.ToString("HH:mm  dd.MM.yyyy"));
                table.Cell().Element(CellStyle).Text(note.Note);
            }
        });
    }

    private static IContainer CellStyle(IContainer container) =>
        container.Border(1).BorderColor(Colors.Grey.Lighten2).Padding(5).AlignLeft();
}