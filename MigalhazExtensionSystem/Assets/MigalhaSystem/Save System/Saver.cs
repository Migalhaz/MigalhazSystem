using MigalhaSystem.Extensions;
using MigalhaSystem.SaveSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MigalhaSystem.SaveSystem
{
    public static class Saver<T>
    {
        public static void SaveData(T dataToSave, string path, bool encrypted)
        {
            IDataService dataService = new JsonDataService();
            long startTime = DateTime.Now.Ticks;

            if (dataService.SaveData(path, dataToSave, encrypted))
            {
                long saveTime = DateTime.Now.Ticks - startTime;
                Debug.Log($"The save operation was carried out successfully!".Color(Color.green) + $" Save Time: {(saveTime / TimeSpan.TicksPerMillisecond):N4}ms".Bold());
            }
            else
            {
                Debug.Log("The save operation was failed!".Error());
            }
        }

        public static T LoadData(string path, bool encrypted)
        {
            IDataService dataService = new JsonDataService();
            long startTime = DateTime.Now.Ticks;
            try
            {
                long loadTime = DateTime.Now.Ticks - startTime;
                T dataLoaded = dataService.LoadData<T>(path, encrypted);
                Debug.Log($"The load operation was carried out successfully!".Color(Color.green) + $" Load Time: {(loadTime / TimeSpan.TicksPerMillisecond):N4}ms".Bold());
                return dataLoaded;
            }
            catch (Exception e)
            {
                Debug.Log("The load operation was failed!".Error());
                throw e;
            }
        }
    }
}