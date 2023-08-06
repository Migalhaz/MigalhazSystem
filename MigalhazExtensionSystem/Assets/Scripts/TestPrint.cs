using MigalhaSystem.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPrint : MonoBehaviour
{
    [ContextMenu("Reset Local Transform")]
    public void ResetLocalTransform()
    {
        transform.ResetLocalTransformation();
    }

    [ContextMenu("Reset Transform")]
    public void ResetTransform()
    {
        transform.ResetTransformation();
    }
}
