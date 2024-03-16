namespace AuthDesk.Core.Models
{
	public class CodeEntries
	{
		public string Id { get; set; }
		public ICollection<CodeEntry> Entries { get; set; } = new List<CodeEntry>();

	}
}
