using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Diagnostics;
using Deb = UnityEngine.Debug;
using MyTools.Pooling.Base;
using MyTools.Pooling;

public class PoolTest : MonoBehaviour
{
    [SerializeField] int m_Count = 10;
    [SerializeField] int m_RmInd = 5;
    
    List<GameObject> m_List = new List<GameObject>();

    int z = 0;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Keypad1))
            GetObj();
        if (Input.GetKeyDown(KeyCode.Keypad3))
            RemoveObj();
        if (Input.GetKeyDown(KeyCode.Keypad0))
            RemoveObj(m_RmInd);
    }

    void GetObj()
    {
        if (m_List.Count > m_Count - 1) return;
        if (!ObjectPool.I.TrySpawn("dummy1", new Vector3(0, 0, z), Quaternion.identity, null, out var obj)) return;
        m_List.Add(obj);
        z++;
    }
    void RemoveObj()
    {
        if (m_List.Count < 1) return;
        if (!ObjectPool.I.Despawn(m_List[0])) return;
        m_List.RemoveAt(0);
        if (m_List.Count < 1) z = 0;
    }
    void RemoveObj(int ind)
    {
        if (ind < 0 || ind > m_List.Count - 1) return;
        if (!ObjectPool.I.Despawn(m_List[ind])) return;
        m_List.RemoveAt(ind);
        if (m_List.Count < 1) z = 0;
    }
}