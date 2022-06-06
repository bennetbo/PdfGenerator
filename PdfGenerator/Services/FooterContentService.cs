using PdfGenerator.DTOs;
using PdfGenerator.Models;

namespace PdfGenerator.Services;  
public class FooterContentService : IPdfContentService<PdfFooterContent>
{    
  public IContentCreationStrategy GetContentCreationStrategy(PdfFooterContent pageContent, int width, int height) => pageContent switch
  {
    PdfFooterContent.PageCount => new PageCounterContentStrategy(),
    _ => throw new NotImplementedException(),
  };
}

