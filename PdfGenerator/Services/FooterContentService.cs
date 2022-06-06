using PdfGenerator.DTOs;
using PdfGenerator.Models;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace PdfGenerator.Services
{  
  public class FooterContentService : IPdfContentService<PdfFooterContent>
  {
    record PageCounterContentStrategy() : IContentCreationStrategy
    {
      public void Use(IContainer container) => container.AlignCenter().Text(t =>
      {
        t.CurrentPageNumber();
        t.Span(" / ");
        t.TotalPages();
      });
    }
    public IContentCreationStrategy GetContentCreationStrategy(PdfFooterContent pageContent, int width, int height) => pageContent switch
    {
      PdfFooterContent.PageCount => new PageCounterContentStrategy(),
      _ => throw new NotImplementedException(),
    };
  }
}
