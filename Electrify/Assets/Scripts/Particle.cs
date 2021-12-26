using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle : MonoBehaviour
{
    public float charge = 1;

    private Color lightBlue = new Color(0.357f, 0.808f, 0.980f, 1f);
    private Color pink = new Color(0.961f, 0.663f, 0.722f, 1f);
    private Color white = new Color(1f, 1f, 1f, 1f);

    private void Start()
    {
        UpdateSurface();
    }
    public void UpdateSurface()
    {
        Color color = charge > 0 ? lightBlue : pink;
        GetComponent<Renderer>().material.color = color;
    }
}
