using AuthDesk.Core.Contracts.Services;
using AuthDesk.Core.Enums;
using AuthDesk.Core.Models;
using AuthDesk.Core.Models.Aegis;
using Newtonsoft.Json;

namespace AuthDesk.Core.Services
{
	public class JsonImporter : IJsonImporter
	{
		public JsonImporter() 
		{
		}

		public IEnumerable<CodeEntry> OpenJsonAegis(string filePath)
		{
			if (!File.Exists(filePath))
				return null;

			var json = File.ReadAllText(filePath);
			var temp = JsonConvert.DeserializeObject<AegisJsonStructure>(json);
			if (temp?.Db?.Entries == null) return Enumerable.Empty<CodeEntry>();

			return temp.Db.Entries.Select(e => new CodeEntry
			{
				Id = e.Uuid,
				Name = e.Name,
				Issuer = e.Issuer,
				Group = e.Group,
				Note = e.Note,
				Icon = e.Icon,
				Type = Enum.TryParse<PassType>(e.Type, true, out var passType) ? passType : default,
				Info = new CodeEntryInfo
				{
					Secrets = e.Info?.Secret,
					AlgoType = Enum.TryParse<AlgoType>(e.Info?.Algo, true, out var algoType) ? algoType : default,
					Digits = e.Info?.Digits ?? 0,
					Period = e.Info?.Period ?? 0,
				}
			});
		}
	}
}
