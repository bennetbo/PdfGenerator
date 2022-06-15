namespace PdfGenerator.Services
{
  public interface IAppConfig
  {
    string FileNamingSchema { get; }
  }

  public interface IConfigurationService
  {
    IAppConfig GetAppConfig();
  }

  public class ConfigurationService : IConfigurationService
  {
    private record AppConfig(string FileNamingSchema) : IAppConfig { }

    public IAppConfig GetAppConfig() => new AppConfig(
      Environment.GetEnvironmentVariable("FILE_NAMING_SCHEMA") ?? "dummy_w{{PAGE_WIDTH}}_h{{PAGE_HEIGHT}}_p{{PAGE_COUNT}}.pdf"
    );
  }
}
