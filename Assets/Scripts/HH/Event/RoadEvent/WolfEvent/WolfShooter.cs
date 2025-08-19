using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class WolfShooter : MonoBehaviour
{
    public GameObject bulletPrefab; 
    public float bulletSpeed = 5f;
    GameObject spawnPoint;

    private LineRenderer lineRenderer;
    private Color normalColor = Color.white;
    private Color clickColor = Color.red;
    private bool isFlashing = false;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        spawnPoint = GameObject.FindWithTag("RoadEvent");
        lineRenderer.positionCount = 2;
        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        SetLineColor(normalColor);

        StartCoroutine(LineRoutine());
    }

    IEnumerator LineRoutine()
    {
        while (true)
        {
            Vector3 startPos = transform.position;
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPos.z = 0f;

            // 방향 벡터 계산 및 정규화
            Vector3 direction = (mouseWorldPos - startPos).normalized;

            // 고정 반지름 100만큼 방향으로 이동한 위치 계산
            Vector3 endPos = startPos + direction * 100f;

            lineRenderer.SetPosition(0, startPos);
            lineRenderer.SetPosition(1, endPos);

            if (Input.GetMouseButtonDown(0) && !isFlashing)
            {
                StartCoroutine(FlashLineColor());
                ShootBullet(direction);
            }

            yield return null;
        }
    }
    void ShootBullet(Vector3 direction)
    {
        if (bulletPrefab == null) return;

        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity,spawnPoint.transform);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            rb.velocity = direction * bulletSpeed;
        }
       
    }
    IEnumerator FlashLineColor()
    {
        isFlashing = true;
        SetLineColor(clickColor);

        yield return new WaitForSeconds(0.1f);

        SetLineColor(normalColor);
        isFlashing = false;
    }

    void SetLineColor(Color color)
    {
        lineRenderer.startColor = color;
        lineRenderer.endColor = color;
    }
}
