using PdfGenerator.Core.Enums;
using PdfGenerator.Core.Services;

namespace PdfGenerator.Test.Services;
public class FooterContentServiceTests : TestBase<FooterContentService>
{
  private const int DUMMY_WIDTH = 400;
  private const int DUMMY_HEIGHT = 400;

  protected override FooterContentService DoSetup() => new();

  [Test]
  public void TestGetContentCreationStrategy_InputAreAllPageContentContentTypes_ProvidesStrategyForAllValues()
  {
    foreach (PdfFooterContent contentType in Enum.GetValues(typeof(PdfFooterContent)))
      Assert.That(() => Sut.GetContentCreationStrategy(contentType, DUMMY_WIDTH, DUMMY_HEIGHT), Throws.Nothing);
  }
}