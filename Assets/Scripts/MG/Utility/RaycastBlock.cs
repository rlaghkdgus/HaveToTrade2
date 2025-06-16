using UnityEngine;
using UnityEngine.UI;

public class RaycastBlock : Graphic
{
    [Range(0f, 1f)] public float xPos;
    [Range(0f, 1f)] public float yPos;
    [Range(0f, 1f)] public float width;
    [Range(0f, 1f)] public float height;

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();
    }

    public override bool Raycast(Vector2 sp, Camera eventCamera)
    {
        RectTransform rt = rectTransform;
        Vector2 local;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rt, sp, eventCamera, out local);
        Vector2 normalized = Rect.PointToNormalized(rt.rect, local);

        if(normalized.x >= xPos && normalized.x <= xPos + width && normalized.y >= yPos && normalized.y <= yPos + height)
        {
            return false;
        }

        return true;
    }
}
