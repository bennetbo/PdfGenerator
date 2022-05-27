using QuestPDF.Helpers;
using System.Diagnostics.CodeAnalysis;

namespace PdfGenerator.Services
{
  interface IMeasurementService
  {
    int MaxWidth { get; }
    int MaxHeight { get; }
    int MinWidth { get; }
    int MinHeight { get; }
    IEnumerable<string> AvailableSizes { get; }
    bool TryMatchSize(string requestSize, out PageSize? pageSize);
    bool TryCreateValidSize(int width, int height, out PageSize? pageSize);
  }

  class MeasurementService : IMeasurementService
  {
    private readonly Dictionary<string, PageSize> _pageSizes;

    public IEnumerable<string> AvailableSizes => _pageSizes.Keys;
    public int MaxWidth { get; } = 10000;
    public int MaxHeight { get; } = 10000;
    public int MinWidth { get; } = 0;
    public int MinHeight { get; } = 0;

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

    public bool TryMatchSize(string requestSize, [NotNullWhen(true)] out PageSize? pageSize)
      => _pageSizes.TryGetValue(requestSize.ToLower(), out pageSize);

    public bool TryCreateValidSize(int width, int height, [NotNull] out PageSize? pageSize)
    {
      var valid = width >= MinWidth && height >= MinHeight && width <= MaxWidth && height <= MaxHeight;
      pageSize = valid ? new PageSize(width, height) : PageSizes.A4;
      return valid;
    }
  }
}

