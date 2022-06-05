using FluentValidation;
using PdfGenerator.DTOs;
using PdfGenerator.Models;
using PdfGenerator.Validators;
using QuestPDF.Helpers;

namespace PdfGenerator.Services;

interface IHttpHandlerService
{
  IResult GetResult<ParamType>(ParamType param, IValidator<ParamType> validator, PdfPageContent contentType = PdfPageContent.RandomSentences, PdfFooterContent footerContent = PdfFooterContent.PageCount)
    where ParamType : IHasPageCount;
}
class HttpHandlerService : IHttpHandlerService
{
  IMeasurementService MeasurementService { get; }
  IGeneratorService DocumentGeneratorService { get; }
  IPdfContentService<PdfPageContent> PageContentService { get; }
  IPdfContentService<PdfFooterContent> PageFooterService { get; }

  public HttpHandlerService(IMeasurementService measurementService, IGeneratorService documentGeneratorService, IPdfContentService<PdfPageContent> pageContentService, IPdfContentService<PdfFooterContent> pageFooterService)
  {
    MeasurementService = measurementService;
    DocumentGeneratorService = documentGeneratorService;
    PageContentService = pageContentService;
    PageFooterService = pageFooterService;
  }

  PageSize GetPageSize<ParamType>(ParamType parameters) where ParamType : IHasPageCount
  {
    return parameters switch
    {
      PageSizeGenerationParams pageSizeGenerationParams => MeasurementService.GetValidPageSize(pageSizeGenerationParams.PageSize ?? "a4"),
      ExplicitGenerationParams explicitGenerationParams => MeasurementService.GetPageSizeOrDefault(explicitGenerationParams.Width ?? 0, explicitGenerationParams.Height ?? 0),
      _ => throw new NotImplementedException($"{parameters.GetType().Name} not supported")
    };
  }

  public IResult GetResult<ParamType>(ParamType generationParams, IValidator<ParamType> validator, PdfPageContent pageContent = PdfPageContent.RandomSentences, PdfFooterContent footerContent = PdfFooterContent.PageCount)
    where ParamType : IHasPageCount
  {
    var validationResult = validator.Validate(generationParams);
    if (validationResult.IsValid)
    {
      var size = GetPageSize(generationParams);
      var pageContentStrategy = PageContentService.GetContentCreationStrategy(pageContent, (int)size.Width, (int)size.Height);
      var footerContentStrategy = PageFooterService.GetContentCreationStrategy(footerContent, (int)size.Width, (int)size.Height);
      var document = DocumentGeneratorService.Generate(size.Width, size.Height, generationParams.PageCount, pageContentStrategy, footerContentStrategy);
      return Results.Bytes(
        contents: document,
        contentType: "application/pdf",
        fileDownloadName: $"test_pdf_w{size.Width}_h{size.Height}_p{generationParams.PageCount}.pdf"
      );
    }
    return Results.ValidationProblem(validationResult.Errors.ToErrorMessageDict());
  }
}
