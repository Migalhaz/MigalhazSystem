using MigalhaSystem.Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerTest : MonoBehaviour
{
    [SerializeField] PoolData poolDataScriptableObject;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PoolManager.Instance.PullObject(poolDataScriptableObject);
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            PoolManager.Instance.PullAllObjects(poolDataScriptableObject);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            PoolManager.Instance.PushAllObjects(poolDataScriptableObject);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            List<Rigidbody2D> p = PoolManager.Instance.PullAllObjects<Rigidbody2D>(poolDataScriptableObject);
        }
    }


}
