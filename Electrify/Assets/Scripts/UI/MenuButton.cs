using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public enum MenuButtionType
    {
        Enable, 
        Disable,
        Quit
    }

    public MenuButtionType menuButtionType;
    [Header("Reference")]
    [SerializeField] private GameObject selector;
    [SerializeField] private MainMenuManager mainMenu;
    [SerializeField] private GameObject myContainer;

    private Image selectorImage;
    private RectTransform selectorRectTransform;
    private LayoutElement layoutElement;
    private RectTransform rectTransform;

    private static LTDescr textBoxAnimation;
    private static LTDescr movingSelector;
    private void Awake()
    {
        selectorImage = selector.GetComponent<Image>();
        selectorRectTransform = selector.GetComponent<RectTransform>();

        layoutElement = GetComponent<LayoutElement>();
        rectTransform = GetComponent<RectTransform>();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (menuButtionType == MenuButtionType.Enable) mainMenu.enableUI(myContainer);
        else if (menuButtionType == MenuButtionType.Disable) mainMenu.disableUI(myContainer);
        else if (menuButtionType == MenuButtionType.Quit) Application.Quit();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        textBoxAnimation = LeanTween.value(gameObject, layoutElement.flexibleHeight, 1.5f, 0.5f).setEase(LeanTweenType.easeOutBack)
            .setOnUpdate((float val) =>
            {
                layoutElement.flexibleHeight = val;
            });
        selectorImage.enabled = true;
        movingSelector = LeanTween.moveX(selectorRectTransform, 300f, 0.5f).setEaseInOutSine().setLoopPingPong();
 
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        //if (textBoxAnimation != null) LeanTween.cancel(textBoxAnimation.uniqueId);
        textBoxAnimation = LeanTween.value(gameObject, layoutElement.flexibleHeight, 1, 0.5f)
            .setOnUpdate((float val) =>
            {
                layoutElement.flexibleHeight = val;
            });
        if (movingSelector != null) LeanTween.cancel(movingSelector.uniqueId);
        selectorImage.enabled = false;
    }
}
