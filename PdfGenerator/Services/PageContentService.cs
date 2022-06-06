using PdfGenerator.DTOs;
using PdfGenerator.Models;
namespace PdfGenerator.Services;
public class PageContentService : IPdfContentService<PdfPageContent>
{
  public IContentCreationStrategy CreateEmtpyContentStrategy() 
    => new EmptyContentCreationStrategy();
  public IContentCreationStrategy CreateCustomTextContentStrategy(int pageIndex, params string[] Contents) 
    => new CustomContentCreationStrategy(Contents[pageIndex]);
  public IContentCreationStrategy CreateCatImageContentStrategy() 
    => new CatImageContentCreationStrategy();
  public IContentCreationStrategy CreateImageContentStrategy(int width, int height) 
    => new ImageContentCreationStrategy(width, height);
  public IContentCreationStrategy CreateRandomTextContentStrategy() 
    => new RandomContentCreationStrategy();

  public IContentCreationStrategy GetContentCreationStrategy(PdfPageContent pageContent, int width, int height) => pageContent switch
  {
    PdfPageContent.RandomSentences => CreateRandomTextContentStrategy(),
    PdfPageContent.Empty => CreateEmtpyContentStrategy(),
    PdfPageContent.Images => CreateImageContentStrategy(width, height),
    PdfPageContent.CatImages => CreateCatImageContentStrategy(),
    _ => throw new NotImplementedException(),
  };
}