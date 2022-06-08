using FluentValidation;
using PdfGenerator.DTOs;
using PdfGenerator.Models;
using PdfGenerator.Scoped;
using PdfGenerator.Validators;
using QuestPDF.Helpers;

namespace PdfGenerator.Services;

interface IHttpHandlerService
{
  IResult GetResult<ParamType>(ParamType generationParams,
    int pageCount,
    IValidator<ParamType> validator,
    PdfPageContent pageContent = PdfPageContent.RandomSentences,
    PdfFooterContent footerContent = PdfFooterContent.PageCount)
  where ParamType : IHasPageCount;
}
class HttpHandlerService : IHttpHandlerService
{
  public const string CONTENT_TYPE_APPLICATION_PDF = "application/pdf";

  private readonly IMeasurementService measurementService;
  private readonly IGeneratorService documentGeneratorService;
  private readonly IPdfContentService<PdfPageContent> pageContentService;
  private readonly IPdfContentService<PdfFooterContent> pageFooterService;
  private readonly IFilenameService filenameService;
  private readonly IConfigurationService configurationService;

  public HttpHandlerService(
    IMeasurementService measurementService,
    IGeneratorService documentGeneratorService,
    IPdfContentService<PdfPageContent> pageContentService,
    IPdfContentService<PdfFooterContent> pageFooterService,
    IFilenameService filenameService,
    IConfigurationService configurationService)
  {
    this.measurementService = measurementService;
    this.documentGeneratorService = documentGeneratorService;
    this.pageContentService = pageContentService;
    this.pageFooterService = pageFooterService;
    this.filenameService = filenameService;
    this.configurationService = configurationService;
  }

  PageSize GetPageSize<ParamType>(ParamType parameters)
    where ParamType : IHasPageCount
  {
    return parameters switch
    {
      PageSizeGenerationParams pageSizeGenerationParams => measurementService.GetValidPageSize(pageSizeGenerationParams.PageSize ?? "a4"),
      ExplicitGenerationParams explicitGenerationParams => measurementService.GetPageSizeOrDefault(explicitGenerationParams.Width ?? 0, explicitGenerationParams.Height ?? 0),
      _ => throw new NotImplementedException($"{parameters.GetType().Name} not supported")
    };
  }
  byte[] GetDocument(int width, int height, int index, PdfPageContent pageContent, PdfFooterContent footerContent)
  {
    var pageContentStrategy = pageContentService.GetContentCreationStrategy(pageContent, width, height);
    var footerContentStrategy = pageFooterService.GetContentCreationStrategy(footerContent, width, height);

    return documentGeneratorService.Generate(width, height, index, pageContentStrategy, footerContentStrategy);
  }

  IResult GetResultImpl<ParamType>(ParamType generationParams, int pageCount, PdfPageContent pageContent = PdfPageContent.RandomSentences, PdfFooterContent footerContent = PdfFooterContent.PageCount)
    where ParamType : IHasPageCount
  {
    var size = GetPageSize(generationParams);
    int width = (int)size.Width, height = (int)size.Height;
    var document = GetDocument(width, height, pageCount, pageContent, footerContent);
    var documentName = filenameService.Replace(configurationService.GetAppConfig().FileNamingSchema, width, height, pageCount);

    return Results.Bytes(contents: document, contentType: CONTENT_TYPE_APPLICATION_PDF, fileDownloadName: documentName);
  }

  public IResult GetResult<ParamType>(ParamType generationParams, int pageIndex, IValidator<ParamType> validator, PdfPageContent pageContent = PdfPageContent.RandomSentences, PdfFooterContent footerContent = PdfFooterContent.PageCount)
    where ParamType : IHasPageCount
  {
    var validationResult = validator.Validate(generationParams);

    return validationResult.IsValid
      ? GetResultImpl(generationParams, pageIndex, pageContent, footerContent)
      : Results.ValidationProblem(validationResult.Errors.ToErrorMessageDict());
  }
}
