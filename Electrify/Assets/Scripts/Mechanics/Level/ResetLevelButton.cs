using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;

public class ResetLevelButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    private RectTransform rectTransform;
    private Image image;
    [Header("Interaction")]
    public Color OnHoverColor;
    public Color OnPressColor;
    public Color DefaultColor;
    private LTDescr rotationAnimation;
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        image = GetComponent<Image>();
        image.color = DefaultColor;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (image != null) image.color = OnPressColor;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        rotationAnimation = LeanTween.rotateAroundLocal(rectTransform, Vector3.forward, 360, 5).setIgnoreTimeScale(true).setLoopClamp(-1);
        if (image != null) image.color = OnHoverColor;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        LeanTween.cancel(rotationAnimation.uniqueId);
        rectTransform.localRotation = Quaternion.identity;
        image.color = DefaultColor;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        LevelManager.Instance.resetLevel(LevelManager.Instance.getCurrentLevel());
    }
}
