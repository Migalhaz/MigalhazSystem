using UnityEngine;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text;
using System;
using System.IO;
using System.Runtime.InteropServices.ComTypes;
using MigalhaSystem.Extensions;

namespace MigalhaSystem
{
	public static class FileManagement
	{

        const string k_KEY = "wPqdMcHljKsL7ZWV097Eb4YQSMizoEVC1w2jciXUJdY=";
        const string k_IV = "oGbTa0CBP+2TZvUdk7aAvQ==";
        public static bool WriteData(string data, string path)
		{
			try
			{
				if (File.Exists(path))
				{
#if DEBUG
					Debug.LogWarning("Data exists. Deleting old file and writing a new one.");
#endif
					File.Delete(path);
				}
				else
				{
#if DEBUG
					Debug.Log("Writing new file!");
#endif
				}

                using FileStream stream = File.Create(path);

				//Criptografia =>>>>
				using Aes aesProvider = ProvideAes();
                using ICryptoTransform cryptoTransform = aesProvider.CreateEncryptor();
                using CryptoStream cryptoStream = new(stream, cryptoTransform, CryptoStreamMode.Write);
                cryptoStream.Write(Encrypt(data));
                return true;
			}
            catch (Exception e)
            {
#if DEBUG
                Debug.LogError($"Unable to save data due to: {e.Message} {e.StackTrace}");
#endif
                return false;
            }
        }

		static Aes ProvideAes()
		{
            Aes aesProvider = Aes.Create();
            aesProvider.Key = Convert.FromBase64String(k_KEY);
            aesProvider.IV = Convert.FromBase64String(k_IV);
			return aesProvider;
        }
		static byte[] Encrypt(string data) => Encoding.ASCII.GetBytes(data);

        public static string ReadData(string path)
		{
            byte[] fileBytes = File.ReadAllBytes(path);

			using Aes aesProvider = ProvideAes();
            using ICryptoTransform cryptoTransform = aesProvider.CreateDecryptor(aesProvider.Key, aesProvider.IV);
            using MemoryStream decryptorStream = new(fileBytes);
            using CryptoStream cryptoStream = new(decryptorStream, cryptoTransform, CryptoStreamMode.Read);

            using StreamReader reader = new StreamReader(cryptoStream);
			return reader.ReadToEnd();
        }
	}

	public static class SaveJsonConverter
	{
		public static string ToJson(object obj) => JsonConvert.SerializeObject(obj);
		public static T FromJson<T>(string json) => JsonConvert.DeserializeObject<T>(json);
    }

	[System.Serializable]
	public class SaveController<T>
	{
		[SerializeField] string m_path;
		[SerializeField] T m_saveObject;

		public void Save()
		{
			FileManagement.WriteData(JsonData(), GetPath());
		}

		public void Load()
		{
			m_saveObject = SaveJsonConverter.FromJson<T>(Reader());
		}

		string GetPath()
		{
			return m_path; 
		}

		string JsonData() => SaveJsonConverter.ToJson(m_saveObject);
        string Reader() => FileManagement.ReadData(GetPath());
	}

	public static class DirectoryManager
	{

	}
}