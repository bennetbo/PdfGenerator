using System.Diagnostics.CodeAnalysis;

namespace PdfGenerator.Test;

public abstract class TestBase<T>
{
  [NotNull] //Marking as not null here, because setup method is called before each test run.
  protected T? Sut { get; private set; }

  [SetUp]
  public void Setup()
  {
    Sut = DoSetup();
  }

  [TearDown]
  public void TearDown()
  {
    Sut = default;
  }

  protected abstract T DoSetup();
}
