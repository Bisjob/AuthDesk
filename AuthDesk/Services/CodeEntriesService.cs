using AuthDesk.Contracts.Services;
using AuthDesk.Core.Contracts.Services;
using AuthDesk.Core.Models;
using AuthDesk.Models;
using Microsoft.Extensions.Options;
using System.IO;

namespace AuthDesk.Services
{
    public class CodeEntriesService : ICodeEntriesService
	{
		private readonly IFileService fileService;
		private readonly AppConfig appConfig;
		private readonly string localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

		public CodeEntries Entries { get; private set; }

		public CodeEntriesService(IFileService fileService, IOptions<AppConfig> appConfig)
		{
			this.fileService = fileService;
			this.appConfig = appConfig.Value;

			var folderPath = Path.Combine(localAppData, this.appConfig.ConfigurationsFolder);
			var fileName = this.appConfig.AppEntriesFileName;

			Entries = fileService.Read<CodeEntries>(folderPath, fileName) ?? new CodeEntries();
		}


		public void SaveData()
		{
			var folderPath = Path.Combine(localAppData, this.appConfig.ConfigurationsFolder);
			var fileName = this.appConfig.AppEntriesFileName;

			fileService.Save(folderPath, fileName, Entries);
		}
	}
}
