using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyTools.Pooling;
using System;

public class PooledObjectTest : MonoBehaviour, IPooledComponent
{
#pragma warning disable 67
    event Action Deactive;
    event Action IPooledComponent.Deactive { add { Deactive += value; } remove { Deactive -= value; } }

    void IPooledComponent.OnActivation()
    {
        Debug.LogWarning("ACTIVATION!!!! " + name);
    }

    void IPooledComponent.OnDeactivation()
    {
        Debug.LogWarning("DEACTIVATION!!!! " + name);
    }
#pragma warning restore 67
}
