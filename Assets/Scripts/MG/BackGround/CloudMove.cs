using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudMove : MonoBehaviour
{
    public float moveDistance; // �̵��Ÿ�
    public float duration; // �̵��ӵ�
    public Vector3 resetPos; // ������ġ

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
