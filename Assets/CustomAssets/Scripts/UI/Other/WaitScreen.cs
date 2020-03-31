using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyTools.Extensions.Common;
using UnityEngine.UI;
using System;
using MyTools.Helpers;
using DG.Tweening;
using MyTools.Singleton;
using MyTools.Extensions.Colors;


public class WaitScreen : MonoSingleton<WaitScreen>
{
#pragma warning disable 649
    [SerializeField] GameObject waitScreenGo;
    [SerializeField] float duration = 0.5f;

    [Header("Background")]
    [SerializeField] Image background;

    [Header("Spinner")]
    [SerializeField] RectTransform spinnerContainer;
    [SerializeField] RectTransform spinnerPointOff;
    [SerializeField] RectTransform spinnerPointOn;
#pragma warning restore 649

    protected override void OnValidate()
    {
        base.OnValidate();
        if (this.waitScreenGo == null) this.waitScreenGo = gameObject;
    }

    protected override void Awake()
    {
        base.Awake();
        this.waitScreenGo.SetActive(false);
    }

    public Coroutine Show(bool forced = false)
    {
        return CorouWaiter.Start(GetRoutine());
        IEnumerator GetRoutine()
        {
            this.waitScreenGo.SetActive(true);
            if (forced)
            {
                this.background.color = this.background.color.SetAlpha(1f);
                this.spinnerContainer.anchoredPosition = this.spinnerPointOn.anchoredPosition;
            }
            else
            {
                this.background.color = this.background.color.SetAlpha(0f);
                this.spinnerContainer.anchoredPosition = this.spinnerPointOff.anchoredPosition;
                var sequence = DOTween.Sequence()
                    .Append(this.background.DOFade(1f, this.duration).SetEase(Ease.InOutSine))
                    .Join(this.spinnerContainer.DOAnchorPos(this.spinnerPointOn.anchoredPosition, this.duration).SetEase(Ease.OutSine));
                yield return sequence.WaitForCompletion(true);
            }
            yield break;
        }
    }

    public Coroutine Hide(bool forced = false)
    {
        return CorouWaiter.Start(GetRoutine());
        IEnumerator GetRoutine()
        {
            if (!forced)
            {
                this.background.color = this.background.color.SetAlpha(1f);
                this.spinnerContainer.anchoredPosition = this.spinnerPointOn.anchoredPosition;
                var sequence = DOTween.Sequence()
                    .Append(this.background.DOFade(0f, this.duration).SetEase(Ease.InOutSine))
                    .Join(this.spinnerContainer.DOAnchorPos(this.spinnerPointOff.anchoredPosition, this.duration).SetEase(Ease.InSine));
                yield return sequence.WaitForCompletion(true);
            }
            this.waitScreenGo.SetActive(false);
            yield break;
        }
    }
}