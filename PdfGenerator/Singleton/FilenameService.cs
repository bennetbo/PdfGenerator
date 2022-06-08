using System.Diagnostics;
using System.Linq;

namespace PdfGenerator.Services
{
  public interface IFilenameService
  {
    string Replace(string configFileName, int pageWidth, int pageHeight, int paceIndex);
  }

  public class FileNameService : IFilenameService
  {
    const char PLACEHOLDER_OPEN_CHAR = '{';
    const char PLACEHOLDER_CLOSE_CHAR = '}';
    const string PAGE_WIDTH_PLACEHOLDER = "PAGE_WIDTH";
    const string PAGE_HEIGHT_PLACEHOLDER = "PAGE_HEIGHT";
    const string PAGE_COUNT_PLACEHOLDER = "PAGE_COUNT";

    const StringSplitOptions FILE_NAMING_SCHEMA_SPLIT_OPTIONS = StringSplitOptions.RemoveEmptyEntries & StringSplitOptions.TrimEntries;

    static string GetPlaceHolder(bool open) => open
      ? new(PLACEHOLDER_OPEN_CHAR, 2)
      : new(PLACEHOLDER_CLOSE_CHAR, 2);
    string PlaceholderOpen { get; } = GetPlaceHolder(open: true);
    string PlaceholderClose { get; } = GetPlaceHolder(open: false);
    string[] PlaceholderSeperators { get; } = new[] { GetPlaceHolder(open: true), GetPlaceHolder(open: false) };

    bool CanReplaceAll(string fileName, out string[] unknownVariables)
    {
      var validPlaceholders = new[] { PAGE_COUNT_PLACEHOLDER, PAGE_HEIGHT_PLACEHOLDER, PAGE_WIDTH_PLACEHOLDER }.ToList();
      unknownVariables = fileName.Split(PlaceholderSeperators, FILE_NAMING_SCHEMA_SPLIT_OPTIONS)
                                 .Where((value, index) => index % 2 == 1)
                                 .Where(part => !validPlaceholders.Contains(part))
                                 .ToArray();
      return unknownVariables.Length == 0;
    }
    
    bool HasValidDoubleBracketCount(string fileName)
     => fileName.Split(PLACEHOLDER_OPEN_CHAR, FILE_NAMING_SCHEMA_SPLIT_OPTIONS).Length.Equals(fileName.Split(PLACEHOLDER_CLOSE_CHAR, FILE_NAMING_SCHEMA_SPLIT_OPTIONS).Length);

    public string Replace(string fileName, int pageWidth, int pageHeight, int pageCount)
    {
      if (!HasValidDoubleBracketCount(fileName))
        throw new Exception($"invalid bracket count");
      else if (!CanReplaceAll(fileName, out string[] unknownVariables))
        throw new Exception($"invalid variables detected in fileschema: {string.Join("\n\t- ", unknownVariables)}");
      else
        return fileName.ReplaceImpl(PAGE_WIDTH_PLACEHOLDER, pageWidth, PlaceholderOpen, PlaceholderClose)
                       .ReplaceImpl(PAGE_HEIGHT_PLACEHOLDER, pageHeight, PlaceholderOpen, PlaceholderClose)
                       .ReplaceImpl(PAGE_COUNT_PLACEHOLDER, pageCount, PlaceholderOpen, PlaceholderClose);
    }
  }
  static class FileDownloadNameReplacementExt
  {
    public static string ReplaceImpl(this string original, string key, int value, string placeholderOpener, string placeholderCloser)
      => original.Replace($"{placeholderOpener + key + placeholderCloser}", value.ToString());
  }
}
