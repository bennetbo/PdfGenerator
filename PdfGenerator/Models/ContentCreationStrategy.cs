using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace PdfGenerator.Models;
public interface IContentCreationStrategy
{
  void Use(IContainer container);
}
public interface IPdfContentService<PdfContentType>
{
  IContentCreationStrategy GetContentCreationStrategy(PdfContentType pageContent, int width, int height);
}
public record EmptyContentCreationStrategy() : IContentCreationStrategy
{
  public void Use(IContainer container) { }
}
public record CustomContentCreationStrategy(string Content) : IContentCreationStrategy
{
  public void Use(IContainer container) => container.Text(Content);
}
public record RandomContentCreationStrategy() : IContentCreationStrategy
{
  public void Use(IContainer container) => container.Text(Placeholders.Sentence());
}
public record CatImageContentCreationStrategy() : IContentCreationStrategy
{
  public void Use(IContainer container) => container.Image("./Images/Professor.jpeg");
}
public record ImageContentCreationStrategy(int Width, int Height) : IContentCreationStrategy
{
  public void Use(IContainer container) => container.Image(Placeholders.Image(Width, Height));
}
public record PageCounterContentStrategy() : IContentCreationStrategy
{
  public void Use(IContainer container) => container.AlignCenter().Text(t =>
  {
    t.CurrentPageNumber();
    t.Span(" / ");
    t.TotalPages();
  });
}