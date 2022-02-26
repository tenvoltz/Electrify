using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;

public class ReturnMenuButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    private RectTransform rectTransform;
    private Image image;
    [Header("Interaction")]
    public Color OnHoverColor;
    public Color OnPressColor;
    public Color DefaultColor;
    private LTDescr swayAnimation;
    private Vector3 initialPosition;
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        image = GetComponent<Image>();
        image.color = DefaultColor;
        initialPosition = rectTransform.localPosition;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (image != null) image.color = OnPressColor;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        Vector3 finalPosition = initialPosition + Vector3.left * rectTransform.rect.width / 4;
        swayAnimation = LeanTween.moveLocal(gameObject, finalPosition, 1).setIgnoreTimeScale(true).setLoopPingPong().setEaseInOutCubic();
        if (image != null) image.color = OnHoverColor;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        LeanTween.cancel(swayAnimation.uniqueId);
        rectTransform.localPosition = initialPosition;
        image.color = DefaultColor;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        LevelManager.Instance.returnToMainMenu(LevelManager.Instance.getCurrentLevel());
    }
}
