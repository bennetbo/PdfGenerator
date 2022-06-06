using QuestPDF.Infrastructure;

namespace PdfGenerator.Models;

public delegate void ContentCreationStrategy(IContainer container);

public interface IPdfContentService<PdfContentType>
{
  ContentCreationStrategy GetContentCreationStrategy(PdfContentType pageContent, int width, int height);
}