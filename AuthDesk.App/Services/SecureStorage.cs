using AuthDesk.Contracts.Services;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace AuthDesk.Services
{
    public class SecureStorage : ISecureStorage
    {
        JsonSerializerOptions jsonOptions = new JsonSerializerOptions();

        public SecureStorage()
        {
        }

        public void Save<T>(T data, string filename)
        {
            var json = JsonSerializer.Serialize(data, jsonOptions);
            byte[] dataToEncrypt = Encoding.Unicode.GetBytes(json);
            byte[] encryptedData = ProtectedData.Protect(dataToEncrypt, null, DataProtectionScope.CurrentUser);

            File.WriteAllBytes(filename, encryptedData);
        }

        public T Read<T>(string filename)
        {
            if (!File.Exists(filename))
            {
                throw new FileNotFoundException("The encrypted data file was not found.", filename);
            }

            byte[] encryptedData = File.ReadAllBytes(filename);
            byte[] decryptedData = ProtectedData.Unprotect(encryptedData, null, DataProtectionScope.CurrentUser);
            var json = Encoding.Unicode.GetString(decryptedData);

            return JsonSerializer.Deserialize<T>(json, jsonOptions);
        }
    }
}
