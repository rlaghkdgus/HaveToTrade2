using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class AnimalMove : MonoBehaviour
{
    public float moveSpeed = 3f;        // 이동 속도 (유닛/초)
    public float yPosition = 0f;        // 고정 y좌표 (수평 이동)
    public Vector3 baseScale = Vector3.one; // 인스펙터에서 조절 가능한 기본 스케일

    private float leftBound;
    private float rightBound;

    private void Start()
    {
        // 카메라 왼쪽/오른쪽 끝 좌표 계산
        Vector3 left = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, Mathf.Abs(Camera.main.transform.position.z)));
        Vector3 right = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, Mathf.Abs(Camera.main.transform.position.z)));

        leftBound = left.x;
        rightBound = right.x;

        // 시작할 때 기본 스케일 적용
        transform.localScale = baseScale;

        MoveToRandomPosition();
    }

    private void MoveToRandomPosition()
    {
        // 랜덤 X 좌표
        float randomX = Random.Range(leftBound, rightBound);
        Vector3 targetPos = new Vector3(randomX, yPosition, transform.position.z);

        // 방향 반전 (원본이 왼쪽 바라보는 이미지라면 그대로, 오른쪽이면 반전)
        if (targetPos.x > transform.position.x)
            transform.localScale = new Vector3(-baseScale.x, baseScale.y, baseScale.z); // 오른쪽
        else
            transform.localScale = new Vector3(baseScale.x, baseScale.y, baseScale.z);  // 왼쪽

        // 거리 계산
        float distance = Vector3.Distance(transform.position, targetPos);

        // 이동 시간 = 거리 ÷ 속도
        float duration = distance / moveSpeed;

        // DOTween 이동 시작
        transform.DOMove(targetPos, duration).SetEase(Ease.Linear).OnComplete(() =>
        {
            MoveToRandomPosition(); // 도착하면 다시 호출
        });
    }
}
