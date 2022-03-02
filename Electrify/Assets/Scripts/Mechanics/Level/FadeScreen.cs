using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeScreen : MonoBehaviour
{
    [SerializeField] private CanvasGroup screenCanvas;
    public float fadeDuration = 1f;
    public void FadeIn()
    {
        gameObject.SetActive(true);
        screenCanvas.alpha = 0;
        LeanTween.value(gameObject, 0, 1, fadeDuration).setIgnoreTimeScale(true).setOnUpdate((float val) =>
        {
            screenCanvas.alpha = val;
        });
    }
    
    public void FadeOut()
    {
        screenCanvas.alpha = 1;
        LeanTween.value(gameObject, 1, 0, fadeDuration).setIgnoreTimeScale(true).setOnUpdate((float val) =>
        {
            screenCanvas.alpha = val;
        }).setOnComplete(() => { gameObject.SetActive(false); });
    }
}
