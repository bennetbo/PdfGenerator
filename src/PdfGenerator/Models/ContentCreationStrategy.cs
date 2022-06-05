using PdfGenerator.DTOs;
using QuestPDF.Infrastructure;

namespace PdfGenerator.Models;

public interface IPageContentParams
{
  int PageCount { get; }
  PdfContent PdfContent { get; }
  IContentCreationStrategy ContentCreationStrategy { get; }
  IContentCreationStrategy FooterCreationStrategy { get; }
}

public interface IContentCreationStrategy
{
  void Use(IContainer container);
}

