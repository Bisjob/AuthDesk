using AuthDesk.Core.Models;

namespace AuthDesk.Core.Contracts.Services
{
	public interface IJsonImporter
	{
		object OpenJsonAegis(string filePath, string password);
	}
}
