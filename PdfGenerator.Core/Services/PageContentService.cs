using PdfGenerator.Core.Enums;
using PdfGenerator.Core.Models;
using QuestPDF.Helpers;
using QuestPDF.Fluent;
using System;

namespace PdfGenerator.Core.Services;

public class PageContentService : IPdfContentService<PdfPageContent>
{
  public ContentCreationStrategy GetContentCreationStrategy(PdfPageContent pageContent, int width, int height) => pageContent switch
  {
    PdfPageContent.RandomSentences => new ContentCreationStrategy(c => c.Text(Placeholders.Sentence())),
    PdfPageContent.Empty => new ContentCreationStrategy(_c => { }),
    PdfPageContent.Images => new ContentCreationStrategy(c => c.Image(Placeholders.Image(width, height))),
    PdfPageContent.CatImages => new ContentCreationStrategy(c => c.Image("./Images/Professor.jpeg")),
    _ => throw new NotImplementedException(),
  };
}