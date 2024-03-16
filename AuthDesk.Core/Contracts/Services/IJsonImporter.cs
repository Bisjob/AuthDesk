using AuthDesk.Core.Models;

namespace AuthDesk.Core.Contracts.Services
{
	public interface IJsonImporter
	{
		IEnumerable<CodeEntry> OpenJsonAegis(string filePath);
	}
}
