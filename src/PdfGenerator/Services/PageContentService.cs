using PdfGenerator.DTOs;
using PdfGenerator.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace PdfGenerator.Services;
public interface IPageContentService
{
  IContentCreationStrategy CreateEmtpyContentStrategy();
  IContentCreationStrategy CreateRandomTextContentStrategy();
  IContentCreationStrategy CreateCustomTextContentStrategy(int pageIndex, params string[] Contents);
  IContentCreationStrategy CreateCatImageContentStrategy();
  IContentCreationStrategy CreateImageContentStrategy(int width, int height);
}

public class PageContentService : IPageContentService
{
  record EmptyContentCreationStrategy() : IContentCreationStrategy 
  { 
    public void Use(IContainer container) { } 
  }
  record CustomContentCreationStrategy(string Content) : IContentCreationStrategy 
  { 
    public void Use(IContainer container)  => container.Text(Content);
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

}