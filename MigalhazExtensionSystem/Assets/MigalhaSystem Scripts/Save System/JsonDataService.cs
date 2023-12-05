using MigalhaSystem.Extensions;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

namespace MigalhaSystem.SaveSystem
{
    public class JsonDataService : IDataService
    {
        const string m_KEY = "wPqdMcHljKsL7ZWV097Eb4YQSMizoEVC1w2jciXUJdY=";
        const string m_IV = "oGbTa0CBP+2TZvUdk7aAvQ==";

        public bool SaveData<T>(string _relativePath, T _data, bool _encrypted)
        {
            string relativePath = NormalizeRelativePath(_relativePath);

            string path = Application.persistentDataPath + relativePath;

            try
            {
                if (File.Exists(path))
                {
                    Debug.LogWarning("Data exists. Deleting old file and writing a new one.");
                    File.Delete(path);
                }
                else
                {
                    Debug.Log("Writing new file!");
                }
                using FileStream stream = File.Create(path);

                if (_encrypted)
                {
                    WriteEncryptedData(_data, stream);
                }
                else
                {
                    stream.Close();
                    File.WriteAllText(path, JsonConvert.SerializeObject(_data));
                }
                       
                
                return true;
            }
            catch(Exception e)
            {
                Debug.LogError($"Unable to save data due to: {e.Message} {e.StackTrace}");
                return false;
            }
        }

        void WriteEncryptedData<T>(T _data, FileStream _stream)
        {
            using Aes aesProvider = Aes.Create();
            aesProvider.Key = Convert.FromBase64String(m_KEY);
            aesProvider.IV = Convert.FromBase64String(m_IV);

            //Debug.Log("KEY:" + $"{Convert.ToBase64String(aesProvider.Key)}".Bold().Color(Color.green));
            //Debug.Log("IV:" + $"{Convert.ToBase64String(aesProvider.IV)}".Bold().Color(Color.red));

            using ICryptoTransform cryptoTransform = aesProvider.CreateEncryptor();
            using CryptoStream cryptoStream = new(_stream, cryptoTransform, CryptoStreamMode.Write);
            cryptoStream.Write(Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(_data)));
        }


        public T LoadData<T>(string _relativePath, bool _encrypted)
        {
            string relativePath = NormalizeRelativePath(_relativePath);

            string path = Application.persistentDataPath + relativePath;


            if (!File.Exists(path))
            {
                Debug.LogError($"Cannot load file at {path}! File does not exist!".Error());
                throw new FileNotFoundException($"{path} does not exist!");
            }

            try
            {
                T data;

                if (_encrypted)
                {
                    data =  ReadEncryptedData<T>(path);
                }
                else
                {
                    data = JsonConvert.DeserializeObject<T>(File.ReadAllText(path));
                }
                
                return data;
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to load data due to: {e.Message} {e.StackTrace}");
                throw e;
            }
        }

        T ReadEncryptedData<T>(string _path)
        {
            byte[] fileBytes = File.ReadAllBytes(_path);
            using Aes aesProvider = Aes.Create();
            aesProvider.Key = Convert.FromBase64String(m_KEY);
            aesProvider.IV = Convert.FromBase64String(m_IV);

            using ICryptoTransform cryptoTransform = aesProvider.CreateDecryptor(aesProvider.Key, aesProvider.IV);
            using MemoryStream decryptorStream = new(fileBytes);
            using CryptoStream cryptoStream = new(decryptorStream, cryptoTransform, CryptoStreamMode.Read);

            using StreamReader reader = new StreamReader(cryptoStream);

            string result = reader.ReadToEnd();

            Debug.Log("Decrypt result  (if the following is not legible, probably wrong KEY or IV)! \n" + result.Color(Color.red).Bold());
            return JsonConvert.DeserializeObject<T>(result);
        }

        string NormalizeRelativePath(string _relativePath)
        {
            string jsonExtension = ".json";
            string normalizePath = _relativePath;

            if (!normalizePath.StartsWith('/'))
            {
                normalizePath = $"/{normalizePath}";
            }

            if (!normalizePath.EndsWith(jsonExtension))
            {
                normalizePath = $"{normalizePath}{jsonExtension}";
            }

            return normalizePath;
        }

        
    }
}