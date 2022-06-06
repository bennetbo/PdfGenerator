using PdfGenerator.DTOs;
using PdfGenerator.Services;

namespace PdfGenerator.Test.Services;
public class FooterContentServiceTests
{
  const int DUMMY_WIDTH = 400;
  const int DUMMY_HEIGHT = 400;

  private FooterContentService footerContentService;

  [SetUp]
  public void Setup()
  {
    footerContentService = new();
  }

  [Test]
  public void TestGetContentCreationStrategy_InputAreAllPageContentContentTypes_ProvidesStrategyForAllValues()
  {
    foreach (PdfFooterContent contentType in Enum.GetValues(typeof(PdfFooterContent)))
      Assert.That(() => footerContentService.GetContentCreationStrategy(contentType, DUMMY_WIDTH, DUMMY_HEIGHT), Throws.Nothing);
  }
}