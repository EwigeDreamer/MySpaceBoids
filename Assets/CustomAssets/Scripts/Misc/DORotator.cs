using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DORotator : MonoBehaviour
{
#pragma warning disable 649
    [SerializeField] Vector3 from;
    [SerializeField] Vector3 to;
    [SerializeField] float duration;
#pragma warning restore 649
    private void OnEnable()
    {
        transform.localRotation = Quaternion.Euler(from);
        transform.DOLocalRotate(this.to, this.duration, RotateMode.FastBeyond360).SetEase(Ease.Linear).SetLoops(-1);
    }
    private void OnDisable()
    {
        transform.DOKill();
    }
}
