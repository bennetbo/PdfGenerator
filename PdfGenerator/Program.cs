using FluentValidation;
using PdfGenerator.DTOs;
using PdfGenerator.Helper;
using PdfGenerator.Scoped;
using PdfGenerator.Services;

ResourceHelper.RegisterRequiredFonts();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services
  .AddEndpointsApiExplorer()
  .AddSwaggerGen()
  .AddSingleton<IConfigurationService, ConfigurationService>()
  .AddSingleton<IFilenameService, FileNameService>()
  .AddSingleton<IGeneratorService, GeneratorService>()
  .AddSingleton<IHttpHandlerService, HttpHandlerService>()
  .AddSingleton<IMeasurementService, MeasurementService>()
  .AddSingleton<IPdfContentService<PdfPageContent>, PageContentService>()
  .AddSingleton<IPdfContentService<PdfFooterContent>, FooterContentService>()
  .AddScoped<IValidator<PageSizeGenerationParams>, PageCountValidator>()
  .AddScoped<IValidator<ExplicitGenerationParams>, DocumentGenerationExplicitParamsValidator>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/listPageSizes", (IMeasurementService measurement) => Results.Json(measurement.AvailableSizes));

app.MapGet("/generate/{pagesize}/{pages:int}", (string pagesize, int pages, IHttpHandlerService handlerService, IValidator<PageSizeGenerationParams> validator)
  => handlerService.GetResult(new(pages, pagesize), pages, validator))
  .WithName("GenerateByPagesize").ProducesValidationProblem().Produces(200);

app.MapGet("/generate/{width:int}/{height:int}/{pages:int}", (int width, int height, int pages, IHttpHandlerService handlerService, IValidator<ExplicitGenerationParams> validator)
  => handlerService.GetResult(new(pages, width, height), pages, validator))
  .WithName("GenerateByExplicitSize").ProducesValidationProblem().Produces(200);

app.MapGet("/generate/imaged/{pagesize}/{pages:int}", (string pagesize, int pages, IHttpHandlerService handlerService, IValidator<PageSizeGenerationParams> validator)
  => handlerService.GetResult(new(pages, pagesize), pages, validator, PdfPageContent.Images))
  .WithName("GenerateImagedByPagesize").ProducesValidationProblem().Produces(200);

app.MapGet("/generate/imaged/{width:int}/{height:int}/{pages:int}", (int width, int height, int pages, IHttpHandlerService handlerService, IValidator<ExplicitGenerationParams> validator)
  => handlerService.GetResult(new(pages, width, height), pages, validator, PdfPageContent.Images))
  .WithName("GenerateImagedByExplicitSize").ProducesValidationProblem().Produces(200);

app.MapGet("/generate/imaged/cat/{pagesize}/{pages:int}", (string pagesize, int pages, IHttpHandlerService handlerService, IValidator<PageSizeGenerationParams> validator)
  => handlerService.GetResult(new(pages, pagesize), pages, validator, PdfPageContent.CatImages))
  .WithName("GenerateCatImagedByPagesize").ProducesValidationProblem().Produces(200);

app.MapGet("/generate/imaged/cat/{width:int}/{height:int}/{pages:int}", (int width, int height, int pages, IHttpHandlerService handlerService, IValidator<ExplicitGenerationParams> validator)
  => handlerService.GetResult(new(pages, width, height), pages, validator, PdfPageContent.CatImages))
  .WithName("GenerateCatImagedByExplicitSize").ProducesValidationProblem().Produces(200);

app.MapGet("/generate/empty/{pagesize}/{pages:int}", (string pagesize, int pages, IHttpHandlerService handlerService, IValidator<PageSizeGenerationParams> validator)
  => handlerService.GetResult(new(pages, pagesize), pages, validator, PdfPageContent.Empty))
  .WithName("EmptyByPagesize").ProducesValidationProblem().Produces(200);

app.MapGet("/generate/empty/{width:int}/{height:int}/{pages:int}", (int width, int height, int pages, IHttpHandlerService handlerService, IValidator<ExplicitGenerationParams> validator)
  => handlerService.GetResult(new(pages, width, height), pages, validator, PdfPageContent.Empty))
  .WithName("EmptyByExplicitSize").ProducesValidationProblem().Produces(200);

app.Run();
