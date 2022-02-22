using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using JetBrains.Annotations;

public class ObjectUI : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] private GameObject lockContainer;
    [SerializeField] private RectTransform pivot;
    [SerializeField] private RectTransform chargeContainer;
    [SerializeField] private TextMeshProUGUI chargeText;
    private RectTransform canvasRectTransform;
    private void Awake()
    {
        canvasRectTransform = GetComponent<RectTransform>();
    }
    private void Start()
    {
        GetComponent<Canvas>().worldCamera = Camera.main;
    }
    private void Update()
    {
        //chargeContainer.transform.LookAt(Camera.main.transform.position); //Rotate so it would always flat to camera, if perspective view
        chargeContainer.eulerAngles = new Vector3(0, 180, 0);
    }
    public void UpdateLocalScale(Vector3 localScale)
    {
        canvasRectTransform.localScale = new Vector3(1 / localScale.x, 1/localScale.y, 1/localScale.z);
    }
    public void UpdateSize(Vector3 size)
    {
        canvasRectTransform.sizeDelta = new Vector2(size.x, size.y);
    }

    public void UpdatePosition(Vector3 size)
    {
        Vector3 localPosition = canvasRectTransform.localPosition;
        localPosition.z = size[2] * 0.6f;
        canvasRectTransform.localPosition = localPosition;
    }

    public void UpdateChargeUI(ParticleType particleType, float magnitude)
    {
        string sign = "";
        switch (particleType)
        {
            case ParticleType.Proton: sign = "+";                   break;
            case ParticleType.Neutron: sign = "";                   break;
            case ParticleType.Electron: sign = "-";                 break;
            default: Debug.Log("Something has gone wrong", this);   break;
        }
        chargeText.text = sign + magnitude;
    }
    public void EnableChargeDisplay()
    {
        chargeContainer.gameObject.SetActive(true);
    }
    public void DisableChargeDisplay()
    {
        chargeContainer.gameObject.SetActive(false);
    }
    public void EnableLock()
    {
        lockContainer.SetActive(true);
    }
    public void DisableLock()
    {
        lockContainer.SetActive(false);
    }
    public void EnablePivot()
    {
        pivot.gameObject.SetActive(true);
    }
    public void DisablePivot()
    {
        pivot.gameObject.SetActive(false);
    }
    public void setPivot(Vector3 centerOfMass)
    {
        Vector2 offset = new Vector2(centerOfMass.x, centerOfMass.y);
        pivot.anchoredPosition = pivot.anchoredPosition + offset;
    }

}
