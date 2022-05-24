using PdfGenerator.Services;
using QuestPDF.Drawing;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.Diagnostics.CodeAnalysis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IDocumentGeneratorService, DocumentGeneratorService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var pdfSizes = new PdfSizes();

app.MapGet("/listPageSizes", () =>
{
  var content = string.Join(Environment.NewLine, pdfSizes.AvailableSizes);
  return Results.Text(content);
});


app.MapGet("/generate/{pagesize}/{pages}", (string pagesize, int pages, IDocumentGeneratorService documentGeneratorService) =>
{
  if (!pdfSizes.TryMatchSize(pagesize, out var size))
    return Results.BadRequest();

  var document = documentGeneratorService.CreateDocument(new(size.Width, size.Height, pages));
  var pdfData = document.GeneratePdf();
  return Results.Bytes(pdfData, contentType: "application/pdf");
});

app.MapGet("/generate/{width}/{height}/{pages}", (int width, int height, int pages, IDocumentGeneratorService documentGeneratorService) =>
{
  var document = documentGeneratorService.CreateDocument(new(width, height, pages));
  var pdfData = document.GeneratePdf();
  return Results.Bytes(pdfData, contentType: "application/pdf");
});

app.Run();

public record PdfGenerationData(float Width, float Height, int PageCount);

public sealed class PdfSizes
{
  private readonly Dictionary<string, PageSize> _pageSizes;

  public IEnumerable<string> AvailableSizes => _pageSizes.Keys;

  public PdfSizes()
  {
    _pageSizes = new();
    var props = typeof(PageSizes)
      .GetProperties()
      .Select(c => (Name: c.Name.ToLower(), Size: c.GetValue(null) as PageSize))
      .Where(c => c.Size != null);

    foreach (var prop in props)
      _pageSizes[prop.Name] = prop.Size!;
  }

  public bool TryMatchSize(string requestSize, [NotNullWhen(true)] out PageSize? pageSize)
    => _pageSizes.TryGetValue(requestSize.ToLower(), out pageSize);
}
