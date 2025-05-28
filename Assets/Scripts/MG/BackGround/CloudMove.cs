using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudMove : MonoBehaviour
{
    public float moveDistance; // 이동거리
    private float duration; // 이동시간
    public float speed; // 이동속도
    public Vector3 resetPos; // 시작위치
    private bool isReset = false;

    public void InitCloud(float moveDistance, Vector3 resetPos)
    {
        this.moveDistance = moveDistance;
        this.resetPos = resetPos;
        CalDuration();
        MoveCloud();
    }

    private void CalDuration()
    {
        duration = moveDistance / speed;
    }

    private void MoveCloud()
    {
        Vector3 endPos = transform.position + Vector3.right * moveDistance;

        transform.DOMoveX(endPos.x, duration).SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                transform.position = resetPos;
                if (!isReset)
                {
                    moveDistance += 15f;
                    CalDuration();
                    isReset = true;
                }
                MoveCloud();
            });
    }

    private void OnDisable()
    {
        DOTween.Kill(transform);
    }
}
