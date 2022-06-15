﻿using Moq;
using PdfGenerator.Core.Models;
using PdfGenerator.Core.Services;

namespace PdfGenerator.Test.Services;
public class GeneratorServiceTests
{
  private GeneratorService? sut;

  [SetUp]
  public void Setup()
  {
    sut = new GeneratorService();
  }

  [Test]
  public void TestGenerate_InputIsStrategyStubs_CallsPageStubPageCountTimesAndFooterStubOneTime([Range(1, 30)] int pageCount)
  {
    var pageContentStrategyStub = new Mock<ContentCreationStrategy>();
    var footerContentStrategyStub = new Mock<ContentCreationStrategy>();
    sut.Generate(400, 450, pageCount, pageContentStrategyStub.Object, footerContentStrategyStub.Object);
    Assert.Multiple(() =>
    {
      Assert.That(pageContentStrategyStub.Invocations, Has.Count.EqualTo(pageCount));
      Assert.That(footerContentStrategyStub.Invocations, Has.Count.EqualTo(1));
    });
  }
}