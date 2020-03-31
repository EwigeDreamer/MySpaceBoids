using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtMainCamera : MonoBehaviour
{
    Transform tr;
    private void Awake()
    {
        this.tr = transform;
    }
    private void LateUpdate()
    {
        var camTr = MainCamera.Camera.transform;
        this.tr.rotation = Quaternion.LookRotation(camTr.forward, camTr.up);
    }
}
