using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldDisplayer : MonoBehaviour
{
    [SerializeField] private GameObject fieldManager;
    private List<MagneticField> mfs;
    private Texture2D mask;

    [SerializeField] private float xSpace = 1f;
    [SerializeField] private float zSpace = 1f;
    void Start()
    {
        mfs = new List<MagneticField>();
        foreach(Transform child in fieldManager.transform)
        {
            MagneticField[] childMF = child.GetComponentsInChildren<MagneticField>();
            if(childMF.Length > 0) mfs.Add(child.GetComponentsInChildren<MagneticField>()[0]);
        }
        GenerateTexture();
    }

    private void GenerateTexture()
    {
        MeshFilter filter = GetComponent<MeshFilter>();
        Bounds b = filter.mesh.bounds;
        int xAmount = (int)(b.size.x / xSpace);
        int zAmount = (int)(b.size.z / zSpace);
        mask = new Texture2D(xAmount, zAmount, TextureFormat.RGBA32, true);
        for (int z = 0; z < mask.height; z++){
            for (int x = 0; x < mask.width; x++){
                Vector3 location = transform.TransformPoint(new Vector3((x+0.5f) * xSpace, 0.0f, (z+0.5f) * zSpace) - b.extents);
                Color color = Color.clear;
                foreach(MagneticField mf in mfs)
                {
                    color += mf.GetColor(location);
                }
                color /= mfs.Count;
                mask.SetPixel(xAmount -x -1, z, color);
            }
        }
        mask.Apply();
        GetComponent<Renderer>().material.mainTexture = mask;
    }
}
