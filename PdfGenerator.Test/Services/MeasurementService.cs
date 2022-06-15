using PdfGenerator.Core.Services;
using QuestPDF.Helpers;

namespace PdfGenerator.Test.Services;

public class MeasurementServiceTest
{
  private const int VALID_PDF_MAX_SIZE = 14400;
  private const int VALID_PDF_MIN_SIZE = 300;

  private MeasurementService? sut;

  [SetUp]
  public void Setup()
  {
    sut = new MeasurementService();
  }

  [Test]
  public void GetValidPageSize_InputIsValidPageSize_ReturnValidPagesizeAndNotUsingDefault()
  {
    var validPageSizes = typeof(PageSizes).GetProperties().Select(p => p.Name);
    var defaultPageSizesShifted = validPageSizes.Where((value, index) => index > 0).Concat(validPageSizes.Where((value, index) => index == 0));

    foreach (var (pageSizeToTest, defaultSize) in validPageSizes.Zip(defaultPageSizesShifted))
    {
      var testResult = sut.GetValidPageSize(pageSizeToTest, defaultSize);
      var defaultResult = sut.GetValidPageSize(defaultSize);
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

    Assert.That(validPageSizes.All(sut.IsValidPageSize), Is.True);
  }

  [Test, Sequential]
  public void IsValidPageSize_InputInvalidPageSize_ReturnFalse([Values("  ", "a100", "####", "AA1sdd")] string pageSize)
  {
    var validPageSizes = typeof(PageSizes).GetProperties().Select(p => p.Name);
    Assert.Multiple(() =>
    {
      Assert.That(validPageSizes, Does.Not.Contain(pageSize));
      Assert.That(sut.IsValidPageSize(pageSize), Is.False);
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
    var testResult = sut.GetValidPageSize(pageSize);
    Assert.Multiple(() =>
    {
      Assert.That(sut.IsValidPageSize(pageSize), Is.False);
      Assert.That(() => sut.GetValidPageSize(pageSize), Throws.Nothing);
      Assert.That(testResult, Is.EqualTo(sut.GetValidPageSize("a4")));
    });
  }

  [Test, Combinatorial]
  public void GetValidPageSize_InputIsUnknownPageSizeUnknownDefaultSize_UsesA4Format(
    [Values("  ", "a100", "####", "AA1sdd")] string pageSize,
    [Values("  ", "a100", "####", "AA1sdd")] string defaultPageSize)
  {
    var testResult = sut.GetValidPageSize("a4");
    Assert.Multiple(() =>
    {
      Assert.That(sut.IsValidPageSize(pageSize), Is.False);
      Assert.That(sut.IsValidPageSize(defaultPageSize), Is.False);
      Assert.That(() => sut.GetValidPageSize(pageSize, defaultPageSize), Throws.Nothing);
      Assert.That(testResult, Is.EqualTo(sut.GetValidPageSize("a4")));
    });
  }

  [Test, Combinatorial, Parallelizable]
  public void TestIsValidHeight_InputIsValidAndInvalidIntegers_ReturnValidResult(
    [Range(VALID_PDF_MIN_SIZE, VALID_PDF_MIN_SIZE + 10)] int valid,
    [Range(VALID_PDF_MIN_SIZE - 11, VALID_PDF_MIN_SIZE - 1)] int lowerInvalid,
    [Range(VALID_PDF_MAX_SIZE + 1, VALID_PDF_MAX_SIZE + 11)] int upperInvalid)
    => Assert.Multiple(() =>
      {
        Assert.That(sut.IsValidHeight(valid), Is.True);
        Assert.That(sut.IsValidHeight(lowerInvalid), Is.False);
        Assert.That(sut.IsValidHeight(upperInvalid), Is.False);
      });

  [Test, Combinatorial, Parallelizable]
  public void TestIsValidWidth_InputIsValidAndInvalidIntegers_ReturnValidResult(
    [Range(VALID_PDF_MIN_SIZE, VALID_PDF_MIN_SIZE + 10)] int valid,
    [Range(VALID_PDF_MIN_SIZE - 11, VALID_PDF_MIN_SIZE - 1)] int lowerInvalid,
    [Range(VALID_PDF_MAX_SIZE + 1, VALID_PDF_MAX_SIZE + 11)] int upperInvalid)
    => Assert.Multiple(() =>
    {
      Assert.That(sut.IsValidWidth(valid), Is.True);
      Assert.That(sut.IsValidWidth(lowerInvalid), Is.False);
      Assert.That(sut.IsValidWidth(upperInvalid), Is.False);
    });

  [Test, Sequential, Parallelizable]
  public void TestIsValidPageSize_InputIsValidAndInvalidPageSize_ReturnValidResult(
    [Values("a4", "a0", "letter")] string validPageSize,
    [Values("  ", "fds", "786767a")] string invalidPageSize)
    => Assert.Multiple(() =>
    {
      Assert.That(sut.IsValidPageSize(validPageSize), Is.True);
      Assert.That(sut.IsValidPageSize(invalidPageSize), Is.False);
    });

  [Test, Combinatorial, Parallelizable]
  public void TestGetValidPageSize_InputIsValidAndInvalidIntegers_NeverThrows(
    [Range(VALID_PDF_MIN_SIZE, VALID_PDF_MIN_SIZE + 10)] int valid,
    [Range(VALID_PDF_MIN_SIZE - 11, VALID_PDF_MIN_SIZE - 1)] int invalid)
    => Assert.Multiple(() =>
    {
      Assert.That(() => sut.GetPageSizeOrDefault(valid, valid), Throws.Nothing);
      Assert.That(() => sut.GetPageSizeOrDefault(valid, invalid), Throws.Nothing);
      Assert.That(() => sut.GetPageSizeOrDefault(invalid, valid), Throws.Nothing);
      Assert.That(() => sut.GetPageSizeOrDefault(invalid, invalid), Throws.Nothing);
    });

}