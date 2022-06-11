using PdfGenerator.Core.Services;
using QuestPDF.Helpers;

namespace PdfGenerator.Test.Services;
public class MeasurementServiceTest
{
  MeasurementService measurementService;
  const int VALID_PDF_MAX_SIZE = 14400;
  const int VALID_PDF_MIN_SIZE = 300;

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

  [Test, Combinatorial, Parallelizable]
  public void TestIsValidHeight_InputIsValidAndInvalidIntegers_ReturnValidResult(
    [Range(VALID_PDF_MIN_SIZE, VALID_PDF_MIN_SIZE + 10)] int valid,
    [Range(VALID_PDF_MIN_SIZE - 11, VALID_PDF_MIN_SIZE - 1)] int lowerInvalid,
    [Range(VALID_PDF_MAX_SIZE + 1, VALID_PDF_MAX_SIZE + 11)] int upperInvalid)
    => Assert.Multiple(() =>
      {
        Assert.That(measurementService.IsValidHeight(valid), Is.True);
        Assert.That(measurementService.IsValidHeight(lowerInvalid), Is.False);
        Assert.That(measurementService.IsValidHeight(upperInvalid), Is.False);
      });

  [Test, Combinatorial, Parallelizable]
  public void TestIsValidWidth_InputIsValidAndInvalidIntegers_ReturnValidResult(
    [Range(VALID_PDF_MIN_SIZE, VALID_PDF_MIN_SIZE + 10)] int valid,
    [Range(VALID_PDF_MIN_SIZE - 11, VALID_PDF_MIN_SIZE - 1)] int lowerInvalid,
    [Range(VALID_PDF_MAX_SIZE + 1, VALID_PDF_MAX_SIZE + 11)] int upperInvalid)
    => Assert.Multiple(() =>
    {
      Assert.That(measurementService.IsValidWidth(valid), Is.True);
      Assert.That(measurementService.IsValidWidth(lowerInvalid), Is.False);
      Assert.That(measurementService.IsValidWidth(upperInvalid), Is.False);
    });

  [Test, Sequential, Parallelizable]
  public void TestIsValidPageSize_InputIsValidAndInvalidPageSize_ReturnValidResult(
    [Values("a4", "a0", "letter")] string validPageSize,
    [Values("  ", "fds", "786767a")] string invalidPageSize)
    => Assert.Multiple(() =>
    {
      Assert.That(measurementService.IsValidPageSize(validPageSize), Is.True);
      Assert.That(measurementService.IsValidPageSize(invalidPageSize), Is.False);
    });

  [Test, Combinatorial, Parallelizable]
  public void TestGetValidPageSize_InputIsValidAndInvalidIntegers_NeverThrows(
    [Range(VALID_PDF_MIN_SIZE, VALID_PDF_MIN_SIZE + 10)] int valid,
    [Range(VALID_PDF_MIN_SIZE - 11, VALID_PDF_MIN_SIZE - 1)] int invalid)
    => Assert.Multiple(() =>
    {
      Assert.That(() => measurementService.GetPageSizeOrDefault(valid, valid), Throws.Nothing);
      Assert.That(() => measurementService.GetPageSizeOrDefault(valid, invalid), Throws.Nothing);
      Assert.That(() => measurementService.GetPageSizeOrDefault(invalid, valid), Throws.Nothing);
      Assert.That(() => measurementService.GetPageSizeOrDefault(invalid, invalid), Throws.Nothing);
    });

}