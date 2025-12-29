using System.Collections;
using System.IO;

using AuthDesk.Contracts.Services;
using AuthDesk.Core.Contracts.Services;
using AuthDesk.Models;

using Microsoft.Extensions.Options;

namespace AuthDesk.Services;

public class PersistAndRestoreService : IPersistAndRestoreService
{
    private readonly IFileService fileService;
    private readonly AppConfig appConfig;
    private readonly string localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

    public PersistAndRestoreService(IFileService fileService, IOptions<AppConfig> appConfig)
    {
        this.fileService = fileService;
        this.appConfig = appConfig.Value;
    }

    public void PersistData()
    {
        if (App.Current.Properties != null)
        {
            var folderPath = Path.Combine(localAppData, appConfig.ConfigurationsFolder);
            var fileName = appConfig.AppPropertiesFileName;
            fileService.Save(folderPath, fileName, App.Current.Properties);
        }
    }

    public void RestoreData()
    {
        var folderPath = Path.Combine(localAppData, appConfig.ConfigurationsFolder);
        var fileName = appConfig.AppPropertiesFileName;
        var properties = fileService.Read<IDictionary>(folderPath, fileName);
        if (properties != null)
        {
            foreach (DictionaryEntry property in properties)
            {
                App.Current.Properties.Add(property.Key, property.Value);
            }
        }
    }
}
