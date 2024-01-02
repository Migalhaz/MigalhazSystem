using MigalhaSystem.Extensions;
using MigalhaSystem.Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondPoolTest : MonoBehaviour, IPoolable
{
    public void OnPull()
    {
        Debug.Log("Pull Test 2".Color(Color.blue));
    }

    public void OnPush()
    {
        Debug.Log("Push Test 2".Color(Color.blue));
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
