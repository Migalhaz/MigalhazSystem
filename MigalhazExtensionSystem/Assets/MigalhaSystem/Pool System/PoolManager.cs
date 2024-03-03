using MigalhaSystem.Extensions;
using MigalhaSystem.Singleton;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MigalhaSystem.Pool
{
    public class PoolManager : Singleton<PoolManager>
    {
        [SerializeField] bool m_removeEmptyPool = true;
        [SerializeField] List<Pool> m_pools = new List<Pool>();

        protected override void Awake()
        {
            base.Awake();
            SetupPoolManager();
        }
        void SetupPoolManager()
        {
            List<Pool> startedPools = new();
            List<Pool> duplicatePool = new();
            
            foreach (Pool pool in m_pools)
            {
                if (pool.m_PoolData == null)
                {
                    Debug.LogError("Empty pool found!", this);
                    continue;
                }
                if (startedPools.Exists(x => x.m_PoolData == pool.m_PoolData))
                {
                    Debug.LogError("Duplicate pool found!", this);
                    duplicatePool.Add(pool);
                    continue;
                }
                startedPools.Add(pool);
                SetPool(pool);
            }
            m_pools.RemoveAll(duplicatePool);
            if (m_removeEmptyPool) m_pools.RemoveAll(x => x.m_PoolData == null);
        }
        void SetPool(Pool pool)
        {
            string NormalizeParentName()
            {
                if (pool.m_PoolData.name.ToUpper().EndsWith("POOL")) return pool.m_PoolData.name.Capitalize();
                return $"{pool.m_PoolData.name} Pool".Capitalize();
            }

            GameObject poolParent = new GameObject(NormalizeParentName());
            poolParent.transform.ResetTransformation();

            pool.SetupPool(poolParent.transform);
            for (int i = 0; i < pool.m_PoolData.m_PoolSize; i++)
            {
                pool.CreateObject();
            }
        }
        public Pool GetPool(PoolData _poolData, bool _debugErrors = true)
        {
            bool PoolCheck(List<Pool> availablePools)
            {
                if (availablePools == null)
                {
                    if (_debugErrors) Debug.LogError("No pool found!");
                    return false;
                }
                if (availablePools.Count <= 0)
                {
                    if (_debugErrors) Debug.LogError("No pool found!");
                    return false;
                }
                if (availablePools.Count > 1)
                {
                    if (_debugErrors) Debug.LogError("More than 1 pool found!");
                    return false;
                }
                return true;       
            }
            List<Pool> pools = m_pools.FindAll(x => x.ComparePoolData(_poolData));
            if (!PoolCheck(pools)) return null;
            return pools[0];
        }
        public bool AddPool(PoolData _poolData)
        {
            Pool pool = GetPool(_poolData, _debugErrors:false);
            if (pool != null) return false;
            Pool newPool = new(_poolData);
            m_pools.Add(newPool);
            SetPool(newPool);
            return true;
        }
        public bool DeletePool(PoolData _poolData)
        {
            Pool pool = GetPool(_poolData);
            if (pool == null) return false;
            pool.DeletePool();
            m_pools.Remove(pool);
            return true;
        }
        public GameObject PullObject(PoolData _poolData, bool _createPoolNotFound = true)
        {
            Pool pool = GetPool(_poolData);
            if (pool != null) return pool.PullObject();
            if (_createPoolNotFound == false) return null;
            Pool newPool = new(_poolData);
            m_pools.Add(newPool);
            SetPool(newPool);
            return newPool.PullObject();
        }
        public GameObject PullObject(PoolData _poolData, Vector3 _position, Quaternion _rotation, bool _createPoolNotFound = true)
        {
            GameObject g = PullObject(_poolData, _createPoolNotFound);
            if (g == null) return null;
            g.transform.position = _position;
            g.transform.rotation = _rotation;
            return g;
        }
        public T PullObject<T>(PoolData _poolData, bool _createPoolNotFound = true) where T : Component
        {
            Pool pool = GetPool(_poolData);
            if (pool != null) return pool.PullObject<T>();
            if (_createPoolNotFound == false) return null;
            Pool newPool = new(_poolData);
            m_pools.Add(newPool);
            SetPool(newPool);
            return newPool.PullObject<T>();
        }
        public T PullObject<T>(PoolData _poolData, Vector3 _position, Quaternion _rotation, bool _createPoolNotFound = true) where T : Component
        {
            T component = PullObject<T>(_poolData, _createPoolNotFound);
            component.transform.position = _position;
            component.transform.rotation = _rotation;
            return component;
        }
        public bool PullObject<T>(PoolData _poolData, out T _component, bool _createPoolNotFound = true) where T : Component
        {
            _component = PullObject<T>(_poolData, _createPoolNotFound);
            return _component != null;
        }
        public bool PullObject<T>(PoolData _poolData, out T _component, Vector3 _position, Quaternion _rotation, bool _createPoolNotFound = true) where T : Component
        {
            _component = PullObject<T>(_poolData, _createPoolNotFound);
            _component.transform.position = _position;
            _component.transform.rotation = _rotation;
            return _component != null;
        }
        public List<GameObject> PullAllObjects(PoolData _poolData, bool _createPoolNotFound = true)
        {
            Pool pool = GetPool(_poolData);
            if (pool != null) return pool.PullAllObjects();
            if (_createPoolNotFound == false) return null;
            Pool newPool = new(_poolData);
            m_pools.Add(newPool);
            SetPool(newPool);
            return newPool.PullAllObjects();
        }
        public List<GameObject> PullAllObjects(PoolData _poolData, Vector3 _position, Quaternion _rotation, bool _createPoolNotFound = true)
        {
            List<GameObject> objs = PullAllObjects(_poolData, _createPoolNotFound);
            foreach (GameObject obj in objs)
            {
                obj.transform.position = _position;
                obj.transform.rotation = _rotation;
            }
            return objs;
        }
        public List<T> PullAllObjects<T>(PoolData _poolData, bool _createPoolNotFound = true) where T : Component
        {
            Pool pool = GetPool(_poolData);
            if (pool != null) return pool.PullAllObjects<T>();
            if (_createPoolNotFound == false) return null;
            Pool newPool = new(_poolData);
            m_pools.Add(newPool);
            SetPool(newPool);
            return newPool.PullAllObjects<T>();
        }
        public List<T> PullAllObjects<T>(PoolData _poolData, Vector3 _position, Quaternion _rotation, bool _createPoolNotFound = true) where T : Component
        {
            List<T> objs = PullAllObjects<T>(_poolData, _createPoolNotFound);
            foreach (T obj in objs)
            {
                obj.transform.position = _position;
                obj.transform.rotation = _rotation;
            }
            return objs;
        }
        public void PushObject(PoolData _poolData, GameObject _gameObject, bool _createPoolNotFound = false)
        {
            Pool pool = GetPool(_poolData);
            if (pool != null)
            {
                pool.PushObject(_gameObject);
                return;
            }
            if (_createPoolNotFound == false) return;
            Pool newPool = new(_poolData);
            m_pools.Add(newPool);
            SetPool(newPool);
            newPool.PushObject(_gameObject);
        }
        public void PushAllObjects(PoolData _poolData, bool _createPoolNotFound = false)
        {
            Pool pool = GetPool(_poolData);
            if (pool != null)
            {
                pool.PushAllObjects();
                return;
            }
            if (_createPoolNotFound == false) return;
            Pool newPool = new(_poolData);
            m_pools.Add(newPool);
            SetPool(newPool);
            newPool.PushAllObjects();
        }
#if UNITY_EDITOR
        private void OnValidate()
        {
            foreach (Pool pool in m_pools)
            {
                pool.OnValidate();
            }
        }
#endif
    }

    [Serializable]
    public class Pool 
    {
        [field: SerializeField, HideInInspector] public string m_PoolTag { get; private set; }
        #region Variables
        [SerializeField] PoolData m_poolData;

        Transform m_poolParent;
        List<GameObject> m_freeObjects;
        List<GameObject> m_inUseObjects;
        #region Getters
        public PoolData m_PoolData => m_poolData;
        public List<GameObject> m_FreeObjects => m_freeObjects;
        public List<GameObject> m_InUseObjects => m_inUseObjects;
        #endregion

        #endregion

        #region Methods
#if UNITY_EDITOR
        public void OnValidate()
        {
            if (m_poolData == null)
            {
                m_PoolTag = "No pool!";
            }
            else
            {
                m_PoolTag = m_poolData.name;
            }

        }
#endif
        public Pool()
        {
            m_freeObjects = new List<GameObject>();
            m_inUseObjects = new List<GameObject>();
            m_poolParent = null;
        }
        public Pool(PoolData poolData)
        {
            m_poolData = poolData;
            m_freeObjects = new List<GameObject>();
            m_inUseObjects = new List<GameObject>();
            m_poolParent = null;
        }
        public bool ComparePoolData(PoolData _poolData)
        {
            return m_poolData == _poolData;
        }
        public void SetupPool(Transform _parent)
        {
            m_freeObjects = new List<GameObject>();
            m_inUseObjects = new List<GameObject>();
            m_poolParent = _parent;
        }
        public void DeletePool()
        {
            m_poolParent.DeleteChildren();
            m_freeObjects.Clear();
            m_inUseObjects.Clear();
            UnityEngine.Object.Destroy(m_poolParent.gameObject);
        }
        public void CreateObject()
        {
            GameObject newGameObject = UnityEngine.Object.Instantiate(m_poolData.m_Prefab, m_poolParent);
            newGameObject.SetActive(false);
            m_freeObjects.Add(newGameObject);
        }
        public GameObject PullObject()
        {
            if (AllowCreateNewObject())
            {
                CreateObject();
            }
            if (!FreeGameObjects())
            {
                return null;
            }

            GameObject poolGameObject = m_FreeObjects[m_FreeObjects.Count - 1];
            m_freeObjects.Remove(poolGameObject);
            m_inUseObjects.Add(poolGameObject);
            poolGameObject.SetActive(true);

            IPullable[] pullableArray = poolGameObject.GetComponentsInChildren<IPullable>();
            if (PullableAvailable())
            {
                foreach (IPullable pullableItem in pullableArray)
                {
                    pullableItem.OnPull();
                }
            }

            bool FreeGameObjects()
            {
                if (m_freeObjects == null) return false;
                if (m_freeObjects.Count <= 0) return false;
                return true;
            }
            bool AllowCreateNewObject()
        {
            if (m_poolParent.childCount <= 0) return true;
            if (m_freeObjects.Count > 0) return false;
            if (m_poolParent.childCount >= m_poolData.m_PoolSize)
            {
                if (!m_poolData.m_ExpandablePool)
                {
                    return false;
                }
            }
            return true;
        }
            bool PullableAvailable()
            {
                if (pullableArray == null) return false;
                if (pullableArray.Length <= 0) return false;
                return true;
            }
            return poolGameObject;
        }
        public T PullObject<T>() where T : Component
        {
            if (PullObject().TryGetComponent(out T component))
            {
                return component;
            }
            Debug.LogError($"{typeof(T)} component not found!");
            return null;
        }
        public List<GameObject> PullAllObjects()
        {
            List<GameObject> gameObjects = new();
            while (m_freeObjects.Count >= 1)
            {
                gameObjects.Add(PullObject());
            }

            return gameObjects;
        }
        public List<T> PullAllObjects<T>() where T : Component
        {
            List<T> gameObjects = new();
            while (m_freeObjects.Count >= 1)
            {
                gameObjects.Add(PullObject<T>());
            }

            return gameObjects;
        }
        public void PushObject(GameObject _activeGameObject)
        {
            if (!m_inUseObjects.Contains(_activeGameObject)) return;
            IPushable[] pushableArray = _activeGameObject.GetComponentsInChildren<IPushable>(true);
            if (IPushableAvailable())
            {
                foreach (IPushable pushableItem in pushableArray)
                {
                    pushableItem.OnPush();
                }
            }
            m_freeObjects.Add(_activeGameObject);
            m_inUseObjects.Remove(_activeGameObject);
            _activeGameObject.SetActive(false);

            if (!m_poolData.m_DestroyExtraObjectsAfterUse) return;
            
            if (m_poolParent.childCount <= m_poolData.MaxPoolSize()) return;
            UnityEngine.Object.Destroy(_activeGameObject);
            m_freeObjects.Remove(_activeGameObject);

            bool IPushableAvailable()
            {
                if (pushableArray == null) return false;
                if (pushableArray.Length <= 0) return false;
                return true;
            }
        }
        public void PushAllObjects()
        {
            while (m_inUseObjects.Count >= 1)
            {
                PushObject(m_inUseObjects[0]);
            }
        }
        #endregion
    }

    public interface IPoolable : IPullable, IPushable
    {
    }
    public interface IPullable
    {
        void OnPull();
    }
    public interface IPushable
    {
        void OnPush();
    }
}
