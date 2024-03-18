using AuthDesk.Core.Models;

namespace AuthDesk.Core.Contracts.Services
{
    public class AsyncPasswordEventArgs : EventArgs
    {
        public string Password { get; set; }
	}

	public interface IJsonImporter
	{
        public delegate Task AsyncPasswordEventHandler(object sender, AsyncPasswordEventArgs e);
        public event AsyncPasswordEventHandler OnAskForPassword;

        Task<IEnumerable<CodeEntry>> OpenJsonAegis(string filePath);
	}
}
