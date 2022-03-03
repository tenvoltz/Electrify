using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using JetBrains.Annotations;

public class ObjectUI : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] private GameObject lockContainer;
    [SerializeField] private RectTransform pivot;
    [SerializeField] private RectTransform chargeContainer;
    [SerializeField] private TextMeshProUGUI chargeText;
    [SerializeField] private RectTransform goal;
    private RectTransform canvasRectTransform;
    [Header("Outline Color")]
    public Color conductorColor = new Color(0.714f, 0.725f, 0.749f);
    public Color insulatorColor = new Color(1.000f, 0.588f, 0.051f);
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
        chargeText.text = sign + magnitude.ToString("F1");
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
        pivot.anchoredPosition = offset;
    }
    public void EnableConductorOutline()
    {
        GetComponentInParent<Renderer>().material.SetColor("_OutlineColor", conductorColor);
    }
    public void EnableInsulatorOutline()
    {
        GetComponentInParent<Renderer>().material.SetColor("_OutlineColor", insulatorColor);
    }
    public void DisableGoal()
    {
        goal.gameObject.SetActive(false);
    }
    public void EnableGoal()
    {
        goal.gameObject.SetActive(true);
    }
    public void UpdateGoalColor(GoalDetector goalDetector)
    {
        goal.GetComponent<Image>().color = goalDetector.color;
    }

}
