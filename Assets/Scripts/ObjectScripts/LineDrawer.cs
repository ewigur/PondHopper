using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LineDrawer : MonoBehaviour
{
    private LineRenderer lineRenderer;
    
    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
    }

    public void DrawLine(Vector2 direction, float magnitude)
    {
        Vector2 start = transform.position;
        lineRenderer.SetPosition(0, new Vector2(start.x, start.y));
        
        Vector2 end = start + direction.normalized * magnitude;
        lineRenderer.SetPosition(1, new Vector2(end.x, end.y));

        lineRenderer.enabled = true;
        
        Debug.Log("Drawing Line");
    }

    public void ClearLine()
    {
        Debug.Log("Clearing Line");
        lineRenderer.enabled = false;
    }
}
