using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MigalhaSystem.Singleton
{
    public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        [SerializeField, Tooltip("Don't Destroy On Load")] bool m_DDOL;
        private static T instance;
        public static T Instance => GetInstance();
        public static bool IsNull => instance == null;

        protected virtual void Awake()
        {
            if (GetInstance(false) != null && GetInstance(false) != this)
            {
                Destroy(gameObject);
            }

            if (m_DDOL)
            {
                DontDestroyOnLoad(Instance.gameObject);
            }
        }

        public static T GetInstance(bool createIfNullInstance = true)
        {
            if (IsNull)
            {
                instance = FindObjectOfType<T>();
                if (IsNull && createIfNullInstance)
                {
                    GameObject singletonObject = new GameObject(typeof(T).Name);
                    instance = singletonObject.AddComponent<T>();
                }
            }
            return instance;
        }
    }
}