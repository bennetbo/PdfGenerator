using PdfGenerator.Core.Enums;
using PdfGenerator.Core.Models;
using PdfGenerator.Core.Properties;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using System;

namespace PdfGenerator.Core.Services;

public class PageContentService : IPdfContentService<PdfPageContent>
{
  public ContentCreationStrategy GetContentCreationStrategy(PdfPageContent pageContent, int width, int height) => pageContent switch
  {
    PdfPageContent.RandomSentences => new ContentCreationStrategy(c => c.Text(Placeholders.Sentence())),
    PdfPageContent.Empty => new ContentCreationStrategy(_c => { }),
    PdfPageContent.Images => new ContentCreationStrategy(c => c.Image(Placeholders.Image(width, height))),
    PdfPageContent.CatImages => new ContentCreationStrategy(c => c.Image(Resources.Professor)),
    _ => throw new NotImplementedException(),
  };
}