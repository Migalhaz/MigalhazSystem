using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace MigalhaSystem.Pool 
{
    [CreateAssetMenu(fileName = "New Pool Data", menuName = "Scriptable Object/Pool/New Pool")]
    public class PoolData : ScriptableObject
    {
        public GameObject m_Prefab;
        public int m_PoolSize = 1;
        public bool m_ExpandablePool = false;
        public bool m_DestroyExtraObjectsAfterUse;
        public int m_ExtraObjectsAllowed;
        public bool m_RemoveIdlePool;
        public float m_IdlePoolLifeTime;

        public int MaxPoolSize()
        {
            if (!m_ExpandablePool)
            {
                return m_PoolSize;
            }
            return m_PoolSize + m_ExtraObjectsAllowed;
        }


        public GameObject PullObject(bool _createPoolNotFound = true) 
            => PoolManager.Instance.PullObject(this, _createPoolNotFound);
        public GameObject PullObject(System.Action<GameObject> action, bool _createPoolNotFound = true) 
            => PoolManager.Instance.PullObject(this, action, _createPoolNotFound);
        public GameObject PullObject(Vector3 _position, Quaternion _rotation, bool _createPoolNotFound = true)
            => PoolManager.Instance.PullObject(this, x => { x.transform.position = _position; x.transform.rotation = _rotation; }, _createPoolNotFound);
        public GameObject PullObject(Vector3 _position, Quaternion _rotation, System.Action<GameObject> action, bool _createPoolNotFound = true)
            => PoolManager.Instance.PullObject(this, x => { x.transform.position = _position; x.transform.rotation = _rotation; action?.Invoke(x); }, _createPoolNotFound);
        public T PullObject<T>(bool _createPoolNotFound = true) where T : Component
            => PoolManager.Instance.PullObject<T>(this, _createPoolNotFound);
        public T PullObject<T>(System.Action<GameObject> action, bool _createPoolNotFound = true) where T : Component
            => PoolManager.Instance.PullObject<T>(this, action, _createPoolNotFound);
        public T PullObject<T>(Vector3 _position, Quaternion _rotation, bool _createPoolNotFound = true) where T : Component
            => PoolManager.Instance.PullObject<T>(this, x => { x.transform.position = _position; x.transform.rotation = _rotation; }, _createPoolNotFound);
        public T PullObject<T>(Vector3 _position, Quaternion _rotation, System.Action<GameObject> action, bool _createPoolNotFound = true) where T : Component
            => PoolManager.Instance.PullObject<T>(this, x => { x.transform.position = _position; x.transform.rotation = _rotation; action?.Invoke(x); }, _createPoolNotFound);
        public bool PullObject<T>(out T _component, bool _createPoolNotFound = true) where T : Component
        {
            _component = PoolManager.Instance.PullObject<T>(this, _createPoolNotFound);
            return _component != null;
        }
        public bool PullObject<T>(out T _component, System.Action<GameObject> action, bool _createPoolNotFound = true) where T : Component
        {
            _component = PoolManager.Instance.PullObject<T>(this, action, _createPoolNotFound);
            return _component != null;
        }
        public bool PullObject<T>(out T _component, Vector3 _position, Quaternion _rotation, bool _createPoolNotFound = true) where T : Component
        {
            _component = PoolManager.Instance.PullObject<T>(this, x => { x.transform.position = _position; x.transform.rotation = _rotation; }, _createPoolNotFound);
            return _component != null;
        }
        public bool PullObject<T>(out T _component, Vector3 _position, Quaternion _rotation, System.Action<GameObject> action, bool _createPoolNotFound = true) where T : Component
        {
            _component = PoolManager.Instance.PullObject<T>(this, x => { x.transform.position = _position; x.transform.rotation = _rotation; action?.Invoke(x); }, _createPoolNotFound);
            return _component != null;
        }
        public List<GameObject> PullAllObjects(bool _createPoolNotFound = true) => PoolManager.Instance.PullAllObjects(this, _createPoolNotFound);
        public List<GameObject> PullAllObjects(System.Action<GameObject> action, bool _createPoolNotFound = true) => PoolManager.Instance.PullAllObjects(this, action, _createPoolNotFound);
        public List<GameObject> PullAllObjects(Vector3 _position, Quaternion _rotation, bool _createPoolNotFound = true)
            => PoolManager.Instance.PullAllObjects(this, x => { x.transform.position = _position; x.transform.rotation = _rotation; }, _createPoolNotFound);
        public List<GameObject> PullAllObjects(Vector3 _position, Quaternion _rotation, System.Action<GameObject> action, bool _createPoolNotFound = true)
           => PoolManager.Instance.PullAllObjects(this, x => { x.transform.position = _position; x.transform.rotation = _rotation; action?.Invoke(x); }, _createPoolNotFound);
        public List<T> PullAllObjects<T>(bool _createPoolNotFound = true) where T : Component => PoolManager.Instance.PullAllObjects<T>(this, _createPoolNotFound);
        public List<T> PullAllObjects<T>(System.Action<GameObject> action, bool _createPoolNotFound = true) where T : Component => PoolManager.Instance.PullAllObjects<T>(this, action, _createPoolNotFound);
        public List<T> PullAllObjects<T>(Vector3 _position, Quaternion _rotation, bool _createPoolNotFound = true) where T : Component
            => PoolManager.Instance.PullAllObjects<T>(this, x => { x.transform.position = _position; x.transform.rotation = _rotation; }, _createPoolNotFound);
        public List<T> PullAllObjects<T>(Vector3 _position, Quaternion _rotation, System.Action<GameObject> action, bool _createPoolNotFound = true) where T : Component
            => PoolManager.Instance.PullAllObjects<T>(this, x => { x.transform.position = _position; x.transform.rotation = _rotation; action?.Invoke(x); }, _createPoolNotFound);
        public bool PushObject(GameObject _gameObject, bool _createPoolNotFound = false) => PoolManager.Instance.PushObject(this, _gameObject, _createPoolNotFound);
        public bool PushObject(GameObject _gameObject, System.Action<GameObject> action, bool _createPoolNotFound = false) => PoolManager.Instance.PushObject(this, _gameObject, action, _createPoolNotFound);
        public bool PushAllObjects(bool _createPoolNotFound = false) => PoolManager.Instance.PushAllObjects(this, _createPoolNotFound);
        public bool PushAllObjects(System.Action<GameObject> action, bool _createPoolNotFound = false) => PoolManager.Instance.PushAllObjects(this, action, _createPoolNotFound);
    }
}