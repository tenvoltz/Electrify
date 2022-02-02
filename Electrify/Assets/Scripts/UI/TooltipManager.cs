using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TooltipManager : MonoBehaviour
{
    private static TooltipManager current;
    public Tooltip tooltip;
    private void Awake()
    {
        current = this;
    }
    public static void Show(string headerText = "", string descriptionText = "")
    {
        current.tooltip.SetText(headerText, descriptionText);
        current.tooltip.gameObject.SetActive(true);
        current.tooltip.FadeIn();

    }
    public static void Hide()
    {
        current.tooltip.gameObject.SetActive(false);
    }
}
