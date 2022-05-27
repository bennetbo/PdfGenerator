using PdfGenerator.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer()
  .AddSwaggerGen()
  .AddSingleton<IGeneratorService, GeneratorService>()
  .AddSingleton<IMeasurementService, MeasurementService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/listPageSizes", (IMeasurementService measurement) => Results.Json(measurement.AvailableSizes));

app.MapGet("/generate/{pagesize}/{pages:int}", (string pagesize, int pages, IGeneratorService documentGeneratorService, IMeasurementService measurement) =>
{
  if (!measurement.TryMatchSize(pagesize, out var size) || size == null)
    return Results.BadRequest();

  var document = documentGeneratorService.Generate(new(size.Width, size.Height, pages));
  if (document == null)
    return Results.StatusCode(500);

  return Results.Bytes(
    contents: document,
    contentType: "application/pdf",
    fileDownloadName: $"test_pdf_w{size.Width}_h{size.Height}_p{pages}.pdf"
  );
});

app.MapGet("/generate/{width:int}/{height:int}/{pages:int}", (int width, int height, int pages, IGeneratorService documentGeneratorService, IMeasurementService measurement) =>
{
  if (!measurement.TryCreateValidSize(width, height, out var size))
    return Results.BadRequest();

  var document = documentGeneratorService.Generate(new(width, height, pages));
  if (document == null)
    return Results.StatusCode(500);

  return Results.Bytes(
    contents: document,
    contentType: "application/pdf",
    fileDownloadName: $"test_pdf_w{size.Width}_h{size.Height}_p{pages}.pdf"
  );
});

app.Run();
