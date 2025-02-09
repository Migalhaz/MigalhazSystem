using MigalhaSystem.Extensions;
using MigalhaSystem.Singleton;
using System.Collections.Generic;
using UnityEngine;

namespace MigalhaSystem.Pool
{
    public class PoolManager : Singleton<PoolManager>
    {
        [SerializeField, HideInInspector] bool m_removeEmptyPool = true;
        [SerializeField] List<Pool> m_pools = new List<Pool>();
        [SerializeField] List<PoolCollection> m_poolCollections = new List<PoolCollection>();

        public bool m_RemoveEmptyPool { get { return m_removeEmptyPool; } set { m_removeEmptyPool = value; } }
        public List<Pool> m_Pools => m_pools;
        public List<PoolCollection> m_PoolCollections => m_poolCollections;

        protected override void Awake()
        {
            base.Awake();
            SetupPoolManager();
        }
        void SetupPoolManager()
        {
            AddPoolCollections();
            List<Pool> startedPools = new();
            List<Pool> duplicatePool = new();
            
            foreach (Pool pool in m_pools)
            {
                if (pool.m_PoolData == null)
                {
#if DEBUG
                    Debug.LogError("Empty pool found!", this);
#endif
                    continue;
                }
                if (startedPools.Exists(x => x.m_PoolData == pool.m_PoolData))
                {
#if DEBUG
                    Debug.LogError("Duplicate pool found!", this);
#endif
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
                if (pool.m_PoolData.name.ToUpper().Contains("POOL")) return pool.m_PoolData.name.Capitalize();
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
        public bool ContainsPool(PoolData _poolData) => m_pools.Exists(pool => pool.ComparePoolData(_poolData));
        public Pool GetPool(PoolData _poolData, bool _debugErrors = true)
        {
            bool PoolCheck(List<Pool> availablePools)
            {
                if (availablePools == null)
                {
#if DEBUG
                    if (_debugErrors) Debug.LogError("No pool found!");
#endif
                    return false;
                }
                if (availablePools.Count <= 0)
                {
#if DEBUG
                    if (_debugErrors) Debug.LogError("No pool found!");
#endif
                    return false;
                }
                if (availablePools.Count > 1)
                {
#if DEBUG
                    if (_debugErrors) Debug.LogError("More than 1 pool found!");
#endif
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
            if (!_createPoolNotFound) return null;
            Pool newPool = new(_poolData);
            m_pools.Add(newPool);
            SetPool(newPool);
            return newPool.PullObject();
        }
        public GameObject PullObject(PoolData _poolData, System.Action<GameObject> action, bool _createPoolNotFound = true)
        {
            Pool pool = GetPool(_poolData);
            if (pool != null) return pool.PullObject(action);
            if (!_createPoolNotFound) return null;
            Pool newPool = new(_poolData);
            m_pools.Add(newPool);
            SetPool(newPool);
            return newPool.PullObject(action);
        }
        public GameObject PullObject(PoolData _poolData, Vector3 _position, Quaternion _rotation, bool _createPoolNotFound = true) 
            => PullObject(_poolData, x => { x.transform.position = _position; x.transform.rotation = _rotation; }, _createPoolNotFound);
        public GameObject PullObject(PoolData _poolData, Vector3 _position, Quaternion _rotation, System.Action<GameObject> action, bool _createPoolNotFound = true)        
            => PullObject(_poolData, x => { x.transform.position = _position; x.transform.rotation = _rotation; action?.Invoke(x); }, _createPoolNotFound);
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
        public T PullObject<T>(PoolData _poolData, System.Action<GameObject> action, bool _createPoolNotFound = true) where T : Component
        {
            Pool pool = GetPool(_poolData);
            if (pool != null) return pool.PullObject<T>(action);
            if (_createPoolNotFound == false) return null;
            Pool newPool = new(_poolData);
            m_pools.Add(newPool);
            SetPool(newPool);
            return newPool.PullObject<T>(action);
        }
        public T PullObject<T>(PoolData _poolData, Vector3 _position, Quaternion _rotation, bool _createPoolNotFound = true) where T : Component
            => PullObject<T>(_poolData, x => { x.transform.position = _position; x.transform.rotation = _rotation; }, _createPoolNotFound);
        public T PullObject<T>(PoolData _poolData, Vector3 _position, Quaternion _rotation, System.Action<GameObject> action, bool _createPoolNotFound = true) where T : Component
            => PullObject<T>(_poolData, x => { x.transform.position = _position; x.transform.rotation = _rotation; action?.Invoke(x); }, _createPoolNotFound);
        public bool PullObject<T>(PoolData _poolData, out T _component, bool _createPoolNotFound = true) where T : Component
        {
            _component = PullObject<T>(_poolData, _createPoolNotFound);
            return _component != null;
        }
        public bool PullObject<T>(PoolData _poolData, out T _component, System.Action<GameObject> action, bool _createPoolNotFound = true) where T : Component
        {
            _component = PullObject<T>(_poolData, action, _createPoolNotFound);
            return _component != null;
        }
        public bool PullObject<T>(PoolData _poolData, out T _component, Vector3 _position, Quaternion _rotation, bool _createPoolNotFound = true) where T : Component
        {
            _component = PullObject<T>(_poolData, x => { x.transform.position = _position; x.transform.rotation = _rotation; }, _createPoolNotFound);
            return _component != null;
        }
        public bool PullObject<T>(PoolData _poolData, out T _component, Vector3 _position, Quaternion _rotation, System.Action<GameObject> action, bool _createPoolNotFound = true) where T : Component
        {
            _component = PullObject<T>(_poolData, x => { x.transform.position = _position; x.transform.rotation = _rotation; action?.Invoke(x); }, _createPoolNotFound);
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
        public List<GameObject> PullAllObjects(PoolData _poolData, System.Action<GameObject> action, bool _createPoolNotFound = true)
        {
            Pool pool = GetPool(_poolData);
            if (pool != null) return pool.PullAllObjects(action);
            if (_createPoolNotFound == false) return null;
            Pool newPool = new(_poolData);
            m_pools.Add(newPool);
            SetPool(newPool);
            return newPool.PullAllObjects(action);
        }
        public List<GameObject> PullAllObjects(PoolData _poolData, Vector3 _position, Quaternion _rotation, bool _createPoolNotFound = true)
            => PullAllObjects(_poolData, x => { x.transform.position = _position; x.transform.rotation = _rotation; }, _createPoolNotFound);
        public List<GameObject> PullAllObjects(PoolData _poolData, Vector3 _position, Quaternion _rotation, System.Action<GameObject> action, bool _createPoolNotFound = true)
           => PullAllObjects(_poolData, x => { x.transform.position = _position; x.transform.rotation = _rotation; action?.Invoke(x); }, _createPoolNotFound);
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
        public List<T> PullAllObjects<T>(PoolData _poolData, System.Action<GameObject> action, bool _createPoolNotFound = true) where T : Component
        {
            Pool pool = GetPool(_poolData);
            if (pool != null) return pool.PullAllObjects<T>(action);
            if (_createPoolNotFound == false) return null;
            Pool newPool = new(_poolData);
            m_pools.Add(newPool);
            SetPool(newPool);
            return newPool.PullAllObjects<T>(action);
        }
        public List<T> PullAllObjects<T>(PoolData _poolData, Vector3 _position, Quaternion _rotation, bool _createPoolNotFound = true) where T : Component
            => PullAllObjects<T>(_poolData, x => { x.transform.position = _position; x.transform.rotation = _rotation; }, _createPoolNotFound);
        public List<T> PullAllObjects<T>(PoolData _poolData, Vector3 _position, Quaternion _rotation, System.Action<GameObject> action, bool _createPoolNotFound = true) where T : Component
            => PullAllObjects<T>(_poolData, x => { x.transform.position = _position; x.transform.rotation = _rotation; action?.Invoke(x); }, _createPoolNotFound);
        public bool PushObject(PoolData _poolData, GameObject _gameObject, bool _createPoolNotFound = false)
        {
            Pool pool = GetPool(_poolData);
            if (pool != null)
            {
                return pool.PushObject(_gameObject);
            }
            if (_createPoolNotFound == false) return false;
            Pool newPool = new(_poolData);
            m_pools.Add(newPool);
            SetPool(newPool);
            return newPool.PushObject(_gameObject);
        }
        public bool PushObject(PoolData _poolData, GameObject _gameObject, System.Action<GameObject> action, bool _createPoolNotFound = false)
        {
            Pool pool = GetPool(_poolData);
            if (pool != null)
            {
                return pool.PushObject(_gameObject, action);
            }
            if (_createPoolNotFound == false) return false;
            Pool newPool = new(_poolData);
            m_pools.Add(newPool);
            SetPool(newPool);
            return newPool.PushObject(_gameObject, action);
        }
        public bool PushAllObjects(PoolData _poolData, bool _createPoolNotFound = false)
        {
            Pool pool = GetPool(_poolData);
            if (pool != null)
            {
                return pool.PushAllObjects();
            }
            if (_createPoolNotFound == false) return false;
            Pool newPool = new(_poolData);
            m_pools.Add(newPool);
            SetPool(newPool);
            return newPool.PushAllObjects();
        }
        public bool PushAllObjects(PoolData _poolData, System.Action<GameObject> action, bool _createPoolNotFound = false)
        {
            Pool pool = GetPool(_poolData);
            if (pool != null)
            {
                return pool.PushAllObjects(action);
            }
            if (_createPoolNotFound == false) return false;
            Pool newPool = new(_poolData);
            m_pools.Add(newPool);
            SetPool(newPool);
            return newPool.PushAllObjects(action);
        }
        public void AddPoolCollections()
        {
            for (int i = m_poolCollections.Count - 1; i >= 0; i--)
            {
                PoolCollection collection = m_poolCollections[i];
                m_poolCollections.RemoveAt(i);
                if (collection == null) continue;
                if (collection.m_pools == null || collection.m_pools.Count <= 0) continue;
                foreach (Pool pool in collection.m_pools)
                {
                    if (pool == null || pool.m_PoolData == null || ContainsPool(pool.m_PoolData)) continue;
                    m_pools.Add(pool);
                }
            }
        }
        private void Update()
        {
            for (int i = m_pools.Count - 1; i >= 0; i--)
            {
                Pool pool = m_pools[i];
                PoolData poolData = pool.m_PoolData;
                if (!poolData.m_RemoveIdlePool) continue;
                pool.UpdateIdleTime(Time.deltaTime);
                if (pool.m_IdleTime >= poolData.m_IdlePoolLifeTime)
                {
                    pool.DeletePool();
                    m_pools.RemoveAt(i);
                }
            }
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

    [System.Serializable]
    public class Pool 
    {
        [field: SerializeField, HideInInspector] public string m_PoolTag { get; private set; }
        #region Variables
        [SerializeField] PoolData m_poolData;

        Transform m_poolParent;
        List<GameObject> m_freeObjects;
        List<GameObject> m_inUseObjects;
        float m_idleTime = 0f;

        Dictionary<GameObject, List<Component>> m_components;

        #region Getters
        public Transform m_PoolParent => m_poolParent;
        public PoolData m_PoolData => m_poolData;
        public List<GameObject> m_FreeObjects => m_freeObjects;
        public List<GameObject> m_InUseObjects => m_inUseObjects;
        public float m_IdleTime => m_idleTime;
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
        public Pool(PoolData poolData)
        {
            m_poolData = poolData;
            m_freeObjects = new List<GameObject>();
            m_inUseObjects = new List<GameObject>();
            m_poolParent = null;
            m_components = new Dictionary<GameObject, List<Component>>();
        }
        public bool ComparePoolData(PoolData _poolData) => m_poolData == _poolData;
        public void SetupPool(Transform _parent)
        {
            m_freeObjects = new List<GameObject>();
            m_inUseObjects = new List<GameObject>();
            m_poolParent = _parent;
            m_idleTime = 0;
            m_components = new Dictionary<GameObject, List<Component>>();
        }
        public void UpdateIdleTime(float deltaTime)
        {
            m_idleTime += deltaTime;
        }
        public void DeletePool()
        {
            if (m_poolParent != null && m_poolParent.childCount > 0)
            {
                m_poolParent.DestroyChildren();
                Object.Destroy(m_poolParent.gameObject);
            }
            m_freeObjects?.Clear();
            m_inUseObjects?.Clear();
        }
        public void CreateObject()
        {
            GameObject newGameObject = Object.Instantiate(m_poolData.m_Prefab, m_poolParent);
            newGameObject.SetActive(false);
            m_freeObjects.Add(newGameObject);
        }
        public GameObject PullObject(System.Action<GameObject> action = null)
        {
            m_idleTime = 0;
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

            if (action != null)
            {
                action?.Invoke(poolGameObject);
            }

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
                    if (!m_poolData.m_ExpandablePool) return false;
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
        public T PullObject<T>(System.Action<GameObject> action = null) where T : Component
        {
            GameObject gameObject = PullObject(action);
            T newComponent = null;
            m_components.Remove(null);

            if (!m_components.ContainsKey(gameObject))
            {
                if (gameObject.TryGetComponent(out newComponent))
                {
                    m_components.Add(gameObject, new List<Component>() { newComponent });
                    return newComponent;
                }
#if DEBUG
                Debug.LogError($"{typeof(T)} component not found!");
#endif                
                return null;
            }

            List<Component> components = m_components[gameObject];
            components.RemoveAll(x => x == null);
            foreach (var c in components)
            {
                if (c is T) return c as T;
            }

            if (gameObject.TryGetComponent(out newComponent))
            {
                components.Add(newComponent);
                return newComponent;
            }
#if DEBUG
            Debug.LogError($"{typeof(T)} component not found!");
#endif
            return null;
        }
        public List<GameObject> PullAllObjects(System.Action<GameObject> action = null)
        {
            m_idleTime = 0;
            List<GameObject> gameObjects = new();
            while (m_freeObjects.Count >= 1)
            {
                gameObjects.Add(PullObject(action));
            }

            return gameObjects;
        }
        public List<T> PullAllObjects<T>(System.Action<GameObject> action = null) where T : Component
        {
            m_idleTime = 0;
            List<T> gameObjects = new();
            while (m_freeObjects.Count >= 1)
            {
                gameObjects.Add(PullObject<T>(action));
            }
            return gameObjects;
        }
        public bool PushObject(GameObject _activeGameObject, System.Action<GameObject> action = null)
        {
            if (!m_inUseObjects.Contains(_activeGameObject)) return false;
            m_idleTime = 0;
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
            if (action != null)
            {
                action?.Invoke(_activeGameObject);
            }

            _activeGameObject.SetActive(false);

            if (!m_poolData.m_DestroyExtraObjectsAfterUse) return true;
            
            if (m_poolParent.childCount <= m_poolData.MaxPoolSize()) return true;
            Object.Destroy(_activeGameObject);
            m_freeObjects.Remove(_activeGameObject);
            return true;
            bool IPushableAvailable()
            {
                if (pushableArray == null) return false;
                if (pushableArray.Length <= 0) return false;
                return true;
            }
        }
        public bool PushAllObjects(System.Action<GameObject> action = null)
        {
            m_idleTime = 0;
            bool result = true;
            while (m_inUseObjects.Count >= 1)
            {
                if (!PushObject(m_inUseObjects[0], action)) result = false;
            }
            return result;
        }
        #endregion
    }
    public interface IPoolable : IPullable, IPushable{}
    public interface IPullable { void OnPull(); }
    public interface IPushable { void OnPush(); }
}