using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] private GameObject mainContainer;

    public void enableUI(GameObject UIContainer)
    {
        UIContainer.SetActive(true);
        LeanTween.scale(mainContainer.GetComponent<RectTransform>(), new Vector3(0.5f, 0.5f, 0.5f), 0.5f).setEaseOutCubic().
            setOnStart(() => { 
                LeanTween.alphaCanvas(UIContainer.GetComponent<CanvasGroup>(), 1f, 0.5f).setEaseInOutQuad();
                
            });
    }
    public void disableUI(GameObject UIContainer)
    {
        LeanTween.scale(mainContainer.GetComponent<RectTransform>(), new Vector3(1f, 1f, 1f), 0.75f).setEaseOutCubic().
            setOnStart(() => {
                LeanTween.alphaCanvas(UIContainer.GetComponent<CanvasGroup>(), 0f, 0.5f).setEaseInOutQuad();
            }).setOnComplete(() => { UIContainer.SetActive(false); });
    }
}
