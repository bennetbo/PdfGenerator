using PdfGenerator.DTOs;
using PdfGenerator.Models;
using QuestPDF.Fluent;

namespace PdfGenerator.Services;
public class FooterContentService : IPdfContentService<PdfFooterContent>
{
  public ContentCreationStrategy GetContentCreationStrategy(PdfFooterContent pageContent, int width, int height) => pageContent switch
  {
    PdfFooterContent.PageCount => new ContentCreationStrategy(c => c.AlignCenter().Text(t =>
    {
      t.CurrentPageNumber();
      t.Span(" / ");
      t.TotalPages();
    })),
    _ => throw new NotImplementedException(),
  };
}

