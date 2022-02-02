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
        rectTransform.pivot = new Vector2(pivotX, pivotY);

        transform.position = mousePosition;
    }

}
