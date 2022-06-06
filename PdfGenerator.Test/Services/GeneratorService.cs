using Moq;
using PdfGenerator.Models;
using PdfGenerator.Services;

namespace PdfGenerator.Test.Services;
public class GeneratorServiceTests
{
  private GeneratorService generatorService;

  [SetUp]
  public void Setup()
  {
    generatorService = new GeneratorService();
  }

  [Test]
  public void TestGenerate_InputIsStrategyStubs_CallsPageStubPageCountTimesAndFooterStubOneTime([Range(1, 30)]int pageCount)
  {
    var pageContentStrategyStub = new Mock<IContentCreationStrategy>();
    var footerContentStrategyStub = new Mock<IContentCreationStrategy>();
    generatorService.Generate(400, 450, pageCount, pageContentStrategyStub.Object, footerContentStrategyStub.Object);
    Assert.Multiple(() =>
    {
      Assert.That(pageContentStrategyStub.Invocations, Has.Count.EqualTo(pageCount));
      Assert.That(footerContentStrategyStub.Invocations, Has.Count.EqualTo(1));
    });
  }
}