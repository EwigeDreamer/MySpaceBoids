using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyTools.Helpers;
using System;

public class PopupBase : MonoValidate
{
    public event Action OnStartShowing = delegate { };
    public event Action OnEndShowing = delegate { };
    public event Action OnStartHiding = delegate { };
    public event Action OnEndHiding = delegate { };
    public event Action OnRemoving = delegate { };

    [SerializeField] int m_SortOrder = 1000;
    [SerializeField] Canvas m_Canvas;

    protected virtual int SortDelta => 0;

    int SortOrder => m_SortOrder + SortDelta;

    protected override void OnValidate()
    {
        base.OnValidate();
        if (ValidateGetComponent(ref m_Canvas))
            if (m_Canvas.sortingOrder != SortOrder)
                m_Canvas.sortingOrder = SortOrder;
    }

    public void Init()
    {
        if (ValidateGetComponent(ref m_Canvas)) 
            m_Canvas.sortingOrder = m_SortOrder + SortDelta;
        OnInit();
    }

    public void Show(Action callback)
    {
        OnStartShowing();
        callback += () => OnEndShowing();
        OnShow(callback);
    }
    public void Hide(Action callback)
    {
        OnStartHiding();
        callback += () => OnEndHiding();
        OnHide(callback);
    }
    public void Remove()
    {
        OnRemoving();
        OnRemove();
        Destroy(GO);
    }

    protected virtual void OnInit()
    {
        GO.SetActive(false);
    }
    protected virtual void OnShow(Action callback)
    {
        GO.SetActive(true);
        callback?.Invoke();
    }
    protected virtual void OnHide(Action callback)
    {
        GO.SetActive(false);
        callback?.Invoke();
        Remove();
    }
    protected virtual void OnRemove() { }
}
