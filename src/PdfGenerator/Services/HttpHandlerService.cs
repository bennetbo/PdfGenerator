using FluentValidation;
using PdfGenerator.DTOs;
using PdfGenerator.Validators;
using QuestPDF.Helpers;

namespace PdfGenerator.Services;

interface IHttpHandlerService
{
  IResult GetResult<ParamType>(ParamType param, IValidator<ParamType> validator, PdfContent contentType = PdfContent.RandomSentences)
    where ParamType : IHasPageCount;
}
class HttpHandlerService : IHttpHandlerService
{
  IMeasurementService MeasurementService { get; }
  IGeneratorService DocumentGeneratorService { get; }

  public HttpHandlerService(IMeasurementService measurementService, IGeneratorService documentGeneratorService)
  {
    MeasurementService = measurementService;
    DocumentGeneratorService = documentGeneratorService;
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

  public IResult GetResult<ParamType>(ParamType generationParams, IValidator<ParamType> validator, PdfContent contentType = PdfContent.RandomSentences)
    where ParamType : IHasPageCount
  {
    var validationResult = validator.Validate(generationParams);
    if (validationResult.IsValid)
    {
      var size = GetPageSize(generationParams);
      var document = DocumentGeneratorService.Generate(size.Width, size.Height, generationParams.PageCount, contentType);
      return Results.Bytes(
        contents: document,
        contentType: "application/pdf",
        fileDownloadName: $"test_pdf_w{size.Width}_h{size.Height}_p{generationParams.PageCount}.pdf"
      );
    }
    return Results.ValidationProblem(validationResult.Errors.ToErrorMessageDict());
  }
}
