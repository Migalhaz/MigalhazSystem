using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MigalhaSystem
{
    public class GameManager : MonoBehaviour
    {
        static GameManager instance;
        public static GameManager Instance
        {
            get
            {
                if (instance == null) instance = FindObjectOfType<GameManager>();
                return instance;
            }
        }
        List<MigalhaComponent> m_components;

        [SerializeField] PlayerMove m_playerMove;

        void Awake() { GetAllComponent().ForEach(x => x.Awake()); }
        void OnEnable() { GetAllComponent().ForEach(x => x.OnEnable()); }
        void Start() { GetAllComponent().ForEach(x => x.Start()); }
        void Update() { GetAllComponent().ForEach(x => x.Update()); }
        void FixedUpdate() { GetAllComponent().ForEach(x => x.FixedUpdate()); }
        void LateUpdate() { GetAllComponent().ForEach(x => x.LateUpdate()); }
        void OnDisable() { GetAllComponent().ForEach(x => x.OnDisable()); }
        void OnDestroy() { GetAllComponent().ForEach(x => x.OnDestroy()); }

        List<MigalhaComponent> GetAllComponent()
        {
            if (m_components == null)
            {
                m_components = new List<MigalhaComponent>()
                {
                    m_playerMove
                };
            }
            return m_components;
        }

        public T GetMigalhaComponent<T>() where T : MigalhaComponent
        {
            T component = (GetAllComponent().Find(x => x is T) as T);
            return component;
        }
        public List<T> GetAllMigalhaComponents<T>() where T : MigalhaComponent
        {
            List<MigalhaComponent> migalhaComponent = GetAllComponent().FindAll(x => x is T);
            List<T> result = new List<T>();
            foreach (MigalhaComponent component in migalhaComponent)
            {
                result.Add(component as T);
            }
            return result;
        }
        public bool TryGetMigalhaComponent<T>(out T component) where T : MigalhaComponent
        {
            component = GetMigalhaComponent<T>();
            return component != null;
        }
    }

    public abstract class MigalhaComponent
    {
        [SerializeField] GameObject m_gameObject;
        public GameObject gameObject => m_gameObject;
        public Transform transform => m_gameObject.transform;
        public GameManager gameManager => GameManager.Instance;

        public virtual void Awake() { }
        public virtual void OnEnable() { }
        public virtual void Start() { }
        public virtual void Update() { }
        public virtual void FixedUpdate() { }
        public virtual void LateUpdate() { }
        public virtual void OnDisable() { }
        public virtual void OnDestroy() { }
    }

    [System.Serializable]
    public class PlayerMove : MigalhaComponent
    {
        [SerializeField, Min(0)] float m_moveSpeed;
        [SerializeField] Rigidbody2D m_rig;
        Vector2 m_input;

        public override void Update()
        {
            float x = Input.GetAxisRaw("Horizontal");
            float y = Input.GetAxisRaw("Vertical");
            m_input.Set(x, y);
        }

        public override void FixedUpdate()
        {
            Vector2 direction = m_rig.velocity;
            direction.x = m_moveSpeed * m_input.x;
            m_rig.velocity = direction;
        }
    }
}
