using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace PdfGenerator.Services
{
  interface IMeasurementService
  {
    int MaxWidth { get; }
    int MaxHeight { get; }
    int MinWidth { get; }
    int MinHeight { get; }
    IEnumerable<string> AvailableSizes { get; }
    bool IsValidWidth(int width);
    bool IsValidHeight(int width);
    bool IsValidPageSize(string requestSize);
    bool IsValidSizeParams(int width, int height);
    PageSize GetValidPageSize(string requestSize, string defaultValue = "a4");
    PageSize GetPageSizeOrDefault(int width, int height, PageSize? defaultPageSize = null);
  }

  class MeasurementService : IMeasurementService
  {
    private readonly Dictionary<string, PageSize> _pageSizes;

    public IEnumerable<string> AvailableSizes => _pageSizes.Keys;
    public int MaxWidth { get; } = (int)Size.Max.Width;
    public int MaxHeight { get; } = (int)Size.Max.Height;
    public int MinWidth { get; } = (int)Size.Zero.Width;
    public int MinHeight { get; } = (int)Size.Zero.Height;

    public MeasurementService()
    {
      _pageSizes = new();
      var props = typeof(PageSizes)
        .GetProperties()
        .Select(c => (Name: c.Name.ToLower(), Size: c.GetValue(null) as PageSize))
        .Where(c => c.Size != null);

      foreach (var (Name, Size) in props)
        _pageSizes[Name] = Size!;
    }

    public bool IsValidPageSize(string? requestSize) => requestSize != null && _pageSizes.ContainsKey(requestSize!.ToLower());

    public bool IsValidWidth(int width) => width <= MaxWidth && width >= MinWidth;
    public bool IsValidHeight(int height) => height >= MinHeight && height <= MaxHeight;
    public bool IsValidSizeParams(int width, int height) => IsValidHeight(height) && IsValidWidth(width);
    public PageSize GetValidPageSize(string requestSize, string defaultValue = "a4")
      => _pageSizes.ContainsKey(requestSize?.ToLower() ?? string.Empty) ? _pageSizes[requestSize!] : _pageSizes[defaultValue];
    public PageSize GetPageSizeOrDefault(int width, int height, PageSize? defaultPageSize = null)
    {
      var valid = IsValidSizeParams(width, height);
      return valid ? new PageSize(width, height) : defaultPageSize ?? PageSizes.A4;
    }
  }
}