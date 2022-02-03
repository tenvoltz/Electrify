using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

[ExecuteInEditMode()]
public class Tooltip : MonoBehaviour
{
    public TextMeshProUGUI header;
    public TextMeshProUGUI description;

    public LayoutElement layout;

    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }
    private void OnEnable()
    {
        UpdatePosition();
        FadeIn();
    }
    private void Update()
    {
        UpdatePosition();
    }
    public void FadeIn()
    {
        LeanTween.value(gameObject, 0, 1, 1).setOnUpdate((float val) =>
        {
            canvasGroup.alpha = val;
        });
    }

    public void SetText(string headerText = "", string descriptionText = "")
    {
        if (string.IsNullOrEmpty(headerText))
        {
            header.gameObject.SetActive(false);
        }
        else
        {
            header.gameObject.SetActive(true);
            header.text = headerText;
        }
        if (string.IsNullOrEmpty(descriptionText))
        {
            description.gameObject.SetActive(false);
        }
        else
        {
            description.gameObject.SetActive(true);
            description.text = descriptionText;
        }

        int headerLength = header.text.Length;
        int descriptionLength = description.text.Length;
        layout.enabled = header.preferredWidth > layout.preferredWidth || description.preferredWidth > layout.preferredWidth;
    }

    public void UpdatePosition()
    {
        Vector2 mousePosition = Input.mousePosition;

        float pivotX = mousePosition.x / Screen.width;
        float pivotY = mousePosition.y / Screen.height;

        TooltipPosition tooltipPosition = TooltipPosition.Center;
        if (pivotX < .8 && pivotY > .5) tooltipPosition = TooltipPosition.TopLeftCorner;
        else if (pivotX > .7 && pivotY > .5) tooltipPosition = TooltipPosition.TopRightCorner;
        else if (pivotX > .7 && pivotY < .7) tooltipPosition = TooltipPosition.BottomRightCorner;
        else if (pivotX < .9 && pivotY < .7) tooltipPosition = TooltipPosition.BottomLeftCorner;

        rectTransform.pivot = getPosition(tooltipPosition, pivotX, pivotY);
        transform.position = mousePosition;
    }

    private enum TooltipPosition{
        TopLeftCorner,
        TopRightCorner,
        BottomLeftCorner,
        BottomRightCorner,
        Center
    }
    private static Vector2 getPosition(TooltipPosition tooltipPosition, float pivotX, float pivotY)
    {
        switch (tooltipPosition)
        {
            case TooltipPosition.TopLeftCorner:
                return new Vector2(-0.1f, 1.1f);
            case TooltipPosition.BottomLeftCorner:
                return new Vector2(-0.1f, -0.1f);
            case TooltipPosition.BottomRightCorner:
                return new Vector2(1.1f, -0.1f);
            case TooltipPosition.TopRightCorner:
                return new Vector2(1.1f, 1.1f);
            default:
                return new Vector2(pivotX, pivotY);
        }
    }

}
