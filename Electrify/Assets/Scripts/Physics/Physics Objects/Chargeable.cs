using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
public enum ParticleType
{
    [Description("Proton - Positively Charged")]
    Proton = 1,
    [Description("Neutron - Neutrally Charged")]
    Neutron = 0,
    [Description("Electron - Negatively Charged")]
    Electron = -1
}

[RequireComponent(typeof(PhysicsObject))]
public class Chargeable : MonoBehaviour
{
    [HideInInspector] 
    public PhysicsObject physicsObject;

    public ParticleType particleType;
    public float magnitude = 1;
    [HideInInspector] 
    public float charge = 1;
    [Header("Color")]
    public Color protonColor = new Color(0.961f, 0.663f, 0.722f);
    public Color electronColor = new Color(0.357f, 0.808f, 0.980f);
    public Color neutronColor = new Color(0.369f, 0.953f, 0.553f);
    public Color unidentifiedColor = new Color(0.933f, 0.961f, 0.859f);

    private ObjectUI objectUI;
    private void Awake()
    {
        physicsObject = GetComponent<PhysicsObject>();
    }
    public void UpdateCharge()
    {
        switch (particleType)
        {
            case ParticleType.Proton: charge = magnitude;           break;
            case ParticleType.Neutron: charge = 0;                  break;
            case ParticleType.Electron: charge = -1 * magnitude;    break;
            default: Debug.Log("Something has gone wrong", this);   break;
        }
        UpdateColor();
        if(physicsObject != null) physicsObject.UI.UpdateChargeUI(particleType, magnitude);
    }
    public void UpdateColor()
    {
        Color color = unidentifiedColor;
        switch (particleType)
        {
            case ParticleType.Proton: color = protonColor;          break;
            case ParticleType.Neutron: color = neutronColor;        break;
            case ParticleType.Electron: color = electronColor;      break;
            default: Debug.Log("Something has gone wrong", this);   break;
        }
        this.GetComponent<Renderer>().sharedMaterial.color = color;
    }
    private void OnValidate()
    {
        if (particleType == ParticleType.Neutron) magnitude = 0;
        if (magnitude < 0) magnitude = 0;
        UpdateCharge();
    }
}
