using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class uiLine : MonoBehaviour
{
    public List<RectTransform> targets;

    public RectTransform linePrefab;

    public float lineThickness = 4f;

    private List<RectTransform> lines = new List<RectTransform>();

    private void OnEnable()
    {
        CreateLines();
    }

    private void OnDisable()
    {
        DestroyLines();
    }

    private void Update()
    {
        UpdateLines();
    }

    private void CreateLines()
    {
        DestroyLines();

        for (int i = 0; i < targets.Count - 1; i++)
        {
            if (targets[i] == null || targets[i + 1] == null)
                continue;

            RectTransform newLine = Instantiate(linePrefab, transform);
            newLine.name = $"Line_{i}";
            newLine.SetAsFirstSibling();
            lines.Add(newLine);
        }
    }

    private void DestroyLines()
    {
        foreach (var line in lines)
        {
            if (line != null)
                Destroy(line.gameObject);
        }
        lines.Clear();
    }

    private void UpdateLines()
    {
        for (int i = 0; i < lines.Count; ++i)
        {
            if (targets[i] == null || targets[i + 1] == null || lines[i] == null) continue;

            Vector3 from = targets[i].position;
            Vector3 to = targets[i + 1].position;

            Vector3 center = (from + to) / 2f;
            lines[i].position = center;

            float distance = Vector3.Distance(from, to);
            lines[i].sizeDelta = new Vector2(distance, lineThickness);

            Vector3 dir = to - from;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            lines[i].rotation = Quaternion.Euler(0, 0, angle);
        }
    }
}
