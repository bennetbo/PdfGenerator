namespace PdfGenerator.DTOs;

public interface IHasPageCount
{
  int PageCount { get; }
}

public record ExplicitGenerationParams(int PageCount, int? Width, int? Height) : IHasPageCount;

public record PageSizeGenerationParams(int PageCount, string? PageSize) : IHasPageCount;
