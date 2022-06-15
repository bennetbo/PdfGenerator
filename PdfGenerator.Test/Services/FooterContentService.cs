using PdfGenerator.Core.Enums;
using PdfGenerator.Core.Services;

namespace PdfGenerator.Test.Services;
public class FooterContentServiceTests
{
  private const int DUMMY_WIDTH = 400;
  private const int DUMMY_HEIGHT = 400;

  private FooterContentService sut;

  [SetUp]
  public void Setup()
  {
    sut = new();
  }

  [Test]
  public void TestGetContentCreationStrategy_InputAreAllPageContentContentTypes_ProvidesStrategyForAllValues()
  {
    foreach (PdfFooterContent contentType in Enum.GetValues(typeof(PdfFooterContent)))
      Assert.That(() => sut.GetContentCreationStrategy(contentType, DUMMY_WIDTH, DUMMY_HEIGHT), Throws.Nothing);
  }
}