using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle : MonoBehaviour
{
    public enum ParticleType
    {
        Proton = 1,
        Neutron = 0,
        Electron = -1
    }
    public ParticleType particleType;
    public float magnitude = 0;
    [HideInInspector] public float charge = 1;

    [Header("Sprite")]
    [SerializeField] private Sprite protonSprite;
    [SerializeField] private Sprite electronSprite;
    [SerializeField] private Sprite neutronSprite;
    [SerializeField] private Sprite unidentifiedSprite;
    [Header("Color")]
    public Color protonColor;
    public Color electronColor;
    public Color neutronColor;
    private Color white = new Color(1f, 1f, 1f, 1f);

    private void Start()
    {
        UpdateSurface();
    }
    public void UpdateSurface()
    {
        Color color = white;
        Sprite sprite = unidentifiedSprite;
        switch (particleType)
        {
            case ParticleType.Proton:
                charge = magnitude;
                color = protonColor;
                sprite = protonSprite;
                break;
            case ParticleType.Neutron:
                charge = 0;
                color = neutronColor;
                sprite = neutronSprite;
                break;
            case ParticleType.Electron:
                charge = -1 * magnitude;
                color = electronColor;
                sprite = electronSprite;
                break;
            default:
                Debug.Log("Something has gone wrong", this);
                break;
        }
        Transform center = gameObject.transform.Find("Center");
        foreach (Transform child in center)
            child.GetComponent<Renderer>().material.color = color;
        Transform sign = gameObject.transform.Find("Sign");
        foreach(Transform child in sign)
            child.GetComponent<SpriteRenderer>().sprite = sprite;
    }
}
