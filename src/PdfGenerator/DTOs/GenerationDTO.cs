namespace PdfGenerator.DTOs;

public record GenerationDTO(float Width, float Height, int PageCount);

public enum PdfContent
{
  Empty,
  RandomSentences,
  CatImages,
  Images
}