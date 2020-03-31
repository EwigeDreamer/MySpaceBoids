using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyTools.Helpers;
using System;

public class UIBase : MonoValidate
{
    public event Action OnShowingStart = delegate { };
    public event Action OnShowingEnd = delegate { };

    public event Action OnHidingStart = delegate { };
    public event Action OnHidingEnd = delegate { };

    public void Show() => ShowInplement(() => OnShowingStart(), () => OnShowingEnd());
    public void Hide() => HideInplement(() => OnHidingStart(), () => OnHidingEnd());

    protected virtual void ShowInplement(Action start, Action end)
    {
        start();
        GO.SetActive(true);
        end();
    }

    protected virtual void HideInplement(Action start, Action end)
    {
        start();
        GO.SetActive(false);
        end();
    }
}
