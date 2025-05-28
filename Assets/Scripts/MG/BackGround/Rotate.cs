using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Rotate : MonoBehaviour
{
    public float RotateTime;

    private void Start()
    {
        transform.DORotate(new Vector3(0, 0, 360), RotateTime, RotateMode.FastBeyond360)
            .SetEase(Ease.Linear)
            .SetLoops(-1);
    }

    private void OnDisable()
    {
        DOTween.Kill(transform);
    }
}
