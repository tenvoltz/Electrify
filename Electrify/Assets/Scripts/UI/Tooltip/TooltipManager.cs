using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TooltipManager : MonoBehaviour
{
    private static TooltipManager current;
    private Canvas canvas;
    public Tooltip tooltip;
    private void Awake()
    {
        canvas = GetComponent<Canvas>();
        current = this;
    }
    public static void Show(string headerText = "", string descriptionText = "")
    {
        current.tooltip.gameObject.SetActive(true);
        current.tooltip.SetText(headerText, descriptionText);
    }
    public static void Hide()
    {
        current.tooltip.gameObject.SetActive(false);
    }
}
