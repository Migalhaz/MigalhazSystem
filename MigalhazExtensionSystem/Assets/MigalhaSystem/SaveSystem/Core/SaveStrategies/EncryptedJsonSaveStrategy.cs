using System.IO;
using System.Security.Cryptography;
using System;
using UnityEngine;
using System.Text;
using MigalhaSystem.Extensions;

namespace MigalhaSystem.SaveSystem
{
    [CreateAssetMenu(fileName = "EncryptedJsonSaveStrategy", menuName = "Scriptable Object/Save System/Save Strategies/Encrypted Json Save Strategy")]
    public class EncryptedJsonSaveStrategy : AbstractSaveStrategy
    {
        const string k_KEY = "wPqdMcHljKsL7ZWV097Eb4YQSMizoEVC1w2jciXUJdY=";
        const string k_IV = "oGbTa0CBP+2TZvUdk7aAvQ==";

        public override bool WriteData(string data, string path)
        {
            try
            {
                using FileStream stream = FileManagement.CreateNewFile(path);
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

        public override string ReadData(string path)
        {
            if (!CheckDataFile(path)) return null;

            using Aes aesProvider = ProvideAes();
            using ICryptoTransform cryptoTransform = aesProvider.CreateDecryptor(aesProvider.Key, aesProvider.IV);
            using MemoryStream decryptorStream = new(Decrypt(path));
            using CryptoStream cryptoStream = new(decryptorStream, cryptoTransform, CryptoStreamMode.Read);

            using StreamReader reader = new StreamReader(cryptoStream);
            return reader.ReadToEnd();
        }

        Aes ProvideAes()
        {
            Aes aesProvider = Aes.Create();
            aesProvider.Key = Convert.FromBase64String(k_KEY);
            aesProvider.IV = Convert.FromBase64String(k_IV);
            return aesProvider;
        }

        byte[] Encrypt(string data) => Encoding.ASCII.GetBytes(data);
        byte[] Decrypt(string path) => File.ReadAllBytes(path);

        public override string GetFileExtension() => ".json";
    }
}