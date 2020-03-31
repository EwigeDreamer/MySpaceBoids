using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Object = UnityEngine.Object;

public static class PopupManager
{
    const string containerName = "PopupContainer";

    static Dictionary<Type, PopupBase> m_PrefabDict;
    static Transform m_Container;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void Init()
    {
        var popupListAsset = Resources.Load<PopupPrefabList>("PopupPrefabList");
        if (popupListAsset == null)
        {
            Debug.LogWarning("No PopUp data!");
            return;
        }

        m_Container = new GameObject(containerName).transform;
        Object.DontDestroyOnLoad(m_Container.gameObject);

        var prefabs = popupListAsset.Popups;
        var dict = m_PrefabDict = new Dictionary<Type, PopupBase>();
        foreach (var prefab in prefabs)
            dict[prefab.GetType()] = prefab;
    }

    public static TPopup GetPopup<TPopup>() where TPopup : PopupBase
    {
        var popup = m_PrefabDict[typeof(TPopup)];
        popup = Object.Instantiate(popup, m_Container);
        popup.Init();
        return (TPopup)popup;
    }

    public static TPopup OpenPopup<TPopup>(Action callback = null) where TPopup : PopupBase
    {
        var popup = GetPopup<TPopup>();
        popup.Show(callback);
        return popup;
    }
}
