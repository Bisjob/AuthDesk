using Newtonsoft.Json;

namespace AuthDesk.Core.Models.Aegis
{
	// Temporary structure to help with deserialization
	public class AegisJsonStructure
	{
		public AegisHeader Header { get; set; }
		public object Db { get; set; }
	}

	public class AegisHeader
	{
		public List<AegisSlot> Slots { get; set; }
		public AegisParams Params { get; set; }
	}

	public class AegisSlot
	{
		public int Type { get; set; }
		public string Uuid { get; set; }

		[JsonConverter(typeof(HexConverter))]
		public byte[] Key { get; set; }

		[JsonProperty("key_params")]
		public AegisParams KeyParams { get; set; }
		public int N { get; set; }
		public int R { get; set; }
		public int P { get; set; }

		[JsonConverter(typeof(HexConverter))]
		public byte[] Salt { get; set; }

		public string Repaired { get; set; }
	}

	public class AegisParams
	{
		[JsonConverter(typeof(HexConverter))]
		public byte[] Nonce { get; set; }

		[JsonConverter(typeof(HexConverter))]
		public byte[] Tag { get; set; }
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
