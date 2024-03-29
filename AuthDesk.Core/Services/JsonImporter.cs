﻿using AuthDesk.Core.Contracts.Services;
using AuthDesk.Core.Enums;
using AuthDesk.Core.Models;
using AuthDesk.Core.Models.Aegis;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static AuthDesk.Core.Contracts.Services.IJsonImporter;

namespace AuthDesk.Core.Services
{
	public class JsonImporter : IJsonImporter
    {
        public event AsyncPasswordEventHandler OnAskForPassword;
        public JsonImporter() 
		{
		}

		public async Task<IEnumerable<CodeEntry>> OpenJsonAegis(string filePath)
		{
			if (!File.Exists(filePath))
				return [];

			var json = File.ReadAllText(filePath);
			var temp = JsonConvert.DeserializeObject<AegisJsonStructure>(json);
			AegisDb db = null;

			if (temp.Db is string)
			{
				// `db` is encrypted.
				var handler = OnAskForPassword;
				if (handler == null)
                    return [];

                var passwordArgs = new AsyncPasswordEventArgs();
				await OnAskForPassword?.Invoke(this, passwordArgs);
				if (string.IsNullOrEmpty(passwordArgs.Password))
                    return [];

                db = HandleEncryptedDb(temp, passwordArgs.Password);
			}
			else
			{
				// `db` is in plain text.
				db = (AegisDb)((JObject)temp.Db).ToObject(typeof(AegisDb));
			}

			return db.Entries.Select(e => new CodeEntry
			{
				Id = e.Uuid,
				Name = e.Name,
				Issuer = e.Issuer,
				Group = e.Group,
				Note = e.Note,
				Icon = e.Icon,
				Type = Enum.TryParse<PassType>(e.Type, true, out var passType) ? passType : default,
				Info = new CodeEntryInfo
				{
					Secrets = e.Info?.Secret,
					AlgoType = Enum.TryParse<AlgoType>(e.Info?.Algo, true, out var algoType) ? algoType : default,
					Digits = e.Info?.Digits ?? 0,
					Period = e.Info?.Period ?? 0,
				}
			});
        }


        private AegisDb HandleEncryptedDb(AegisJsonStructure file, string password)
        {
            var slot = file.Header.Slots[0];  // Caveat: This doesn't support multiple slots.
            var slotKey = AegisCrypto.DeriveKey(
                    password: password,
                    salt: slot.Salt,
                    n: slot.N,
                    r: slot.R,
                    p: slot.P
            );
            var masterKey = AegisCrypto.Decrypt(
                    ciphertext: slot.Key,
                    key: slotKey,
                    nonce: slot.KeyParams.Nonce,
                    tag: slot.KeyParams.Tag
            );

            var dbEncrypted = Convert.FromBase64String((string)file.Db);
            var dbPlain = AegisCrypto.Decrypt(
                    ciphertext: dbEncrypted,
                    key: masterKey,
                    nonce: file.Header.Params.Nonce,
                    tag: file.Header.Params.Tag
            );
            string dbString = System.Text.Encoding.UTF8.GetString(dbPlain);
            return JsonConvert.DeserializeObject<AegisDb>(dbString);
        }
    }
}
