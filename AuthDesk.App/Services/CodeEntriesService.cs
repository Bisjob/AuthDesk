using AuthDesk.Contracts.Services;
using AuthDesk.Core.Models;
using AuthDesk.Models;
using Microsoft.Extensions.Options;
using System.IO;

namespace AuthDesk.Services
{
    public class CodeEntriesService : ICodeEntriesService
	{
		private readonly ISecureStorage secureStorage;
		private readonly AppConfig appConfig;
        private readonly string localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

        public CodeEntries Entries { get; private set; }

		public CodeEntriesService(ISecureStorage secureStorage, IOptions<AppConfig> appConfig)
		{
			this.secureStorage = secureStorage;
			this.appConfig = appConfig.Value;

            var fileName = Path.Combine(localAppData, this.appConfig.ConfigurationsFolder, this.appConfig.AppEntriesFileName);

            try
			{
                Entries = secureStorage.Read<CodeEntries>(fileName);
            }
			catch (FileNotFoundException)
			{
                Entries = new CodeEntries();
            }
			catch (Exception e)
			{
				// Error while loading file
				throw;
			}
		}


		public void SaveData()
		{
            var fileName = Path.Combine(localAppData, this.appConfig.ConfigurationsFolder, this.appConfig.AppEntriesFileName);
			try
            {
                secureStorage.Save(Entries, fileName);
			}
			catch (Exception e)
            {
                // Error while saving file
                throw;
			}
		}
	}
}
