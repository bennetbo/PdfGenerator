using PdfGenerator.Core.Enums;
using PdfGenerator.Core.Services;

namespace PdfGenerator.Test.Services;

public class PageContentServiceTests : TestBase<PageContentService>
{
  private const int DUMMY_WIDTH = 400;
  private const int DUMMY_HEIGHT = 400;

  protected override PageContentService DoSetup() => new();

  [Test]
  public void TestGetContentCreationStrategy_InputAreAllPageContentContentTypes_ProvidesStrategyForAllValues()
  {
    foreach (PdfPageContent contentType in Enum.GetValues(typeof(PdfPageContent)))
      Assert.That(() => Sut.GetContentCreationStrategy(contentType, DUMMY_WIDTH, DUMMY_HEIGHT), Throws.Nothing);
  }
}