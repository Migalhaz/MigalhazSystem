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
        public static void SaveData(IDataService dataService, T dataToSave, string path, bool encrypted)
        {
            long startTime = DateTime.Now.Ticks;

            if (dataService.SaveData(path, dataToSave, encrypted))
            {
                long saveTime = DateTime.Now.Ticks - startTime;
#if DEBUG
                Debug.Log($"The save operation was carried out successfully!".Color(Color.green) + $" Save Time: {(saveTime / TimeSpan.TicksPerMillisecond):N4}ms".Bold());
#endif
            }
            else
            {
#if DEBUG
                Debug.Log("The save operation was failed!".Error());
#endif
            }
        }

        public static T LoadData(IDataService dataService, string path, bool encrypted)
        {
            long startTime = DateTime.Now.Ticks;
            try
            {
                long loadTime = DateTime.Now.Ticks - startTime;
                T dataLoaded = dataService.LoadData<T>(path, encrypted);
#if DEBUG
                Debug.Log($"The load operation was carried out successfully!".Color(Color.green) + $" Load Time: {(loadTime / TimeSpan.TicksPerMillisecond):N4}ms".Bold());
#endif
                return dataLoaded;
            }
            catch (Exception e)
            {
#if DEBUG
                Debug.Log("The load operation was failed!".Error());
#endif
                throw e;
            }
        }
    }
}