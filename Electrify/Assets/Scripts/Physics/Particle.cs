using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle : MonoBehaviour
{
    public float charge = 1;

    private Color lightBlue = new Color(0.357f, 0.808f, 0.980f, 1f);
    private Color pink = new Color(0.961f, 0.663f, 0.722f, 1f);
    private Color white = new Color(1f, 1f, 1f, 1f);

    [Header("Sprite")]
    [SerializeField] private Sprite proton;
    [SerializeField] private Sprite electron;
    [SerializeField] private Sprite unidentified;

    private void Start()
    {
        UpdateSurface();
    }
    public void UpdateSurface()
    {
        Color color = charge > 0 ? lightBlue : pink;
        Sprite sprite = charge > 0 ? proton : electron;
        Transform center = gameObject.transform.Find("Center");
        foreach (Transform child in center)
            child.GetComponent<Renderer>().material.color = color;
        Transform sign = gameObject.transform.Find("Sign");
        foreach(Transform child in sign)
            child.GetComponent<SpriteRenderer>().sprite = sprite;
    }
}
