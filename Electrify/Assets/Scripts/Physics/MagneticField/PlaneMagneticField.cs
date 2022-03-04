using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshCollider))]

public class PlaneMagneticField : MagneticField
{
    private MeshCollider c;
    public Vector3 worldDirection;
    public float strength = 1;
    public int numRows = 10;
    public int numCols = 10;
    [Header("Sprite")]
    [SerializeField] private Sprite pointingArrow;
    [SerializeField] private Sprite outArrow;
    [SerializeField] private Sprite inArrow;
    public Color arrowColor;

    private float unitLength;
    private float unitWidth;
    private List<Vector3> fieldPoints;
    private List<GameObject> arrowObjects;
    public override Vector3 GetField(Vector3 other)
    {
        Vector3 direction = GetPlaneNormal();
        float distance = (this.transform.position - other).magnitude;
        Ray ray = new Ray(other, -1 * direction);
        RaycastHit hit;
        if (c.Raycast(ray, out hit, distance))
            return strength * worldDirection;
        return Vector3.zero;
    }
    public override Vector3 GetExposedFieldFromGilbert(Vector3 other, List<GameObject> gilbertObjects)
    {
        Vector3 pointToOther = other - this.transform.position;
        if (IntersectGilbertCage(other, -GetPlaneNormal(), pointToOther.magnitude, gilbertObjects))
        {
            return Vector3.zero;
        }
        else return GetField(other);
    }
    private void Awake()
    {

        if (GetComponent<MeshCollider>() != null) c = GetComponent<MeshCollider>();
        else c = gameObject.AddComponent<MeshCollider>();
        unitLength = GetSize() / numRows;
        unitWidth = GetSize() / numCols;
        setFieldPoints();
        setArrowObjects();
        Render();
    }
    private void Render()
    {
        for (int p = 0; p < fieldPoints.Count; p++)
        {
            arrowObjects[p].GetComponent<SpriteRenderer>().color = arrowColor;
            if (Vector3.Angle(GetPlaneNormal(), worldDirection) == 0)
            {
                arrowObjects[p].GetComponent<SpriteRenderer>().sprite = outArrow;
                arrowObjects[p].transform.Rotate(90, 0, 0, Space.Self);
            }
            else if (Vector3.Angle(GetPlaneNormal(), worldDirection) == 180)
            {
                arrowObjects[p].GetComponent<SpriteRenderer>().sprite = inArrow;
                arrowObjects[p].transform.Rotate(90, 0, 0, Space.Self);
            }
            else
            {
                arrowObjects[p].GetComponent<SpriteRenderer>().sprite = pointingArrow;
                Quaternion toRotate = Quaternion.LookRotation(worldDirection, this.transform.up);
                arrowObjects[p].transform.rotation = toRotate;
                arrowObjects[p].transform.Rotate(90, 0, 0, Space.Self);
            }
        }
    }
    private void setFieldPoints()
    {
        fieldPoints = new List<Vector3>();
        Vector3 localPosition = Vector3.zero;
        for (int r = 0; r < numRows; r++)
        {
            for (int c = 0; c < numCols; c++)
            {
                float xPos = (localPosition[0] - GetSize() / 2 + unitWidth / 2) + c * unitWidth;
                float zPos = (localPosition[1] - GetSize() / 2 + unitLength / 2) + r * unitLength;
                Vector3 localPoint = new Vector3(xPos, 0.1f, zPos);
                fieldPoints.Add(transform.TransformPoint(localPoint));
            }
        }
    }
    private void setArrowObjects()
    {
        arrowObjects = new List<GameObject>();
        for (int i = 0; i < fieldPoints.Count; i++)
        {
            GameObject arrowObject = new GameObject("FieldArrow" + i, typeof(SpriteRenderer));
            arrowObject.transform.LookAt(arrowObject.transform.position - this.transform.up);
            arrowObject.transform.localScale = new Vector3(unitLength, 1, unitWidth);
            arrowObject.transform.SetParent(this.transform, false);
            arrowObject.transform.position = fieldPoints[i];
            arrowObjects.Add(arrowObject);
        }
    }

    public Vector3 GetPlaneNormal()
    {
        return this.transform.TransformDirection(Vector3.up);
    }

    private float GetSize() // A standard plane is 10 x 10 unit
    {
        return 10;
    }
}