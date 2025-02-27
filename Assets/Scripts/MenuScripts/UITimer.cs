using System.Collections;
using UnityEngine;

public class UITimer : MonoBehaviour
{
    private readonly float fadeInDuration = 1f;
    private readonly float delay = 4f;
    
    [SerializeField] private CanvasGroup[] elementsToFadeIn;

    private void Start()
    {
        foreach (var element in elementsToFadeIn)
        {
            element.alpha = 0f;
            element.gameObject.SetActive(true);
        }

        StartCoroutine(FadeInUI());
    }

    private IEnumerator FadeInUI()
    {
        yield return new WaitForSeconds(delay);
        
        float elapsedTime = 0f;

        while (elapsedTime < fadeInDuration)
        {
            float alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeInDuration);

            foreach (var element in elementsToFadeIn)
            {
                element.alpha = alpha;
            }
            
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        foreach (var element in elementsToFadeIn)
        {
            element.alpha = 1f;
        }
    }
}
