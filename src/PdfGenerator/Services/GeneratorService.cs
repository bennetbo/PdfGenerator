using PdfGenerator.Models;
using QuestPDF.Drawing;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace PdfGenerator.Services;
public interface IGeneratorService
{
  byte[] Generate(float width, float height, int pageCount, IContentCreationStrategy pageContentCreationStrategy, IContentCreationStrategy footerContentCreationStrategy);
}

public class GeneratorService : IGeneratorService
{
  public byte[] Generate(float width, float height, int pageCount, IContentCreationStrategy pageContentCreationStrategy, IContentCreationStrategy footerContentCreationStrategy)
  {
    return Document.Create(c =>
    {
      c.Page(p =>
      {
        p.Margin(50);
        p.Size(width, height, Unit.Point);
        p.DefaultTextStyle(t => t.FontSize(18));

        p.Content().Column(c =>
        {
          foreach (var pageIndex in Enumerable.Range(0, pageCount))
          {
            pageContentCreationStrategy.Use(c.Item());
            if (pageIndex < pageCount - 1)
              c.Item().PageBreak();
          }
        });
        footerContentCreationStrategy.Use(p.Footer());
      });
    }).WithMetadata(new DocumentMetadata()
    {
      DocumentLayoutExceptionThreshold = 2500,
    }).GeneratePdf();
  }
}

