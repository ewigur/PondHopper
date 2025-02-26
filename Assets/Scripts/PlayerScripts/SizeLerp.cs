using UnityEngine;
//TODO: Find out why the lerp doesn't work, otherwise remove it!
public class SizeLerp : MonoBehaviour
{
    private Transform frogScale;
    
    private Vector2 currentSize;
    private Vector2 position;
    private float newScaleX;
    private float newScaleY;

    private readonly float furtherAwayScale = 0.4f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentSize = transform.localScale;
        position = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        LerpSize(position, currentSize);
    }

    private void LerpSize(Vector2 pos, Vector2 scale)
    {
        if (pos.y < 1.3f)
        {
            newScaleX = scale.x - furtherAwayScale;
            newScaleY = scale.y - furtherAwayScale;
            
            Vector2 newScale = new Vector2(newScaleX, newScaleY);
            transform.localScale = Vector2.Lerp(scale, newScale, 0.7f);
            
            Debug.Log("Lerp Size Down");
        }

        else if (pos.y < -1)
        {
            transform.localScale = scale;
            Debug.Log("Size == " + transform.localScale);
        }
    }
}
