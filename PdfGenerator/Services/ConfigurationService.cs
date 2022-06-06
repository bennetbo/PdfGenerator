namespace PdfGenerator.Services
{
  interface IAppConfig
  {
    string FileNamingSchema { get; }
  }
  interface IConfigurationService
  {
    IAppConfig GetAppConfig();
  }

  class ConfigurationService : IConfigurationService
  {
    record AppConfig(string FileNamingSchema) : IAppConfig { }

    public IAppConfig GetAppConfig() => new AppConfig(
      Environment.GetEnvironmentVariable("FILE_NAMING_SCHEMA") ?? "dummy_w{{PAGE_WIDTH}}_h{{PAGE_HEIGHT}}_p{{PAGE_COUNT}}.pdf"
    );
  }
}
