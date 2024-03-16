using AuthDesk.Core.Enums;

namespace AuthDesk.Core.Models
{
	public class CodeEntry
	{
		public string Id { get; set; }
		public string Name { get; set; }
		public string Issuer { get; set; }
		public string Group { get; set; }
		public string Note { get; set; }
		public string Icon { get; set; }

		public PassType Type { get; set; }

		public CodeEntryInfo Info { get; set; }
	}

	public class CodeEntryInfo
	{
		public string Secrets { get; set; }
		
		public AlgoType AlgoType { get; set; }

		public int Digits { get;set; }
		public decimal Period { get; set; }
	}
}
