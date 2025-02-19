using UnityEngine;

public class ScreenOriantation : MonoBehaviour
{
    void Awake()
    {
        Screen.orientation = ScreenOrientation.LandscapeRight;
    }
}
