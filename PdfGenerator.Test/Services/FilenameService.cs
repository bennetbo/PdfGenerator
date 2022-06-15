using PdfGenerator.Services;

namespace PdfGenerator.Test.Services;
public class FilenameServiceTests
{
  private FileNameService? sut;

  [SetUp]
  public void Setup()
  {
    sut = new FileNameService();
  }

  [Test]
  public void TestReplace_InputIsStringWithoutPlaceholders_ReturnResultEqualToStringWithoutPlaceholders()
  {
    var stringWithoutPlaceholders = "testname_without_placeholders.pdf";
    var result = sut.Replace(stringWithoutPlaceholders, 500, 500, 10);

    Assert.That(result, Is.EqualTo(stringWithoutPlaceholders));
  }

  [Test, Combinatorial]
  public void TestReplace_InputIsIntegerRanger_ReturnFilenameWithPlaceholdersReplaced([Range(1, 10)] int pageCount, [Range(550, 555)] int width, [Range(600, 605)] int height)
  {
    var stringPlaceholders = "test_{{PAGE_WIDTH}}_{{PAGE_HEIGHT}}_{{PAGE_COUNT}}.pdf";
    var result = sut.Replace(stringPlaceholders, width, height, pageCount);

    Assert.That(result, Is.EqualTo($"test_{width}_{height}_{pageCount}.pdf"));
  }

  [Test]
  public void TestReplace_InputIsIntegerRanger_ReturnFilenameWithMultiplePlaceholdersReplaced([Range(1, 10)] int pageCount, [Range(550, 555)] int width, [Range(600, 605)] int height)
  {
    var stringPlaceholders = "test_{{PAGE_WIDTH}}_{{PAGE_HEIGHT}}_{{PAGE_COUNT}}_{{PAGE_COUNT}}.pdf";
    var result = sut.Replace(stringPlaceholders, width, height, pageCount);

    Assert.That(result, Is.EqualTo($"test_{width}_{height}_{pageCount}_{pageCount}.pdf"));
  }

  [Test]
  public void TestReplace_InputIsFilenameWithUnknownPlaceholders_ThrowsExceptionContainingUnknownPlaceholder()
  {
    var stringPlaceholders = "test_{{PAGE_WIDTH}}_{{PAGE_HEIGHT}}_{{PAGE_COUNT}}_{{PAGE_UNKNOWN_VARIABLE}}.pdf";

    Assert.That(() => sut.Replace(stringPlaceholders, 500, 500, 10), Throws.TypeOf<ArgumentException>()
      .With.Message.Contains("PAGE_UNKNOWN_VARIABLE"));
  }

  [Test]
  public void TestReplace_InputIsFilenameInvalidBracketCount_ThrowsException()
  {
    var stringPlaceholders = "test_{{PAGE_WIDTH}}_{{PAGE_HEIGHT}}}_{{PAGE_COUNT}}_{{PAGE_UNKNOWN_VARIABLE}}.pdf";

    Assert.That(() => sut.Replace(stringPlaceholders, 500, 500, 10), Throws.TypeOf<ArgumentException>()
      .With.Message.Contains("invalid bracket count"));
  }

  [Test]
  public void TestReplace_InputIsFilenameInvalidBracketOrder_ThrowsException()
  {
    var stringPlaceholders = "test_{{PAGE_WIDTH}}_{{PAGE_HEIGHT}}}}aaa{{_{{PAGE_COUNT}}_{{PAGE_UNKNOWN_VARIABLE}}.pdf";

    Assert.That(() => sut.Replace(stringPlaceholders, 500, 500, 10), Throws.Exception);
  }
}