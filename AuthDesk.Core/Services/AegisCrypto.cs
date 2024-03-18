using System.Security.Cryptography;
using Norgerman.Cryptography.Scrypt;

namespace AuthDesk.Core.Services
{
	public class AegisCrypto
	{

		public static byte[] DeriveKey(
				string password,
				byte[] salt,
				int n,
				int r,
				int p
		) {
			return ScryptUtil.Scrypt(
				password: password,
				salt: salt,
				N: n,
				r: r,
				p: p,
				dkLen: 32
			);
		}

		public static byte[] Decrypt(
				byte[] ciphertext,
				byte[] key,
				byte[] nonce,
				byte[] tag
		) {
			using var aesGcm = new AesGcm(key, tagSizeInBytes: 16);
			byte[] plaintext = new byte[ciphertext.Length];
			aesGcm.Decrypt(nonce, ciphertext, tag, plaintext, null);
			return plaintext;
		}
	}
}
