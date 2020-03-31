using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyTools.Singleton;
using Cinemachine;

public class MainCamera : MonoSingleton<MainCamera>
{
    static new Camera camera = null;
    static CinemachineBrain cinemachineBrain = null;
    public static Camera Camera => camera;
    public static CinemachineBrain CinemachineBrain => cinemachineBrain;

    protected override void Awake()
    {
        base.Awake();
        camera = GetComponent<Camera>();
        cinemachineBrain = GetComponent<CinemachineBrain>();
    }
}
