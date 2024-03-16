namespace AuthDesk.Core.Models.Aegis
{
	// Temporary structure to help with deserialization
	public class AegisJsonStructure
	{
		public AegisDb Db { get; set; }
	}

	public class AegisDb
	{
		public List<AegisEntry> Entries { get; set; }
	}

	public class AegisEntry
	{
		public string Type { get; set; }
		public string Uuid { get; set; }
		public string Name { get; set; }
		public string Issuer { get; set; }
		public string Group { get; set; }
		public string Note { get; set; }
		public bool Favorite { get; set; }
		public string Icon { get; set; }
		public AegisInfo Info { get; set; }
	}

	public class AegisInfo
	{
		public string Secret { get; set; }
		public string Algo { get; set; }
		public int? Digits { get; set; }
		public decimal? Period { get; set; }
	}
}
