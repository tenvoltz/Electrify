using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string header;
    [TextArea(5,10), Multiline()] 
    public string description;

    private static LTDescr delay;
    public void OnPointerExit(PointerEventData eventData)
    {
        LeanTween.cancel(delay.uniqueId);
        TooltipManager.Hide();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        delay = LeanTween.delayedCall(0.5f, () =>
        {
            TooltipManager.Show(header, description);
        });
    }
}
