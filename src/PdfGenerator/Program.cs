using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using QuestPDF.Previewer;

static IDocument CreateDocument(PdfGenerationData data)
{
  return Document.Create(c =>
  {
    c.Page(p =>
    {
      p.Margin(50);
      p.Size(data.Width, data.Height, Unit.Point);

      p.Content()
       .DefaultTextStyle(t => t.FontSize(30))
       .Column(c =>
       {
         foreach (var i in Enumerable.Range(0, data.PageCount))
         {
           c.Item().Text("Hallo :)");
           if (i < data.PageCount - 1)
             c.Item().PageBreak();
         }
       });
      p.Footer().AlignCenter().Text(t =>
      {
        t.CurrentPageNumber();
        t.Span(" / ");
        t.TotalPages();
      });
    });
  });
}

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/generate/{width}/{height}/{pages}", (int width, int height, int pages) =>
{
  var document = CreateDocument(new(width, height, pages));
  var pdfData = document.GeneratePdf();
  return Results.Bytes(pdfData, contentType: "application/pdf");
});

app.Run();

public record PdfGenerationData(int Width, int Height, int PageCount);
