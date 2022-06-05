using PdfGenerator.DTOs;
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


