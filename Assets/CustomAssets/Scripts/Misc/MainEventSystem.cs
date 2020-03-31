using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using MyTools.Singleton;

public class MainEventSystem : MonoSingleton<MainEventSystem>
{
    static EventSystem eventSystem = null;
    static StandaloneInputModule standaloneInputModule = null;
    public static EventSystem EventSystem => eventSystem;
    public static StandaloneInputModule StandaloneInputModule => standaloneInputModule;

    protected override void Awake()
    {
        base.Awake();
        eventSystem = GetComponent<EventSystem>();
        standaloneInputModule = GetComponent<StandaloneInputModule>();
    }
}
