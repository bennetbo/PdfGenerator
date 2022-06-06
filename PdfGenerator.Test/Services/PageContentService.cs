using Moq;
using PdfGenerator.DTOs;
using PdfGenerator.Services;
using QuestPDF.Infrastructure;

namespace PdfGenerator.Test.Services;
public class PageContentServiceTests
{
  const int DUMMY_WIDTH = 400;
  const int DUMMY_HEIGHT = 400;

  private PageContentService pageContentService;

  [SetUp]
  public void Setup()
  {
    pageContentService = new();
  }

  [Test]
  public void TestGetContentCreationStrategy_InputAreAllPageContentContentTypes_ProvidesStrategyForAllValues()
  {
    foreach (PdfPageContent contentType in Enum.GetValues(typeof(PdfPageContent)))
      Assert.That(() => pageContentService.GetContentCreationStrategy(contentType, DUMMY_WIDTH, DUMMY_HEIGHT), Throws.Nothing);
  }
}