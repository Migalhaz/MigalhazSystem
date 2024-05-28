using MigalhaSystem.Extensions;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.IO;

namespace MigalhaSystem.Analysis
{
	public class Benchmarker : MonoBehaviour
	{
#if UNITY_EDITOR
        public int m_Iteration;
        public List<BenchmarkAction> m_Actions;

        [Header("Log Settings")]
        public bool m_GenerateLog = false;
        public string m_LogPath;
        public string m_LogTextFileName;
        public bool m_OpenFileOnWrite;

        public bool m_LogTime = true;
        public bool m_LogGameObjectName = true;
        public bool m_LogIterationsCount = true;
        public bool m_LogMessage = false;
        public string m_Message;

        public void Execute()
        {
            if (m_Actions == null || m_Actions.Count <= 0) return;

            string log = LogLabel();
            foreach (BenchmarkAction action in m_Actions)
            {
                Benchmark.TimeExecution(() => ActionExecution(action),out string result, label: action.m_Label);
                log += $"{result}\n";
            }
            if (m_GenerateLog) GenerateLog(log);
        }

        void GenerateLog(string log)
        {
            string applicationFile = $"{m_LogPath}/{Application.productName}";

            if (!Directory.Exists(applicationFile))
            {
                Directory.CreateDirectory(applicationFile);
            }

            string logFileName = $"{applicationFile}/Log";
            if (!Directory.Exists(logFileName))
            {
                Directory.CreateDirectory(logFileName);
            }

            m_LogTextFileName = string.IsNullOrEmpty(m_LogTextFileName) ? gameObject.name : m_LogTextFileName;
            string textFileName = $"{logFileName}/{m_LogTextFileName}.txt";
            if (!File.Exists(textFileName))
            {
                using FileStream stream = File.Create(textFileName);
                stream.Close();
            }
            File.AppendAllText(textFileName, log + System.Environment.NewLine);

            if (m_OpenFileOnWrite)
            {
                System.Diagnostics.Process.Start(textFileName);
            }
        }

        string LogLabel()
        {
            string log = "[";
            string logTime = $"{System.DateTime.Now}";
            string logObjName = gameObject.name;
            string logIterations = $"{m_Iteration}";
            string logMessage = m_Message;

            if (m_LogTime)
            {
                if (log[log.Length - 1] != '[') log += " ";
                log += logTime;
            }
            if (m_LogGameObjectName)
            {
                if (log[log.Length - 1] != '[') log += " ";
                log += logObjName;
            }
            if (m_LogIterationsCount)
            {
                if (log[log.Length - 1] != '[') log += " ";
                log += logIterations;
            }
            if (m_LogMessage)
            {
                if (log[log.Length - 1] != '[') log += " ";
                log += logMessage;
            }
            log += "]\n";
            if (log == "[]\n") log = string.Empty;
            return log;
        }

        public void DeleteLog()
        {
            string applicationFile = $"{m_LogPath}/{Application.productName}";

            if (!Directory.Exists(applicationFile)) return;
            
            string logFileName = $"{applicationFile}/Log";
            
            if (!Directory.Exists(logFileName)) return;
            string textFileName = $"{logFileName}/{m_LogTextFileName}.txt";
            if (File.Exists(textFileName))
            {
                File.Delete(textFileName);
            }
        }

        void ActionExecution(BenchmarkAction action)
        {
            for (int i = 0; i < m_Iteration; i++)
            {
                action?.m_Event?.Invoke();
            }
        }

        [System.Serializable]
        public class BenchmarkAction
        {
            [SerializeField] string m_label;
            [SerializeField] UnityEvent m_event;
            public string m_Label => m_label;
            public UnityEvent m_Event => m_event;
        }
#endif
    }

    

    public static class Benchmark
    {
        public static void TimeExecution(System.Action function, string label = null, bool ms = true)
        {
            System.Diagnostics.Stopwatch timer = System.Diagnostics.Stopwatch.StartNew();
            try
            {
                function?.Invoke();
            }
            finally
            {
                timer.Stop();
                string timeMessage = ms ? $"{timer.ElapsedMilliseconds}ms" : $"{timer.Elapsed}";
                timeMessage = timeMessage.Color(Color.green);

                string result = $"{"Took:".Color(Color.white)} {timeMessage}".Bold();
                if (label != null) result = $"{$"({label})".Bold().Color(Color.white)} {result}";
                Debug.Log(result);
            }
        }

        public static void TimeExecution(System.Action function, out string result, string label = null, bool ms = true)
        {
            System.Diagnostics.Stopwatch timer = System.Diagnostics.Stopwatch.StartNew();
            try
            {
                function?.Invoke();
            }
            finally
            {
                timer.Stop();
                string timeMessage = ms ? $"{timer.ElapsedMilliseconds}ms" : $"{timer.Elapsed}";
                result = $"Took: {timeMessage}";
                if (label != null) result = $"{label} {result}";
                Debug.Log(result);
            }
        }
    }
}