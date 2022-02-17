using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    [SerializeField] 
    private RectTransform inventoryRectTransform;
    private Image image;
    private RectTransform buttonRectTranform;
    private bool isPressed = false;
    [Header("Interaction")]
    public Color OnHoverColor;
    public Color OnPressColor;
    public Color DefaultColor;
    public Color InEffectColor;
    private static LTDescr inventoryAnimation;
    private void Awake()
    {
        buttonRectTranform = GetComponent<RectTransform>();
        image = GetComponent<Image>();
        SetDefaultColor();
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (image != null) image.color = OnPressColor;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (image != null) image.color = OnHoverColor;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        SetDefaultColor();
    }
    public void SetDefaultColor()
    {
        if (image != null) image.color = isPressed ? InEffectColor : DefaultColor;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        isPressed = !isPressed;
        if (isPressed)
        {
            moveInventoryUp();
        }
        else
        {
            moveInventoryDown();
        }
    }

    private void moveInventoryDown()
    {
        float finalYPosition = inventoryRectTransform.anchoredPosition.y - buttonRectTranform.rect.height - inventoryRectTransform.rect.height;
        inventoryAnimation = LeanTween.moveY(inventoryRectTransform, finalYPosition, 0.5f).setEaseOutQuart().setIgnoreTimeScale(true);

    }
    private void moveInventoryUp()
    {
        float finalYPosition = inventoryRectTransform.anchoredPosition.y + buttonRectTranform.rect.height + inventoryRectTransform.rect.height;
        inventoryAnimation = LeanTween.moveY(inventoryRectTransform, finalYPosition, 0.5f).setEaseOutQuart().setIgnoreTimeScale(true);
    }
}
