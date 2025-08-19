using UnityEngine;
using UnityEngine.UI;

public class RaycastBlock : MonoBehaviour, ICanvasRaycastFilter
{
    public RectTransform hole;

    private Camera eventCamera;

    public bool IsRaycastLocationValid(Vector2 sp, Camera eventCamera)
    {
        this.eventCamera = eventCamera;

        if(hole == null)
        {
            return true;
        }

        bool isInsideHole = RectTransformUtility.RectangleContainsScreenPoint(hole, sp, this.eventCamera);

        return !isInsideHole;
    }
}
