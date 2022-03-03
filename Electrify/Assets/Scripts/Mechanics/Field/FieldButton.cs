using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FieldButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    private FieldDisplay fieldDisplayer;
    public FieldType fieldType;
    [Header("Sprite")]
    public Sprite buttonUnpressed;
    public Sprite buttonPressed;
    [Header("Interaction")]
    public Color OnHoverColor;
    public Color OnPressColor;
    public Color DefaultColor;
    public Color InEffectColor;
    private RectTransform rectTransform;
    private Image image;
    private bool isPressed = false;
    private void Awake()
    {
        fieldDisplayer = FindObjectOfType<FieldDisplay>();
        rectTransform = GetComponent<RectTransform>();  
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
        if (image != null)
        {
            image.color = isPressed ? InEffectColor : DefaultColor;
            image.sprite = isPressed ? buttonPressed : buttonUnpressed;
        }
    }
    public void SetSprite()
    {
        if (image != null)
        {
            image.sprite = isPressed ? buttonPressed : buttonUnpressed;
            rectTransform.sizeDelta = new Vector2(image.sprite.texture.width, image.sprite.texture.height);
        }
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        isPressed = !isPressed;
        SetSprite();
        if (isPressed) fieldDisplayer.fieldType = fieldType;
        else if(fieldDisplayer.fieldType == fieldType) fieldDisplayer.fieldType = FieldType.None;
    }
}
