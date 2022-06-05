using PdfGenerator.DTOs;
using PdfGenerator.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace PdfGenerator.Services;
public class PageContentService : IPdfContentService<PdfPageContent>
{
  record EmptyContentCreationStrategy() : IContentCreationStrategy
  {
    public void Use(IContainer container) { }
  }
  record CustomContentCreationStrategy(string Content) : IContentCreationStrategy
  {
    public void Use(IContainer container) => container.Text(Content);
  }
  record RandomContentCreationStrategy() : IContentCreationStrategy
  {
    public void Use(IContainer container) => container.Text(Placeholders.Sentence());
  }
  record CatImageContentCreationStrategy() : IContentCreationStrategy
  {
    public void Use(IContainer container) => container.Image("./Images/Professor.jpeg");
  }
  record ImageContentCreationStrategy(int Width, int Height) : IContentCreationStrategy
  {
    public void Use(IContainer container) => container.Image(Placeholders.Image(Width, Height));
  }


  public IContentCreationStrategy CreateEmtpyContentStrategy() => new EmptyContentCreationStrategy();
  public IContentCreationStrategy CreateCustomTextContentStrategy(int pageIndex, params string[] Contents) => new CustomContentCreationStrategy(Contents[pageIndex]);
  public IContentCreationStrategy CreateCatImageContentStrategy() => new CatImageContentCreationStrategy();
  public IContentCreationStrategy CreateImageContentStrategy(int width, int height) => new ImageContentCreationStrategy(width, height);
  public IContentCreationStrategy CreateRandomTextContentStrategy() => new RandomContentCreationStrategy();

  public IContentCreationStrategy GetContentCreationStrategy(PdfPageContent pageContent, int width, int height) => pageContent switch
  {
    PdfPageContent.RandomSentences => CreateRandomTextContentStrategy(),
    PdfPageContent.Empty => CreateEmtpyContentStrategy(),
    PdfPageContent.Images => CreateImageContentStrategy(width, height),
    PdfPageContent.CatImages => CreateCatImageContentStrategy(),
    _ => throw new NotImplementedException(),
  };
}