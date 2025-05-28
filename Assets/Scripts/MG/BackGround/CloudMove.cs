using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudMove : MonoBehaviour
{
    public float moveDistance; // 이동거리
    public float duration; // 이동속도
    public Vector3 resetPos; // 시작위치

    public void InitCloud(float moveDistance, float duration, Vector3 resetPos)
    {
        this.moveDistance = moveDistance;
        this.duration = duration;
        this.resetPos = resetPos;
        MoveCloud();
    }

    private void MoveCloud()
    {
        Vector3 endPos = transform.position + Vector3.right * moveDistance;

        transform.DOMoveX(endPos.x, duration).SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                transform.position = resetPos;
                MoveCloud();
            });
    }
}
