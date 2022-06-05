using PdfGenerator.DTOs;
using PdfGenerator.Models;
using QuestPDF.Drawing;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace PdfGenerator.Services;
public interface IGeneratorService
{
  byte[] Generate(GenerationDTO data, PdfContent content = PdfContent.RandomSentences);
}

public class GeneratorService : IGeneratorService
{
  public GeneratorService(IPageContentService pageContentService)
  {
    PageContentService = pageContentService;
  }

  public IPageContentService PageContentService { get; }

  public byte[] Generate(GenerationDTO data, PdfContent content = PdfContent.RandomSentences)
  {
    return Document.Create(c =>
    {
      c.Page(p =>
      {
        p.Margin(50);
        p.Size(data.Width, data.Height, Unit.Point);
        p.DefaultTextStyle(t => t.FontSize(18));

        p.Content().Column(c =>
        {
          foreach (var pageIndex in Enumerable.Range(0, data.PageCount))
          {
            IContentCreationStrategy contentCreationStrategy = content switch
            {
              PdfContent.RandomSentences => PageContentService.CreateRandomTextContentStrategy(),
              PdfContent.Empty => PageContentService.CreateEmtpyContentStrategy(),
              PdfContent.Images => PageContentService.CreateImageContentStrategy((int)data.Width, (int)data.Height),
              PdfContent.CatImages=> PageContentService.CreateCatImageContentStrategy(),
              _ => throw new NotImplementedException(),
            };
            contentCreationStrategy.Use(c.Item());

            if (pageIndex < data.PageCount - 1)
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

