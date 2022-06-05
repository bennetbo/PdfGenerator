using PdfGenerator.DTOs;
using PdfGenerator.Models;
using QuestPDF.Drawing;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace PdfGenerator.Services;
public interface IGeneratorService
{
  byte[] Generate(float width, float height, int pageCount, PdfContent content = PdfContent.RandomSentences);
}

public class GeneratorService : IGeneratorService
{
  public GeneratorService(IPageContentService pageContentService)
  {
    PageContentService = pageContentService;
  }

  public IPageContentService PageContentService { get; }

  public byte[] Generate(float width, float height, int pageCount, PdfContent content = PdfContent.RandomSentences)
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
            IContentCreationStrategy contentCreationStrategy = content switch
            {
              PdfContent.RandomSentences => PageContentService.CreateRandomTextContentStrategy(),
              PdfContent.Empty => PageContentService.CreateEmtpyContentStrategy(),
              PdfContent.Images => PageContentService.CreateImageContentStrategy((int)width, (int)height),
              PdfContent.CatImages => PageContentService.CreateCatImageContentStrategy(),
              _ => throw new NotImplementedException(),
            };
            contentCreationStrategy.Use(c.Item());

            if (pageIndex < pageCount - 1)
              c.Item().PageBreak();
          }
        });
        p.Footer().AlignCenter().Text(t =>
        {
          t.CurrentPageNumber();
          t.Span(" / ");
          t.TotalPages();
        });
      });
    }).WithMetadata(new DocumentMetadata()
    {
      DocumentLayoutExceptionThreshold = 2500,
    }).GeneratePdf();
  }
}

