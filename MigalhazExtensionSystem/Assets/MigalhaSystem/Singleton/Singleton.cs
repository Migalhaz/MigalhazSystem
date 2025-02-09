using UnityEngine;

namespace MigalhaSystem.Singleton
{
    public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        [SerializeField, Tooltip("Don't Destroy On Load")] bool m_DDOL;
        private static T instance;
        public static T Instance
        {
            get
            {
                if (instance == null) return null;
                if (instance.gameObject == null) return null;
                return instance;
            }
        }
        public static bool IsNull => Instance == null;

        protected virtual void Awake()
        {
            SetupObject();
        }

        void SetupObject()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            instance = this as T;
            if (m_DDOL)
            {
                DontDestroyOnLoad(Instance.gameObject);
            }
        }

        static T CreateNewSingletonObject()
        {
            GameObject singletonObject = new GameObject(typeof(T).Name);
            instance = singletonObject.AddComponent<T>();
            return instance;
        }

        public static T ProvideInstance()
        {
            if (!IsNull) return Instance;
            instance = FindObjectOfType<T>();
            if (IsNull)
            {
                CreateNewSingletonObject();
            }
            return instance;
        }

        public static T OverrideInstance()
        {
            if (IsNull) return ProvideInstance();
            Destroy(Instance.gameObject);
            instance = null;
            CreateNewSingletonObject();
            return instance;
        }
    }
}