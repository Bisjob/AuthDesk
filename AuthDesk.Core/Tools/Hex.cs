namespace AuthDesk.Core.Tools
{
	public class Hex {
		public static byte[] ToBytes(String hex)
		{
			return Convert.FromHexString(hex);
		}

		public static string FromBytes(byte[] bytes)
		{
			return Convert.ToHexString(bytes).ToLower();
		}
	}
}
