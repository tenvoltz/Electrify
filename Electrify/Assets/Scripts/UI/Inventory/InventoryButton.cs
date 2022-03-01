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
    [Header("Sprite")]
    public Sprite inventoryClose;
    public Sprite inventoryOpen;
    [Header("Interaction")]
    public Color OnHoverColor;
    public Color OnPressColor;
    public Color DefaultColor;
    public Color InEffectColor;
    private LTDescr inventoryAnimation;
    private Vector2 initialRectPosition;
    private void Awake()
    {
        buttonRectTranform = GetComponent<RectTransform>();
        image = GetComponent<Image>();
        initialRectPosition = inventoryRectTransform.anchoredPosition;
        isPressed = true;
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
        if (image != null)
        {
            image.color = isPressed ? InEffectColor : DefaultColor;
            image.sprite = isPressed ? inventoryOpen : inventoryClose;
        }

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

    public void moveInventoryDown()
    {
        float finalYPosition = initialRectPosition.y - buttonRectTranform.rect.height - inventoryRectTransform.rect.height;
        inventoryAnimation = LeanTween.moveY(inventoryRectTransform, finalYPosition, 0.5f).setEaseOutQuart().setIgnoreTimeScale(true);
    }
    public void moveInventoryUp()
    {
        inventoryAnimation = LeanTween.moveY(inventoryRectTransform, initialRectPosition.y, 0.5f).setEaseOutQuart().setIgnoreTimeScale(true);
    }
}
