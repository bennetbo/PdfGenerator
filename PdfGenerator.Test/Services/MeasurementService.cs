using PdfGenerator.Services;
using QuestPDF.Helpers;

namespace PdfGenerator.Test.Services;
public class MeasurementServiceTest
{
  MeasurementService measurementService;

  [SetUp]
  public void Setup()
  {
    measurementService = new MeasurementService();
  }

  [Test]
  public void GetValidPageSize_InputIsValidPageSize_ReturnValidPagesizeAndNotUsingDefault()
  {
    var validPageSizes = typeof(PageSizes).GetProperties().Select(p => p.Name);
    var defaultPageSizesShifted = validPageSizes.Where((value, index) => index > 0).Concat(validPageSizes.Where((value, index) => index == 0));

    foreach (var (pageSizeToTest, defaultSize) in validPageSizes.Zip(defaultPageSizesShifted))
    {
      var testResult = measurementService.GetValidPageSize(pageSizeToTest, defaultSize);
      var defaultResult = measurementService.GetValidPageSize(defaultSize);
      Assert.Multiple(() =>
      {
        Assert.That(pageSizeToTest, Is.Not.EqualTo(defaultSize));
        Assert.That(testResult, Is.Not.EqualTo(defaultResult));
      });
    }
  }

  [Test]
  public void IsValidPageSize_InputAllValidPageSizes_ReturnTrue()
  {
    var validPageSizes = typeof(PageSizes).GetProperties().Select(p => p.Name);

    Assert.That(validPageSizes.All(measurementService.IsValidPageSize), Is.True);
  }

  [Test, Sequential]
  public void IsValidPageSize_InputInvalidPageSize_ReturnFalse([Values("  ", "a100", "####", "AA1sdd")] string pageSize)
  {
    var validPageSizes = typeof(PageSizes).GetProperties().Select(p => p.Name);
    Assert.Multiple(() =>
    {
      Assert.That(validPageSizes.Contains(pageSize), Is.False);
      Assert.That(measurementService.IsValidPageSize(pageSize), Is.False);
    });
  }

  [TestCase("  ")]
  [TestCase("a100")]
  [TestCase("####")]
  [TestCase("AA1sdd")]
  public void GetValidPageSize_InputIsUnknownPageSizeDefaultValueIsNull_ReturnDefaultPageSize(string pageSize)
  {
    var validPageSizes = typeof(PageSizes).GetProperties().Select(p => p.Name);
    var defaultPageSizesShifted = validPageSizes.Where((value, index) => index > 0).Concat(validPageSizes.Where((value, index) => index == 0));
    var testResult = measurementService.GetValidPageSize(pageSize);
    Assert.Multiple(() =>
    {
      Assert.That(measurementService.IsValidPageSize(pageSize), Is.False);
      Assert.That(() => measurementService.GetValidPageSize(pageSize), Throws.Nothing);
      Assert.That(testResult, Is.EqualTo(measurementService.GetValidPageSize("a4")));
    });
  }

  [Test, Combinatorial]
  public void GetValidPageSize_InputIsUnknownPageSizeUnknownDefaultSize_UsesA4Format(
    [Values("  ", "a100", "####", "AA1sdd")] string pageSize,
    [Values("  ", "a100", "####", "AA1sdd")] string defaultPageSize)
  {
    var testResult = measurementService.GetValidPageSize("a4");
    Assert.Multiple(() =>
    {
      Assert.That(measurementService.IsValidPageSize(pageSize), Is.False);
      Assert.That(measurementService.IsValidPageSize(defaultPageSize), Is.False);
      Assert.That(() => measurementService.GetValidPageSize(pageSize, defaultPageSize), Throws.Nothing);
      Assert.That(testResult, Is.EqualTo(measurementService.GetValidPageSize("a4")));
    });
  }
}