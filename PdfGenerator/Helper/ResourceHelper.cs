using PdfGenerator.Properties;
using QuestPDF.Drawing;

namespace PdfGenerator.Helper;

public static class ResourceHelper
{
  public static void RegisterRequiredFonts()
  {
    RegisterFont(Resources.calibri_regular);
  }

  public static void RegisterFont(byte[] resource)
  {
    using var stream = new MemoryStream(resource);
    FontManager.RegisterFont(stream);
  }
}
