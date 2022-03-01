using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField] private CanvasGroup loadingScreenCanvas;
    public float fadeDuration = 1f;
    public void FadeIn()
    {
        gameObject.SetActive(true);
        LeanTween.value(gameObject, 0, 1, fadeDuration).setIgnoreTimeScale(true).setOnUpdate((float val) =>
        {
            loadingScreenCanvas.alpha = val;
        });
    }
    
    public void FadeOut()
    {
        LeanTween.value(gameObject, 1, 0, fadeDuration).setIgnoreTimeScale(true).setOnUpdate((float val) =>
        {
            loadingScreenCanvas.alpha = val;
        }).setOnComplete(() => { gameObject.SetActive(false); });
    }
}
