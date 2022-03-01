using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BarbedWire : MonoBehaviour
{
    private GraphicRaycaster graphicRaycaster;
    private void Awake()
    {
        graphicRaycaster = GetComponent<GraphicRaycaster>();   
    }
    public bool isHoverOnWire()
    {
        List<RaycastResult> hits = new List<RaycastResult>();
        PointerEventData pointer = new PointerEventData(null);
        pointer.position = Input.mousePosition;
        graphicRaycaster.Raycast(pointer, hits);
        foreach (RaycastResult hit in hits)
        {
            GameObject collidedGameObject = hit.gameObject;
            if (collidedGameObject.transform.parent == transform) return true;
        }
        return false;
    }
}
