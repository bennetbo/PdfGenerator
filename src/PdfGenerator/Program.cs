using FluentValidation;
using PdfGenerator.DTOs;
using PdfGenerator.Models;
using PdfGenerator.Services;
using PdfGenerator.Validators;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer()
  .AddSwaggerGen()
  .AddSingleton<IPdfContentService<PdfPageContent>, PageContentService>()
  .AddSingleton<IPdfContentService<PdfFooterContent>, FooterContentService>()
  .AddSingleton<IGeneratorService, GeneratorService>()
  .AddSingleton<IMeasurementService, MeasurementService>()
  .AddSingleton<IHttpHandlerService, HttpHandlerService>()
  .AddScoped<IValidator<PageSizeGenerationParams>, PagesizeValidator>()
  .AddScoped<IValidator<ExplicitGenerationParams>, ExplicitParamsValidator>();

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
  => handlerService.GetResult(new(pages, pagesize), validator))
  .WithName("GenerateByPagesize").ProducesValidationProblem().Produces(200);

app.MapGet("/generate/{width:int}/{height:int}/{pages:int}", (int width, int height, int pages, IHttpHandlerService handlerService, IValidator<ExplicitGenerationParams> validator)
  => handlerService.GetResult(new(pages, width, height), validator))
  .WithName("GenerateByExplicitSize").ProducesValidationProblem().Produces(200);

app.MapGet("/generate/imaged/{pagesize}/{pages:int}", (string pagesize, int pages, IHttpHandlerService handlerService, IValidator<PageSizeGenerationParams> validator)
  => handlerService.GetResult(new(pages, pagesize), validator, PdfPageContent.Images))
  .WithName("GenerateImagedByPagesize").ProducesValidationProblem().Produces(200);

app.MapGet("/generate/imaged/{width:int}/{height:int}/{pages:int}", (int width, int height, int pages, IHttpHandlerService handlerService, IValidator<ExplicitGenerationParams> validator)
  => handlerService.GetResult(new(pages, width, height), validator, PdfPageContent.Images))
  .WithName("GenerateImagedByExplicitSize").ProducesValidationProblem().Produces(200); ;

app.MapGet("/generate/imaged/cats/{pagesize}/{pages:int}", (string pagesize, int pages, IHttpHandlerService handlerService, IValidator<PageSizeGenerationParams> validator)
  => handlerService.GetResult(new(pages, pagesize), validator, PdfPageContent.CatImages))
  .WithName("GenerateCatImagedByPagesize").ProducesValidationProblem().Produces(200);

app.MapGet("/generate/imaged/cats/{width:int}/{height:int}/{pages:int}", (int width, int height, int pages, IHttpHandlerService handlerService, IValidator<ExplicitGenerationParams> validator)
  => handlerService.GetResult(new(pages, width, height), validator, PdfPageContent.CatImages))
  .WithName("GenerateCatImagedByExplicitSize").ProducesValidationProblem().Produces(200); ;

app.MapGet("/generate/empty/{pagesize}/{pages:int}", (string pagesize, int pages, IHttpHandlerService handlerService, IValidator<PageSizeGenerationParams> validator)
  => handlerService.GetResult(new(pages, pagesize), validator, PdfPageContent.Empty))
  .WithName("EmptyByPagesize").ProducesValidationProblem().Produces(200);

app.MapGet("/generate/empty/{width:int}/{height:int}/{pages:int}", (int width, int height, int pages, IHttpHandlerService handlerService, IValidator<ExplicitGenerationParams> validator)
  => handlerService.GetResult(new(pages, width, height), validator, PdfPageContent.Empty))
  .WithName("EmptyByExplicitSize").ProducesValidationProblem().Produces(200); ;


app.Run();
