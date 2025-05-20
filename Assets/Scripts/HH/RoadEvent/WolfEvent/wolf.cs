using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wolf : MonoBehaviour
{
    public float moveSpeed = 0.1f;
    public float stopDistance = 0.1f;

    void Start()
    {
        StartCoroutine(MoveToCenter());
    }

    IEnumerator MoveToCenter()
    {
        Vector3 target = Vector3.zero;

        while (Vector3.Distance(transform.position, target) > stopDistance)
        {
            Vector3 direction = (target - transform.position).normalized;
            transform.position += direction * moveSpeed * Time.deltaTime;

            yield return null; // ���� �����ӱ��� ���
        }

        
        // �̵� ���� �� �ൿ �߰� ����
    }
    private void OnDestroy()
    {
        WolfEvent.wolfCount--;
    }
}

