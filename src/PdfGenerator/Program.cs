using FluentValidation;
using PdfGenerator.DTOs;
using PdfGenerator.Services;
using PdfGenerator.Validators;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer()
  .AddSwaggerGen()
  .AddSingleton<IPageContentService, PageContentService>()
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
  => handlerService.GetResult(new(pages, pagesize), validator, PdfContent.Images))
  .WithName("GenerateImagedByPagesize").ProducesValidationProblem().Produces(200);

app.MapGet("/generate/imaged/{width:int}/{height:int}/{pages:int}", (int width, int height, int pages, IHttpHandlerService handlerService, IValidator<ExplicitGenerationParams> validator)
  => handlerService.GetResult(new(pages, width, height), validator, PdfContent.Images))
  .WithName("GenerateImagedByExplicitSize").ProducesValidationProblem().Produces(200); ;

app.MapGet("/generate/imaged/cats/{pagesize}/{pages:int}", (string pagesize, int pages, IHttpHandlerService handlerService, IValidator<PageSizeGenerationParams> validator)
  => handlerService.GetResult(new(pages, pagesize), validator, PdfContent.CatImages))
  .WithName("GenerateCatImagedByPagesize").ProducesValidationProblem().Produces(200);

app.MapGet("/generate/imaged/cats/{width:int}/{height:int}/{pages:int}", (int width, int height, int pages, IHttpHandlerService handlerService, IValidator<ExplicitGenerationParams> validator)
  => handlerService.GetResult(new(pages, width, height), validator, PdfContent.CatImages))
  .WithName("GenerateCatImagedByExplicitSize").ProducesValidationProblem().Produces(200); ;

app.MapGet("/generate/empty/{pagesize}/{pages:int}", (string pagesize, int pages, IHttpHandlerService handlerService, IValidator<PageSizeGenerationParams> validator)
  => handlerService.GetResult(new(pages, pagesize), validator, PdfContent.Empty))
  .WithName("EmptyByPagesize").ProducesValidationProblem().Produces(200);

app.MapGet("/generate/empty/{width:int}/{height:int}/{pages:int}", (int width, int height, int pages, IHttpHandlerService handlerService, IValidator<ExplicitGenerationParams> validator)
  => handlerService.GetResult(new(pages, width, height), validator, PdfContent.Empty))
  .WithName("EmptyByExplicitSize").ProducesValidationProblem().Produces(200); ;


app.Run();
