using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.ComponentModel;

public enum FieldType
{
    [Description("No Field")]
    None = 0,
    [Description("Electric Field")]
    Electric = 1,
    [Description("Magnetic Field")]
    Magnetic = 2
}
public class FieldDisplay : MonoBehaviour
{
    [SerializeField] PhysicsEMManager physicsEMManager;
    public FieldType fieldType;
    public int numRows = 1;
    public int numCols = 1;
    public float strengthCap = 1;
    private float unitLength;
    private float unitWidth;
    private List<Vector3> fieldPoints;
    private List<GameObject> arrowObjects;
    [Header("Sprite")]
    [SerializeField] private Sprite pointingArrow;
    [SerializeField] private Sprite outArrow;
    [SerializeField] private Sprite inArrow;
    private static Gradient gradient;
    void Start()
    {
        unitLength = GetLength() / numRows;
        unitWidth = GetWidth() / numCols;
        setFieldPoints();
        setArrowObjects();
    }
    private void Update()
    {
        //debugDisplayBounds();
        //debugDisplayPoints();
        //debugDisplayGrid();
        if (fieldType == FieldType.Electric) displayElectricFieldObjects(physicsEMManager.electricFieldList, physicsEMManager.faradayList);
        else if (fieldType == FieldType.Magnetic) displayMagneticFieldObjects(physicsEMManager.magneticFieldList);
        else displayNoFieldObjects();
    }

    private Color getColorFromFieldStrength(float magnitude)
    {
        float time = Mathf.InverseLerp(0, strengthCap, magnitude);
        Color arrowColor = GetGradient().Evaluate(time);
        return arrowColor;
    }
    private void displayNoFieldObjects()
    {
        for (int p = 0; p < fieldPoints.Count; p++)
        {
            arrowObjects[p].GetComponent<SpriteRenderer>().sprite = null;
        }
    }
    private void displayElectricFieldObjects(List<ElectricField> electricFieldList, List<GameObject> faradayList)
    {
        for (int p = 0; p < fieldPoints.Count; p++)
        {
            Vector3 combinedElectricField = Vector3.zero;
            foreach (ElectricField electricField in electricFieldList)
            {
                if (faradayList.Count == 0) combinedElectricField += electricField.GetField(fieldPoints[p]);
                else combinedElectricField += electricField.GetExposedFieldFromFaraday(fieldPoints[p], faradayList);
            }
            if (combinedElectricField.magnitude == 0)
            {
                arrowObjects[p].GetComponent<SpriteRenderer>().sprite = null;
                continue;
            }
            Vector3 electricFieldProjOntoPlaneNorm = Vector3.Dot(combinedElectricField, getPlaneNormal()) * getPlaneNormal();
            Vector3 electricFieldProjAlongPlane = combinedElectricField - electricFieldProjOntoPlaneNorm;
            arrowObjects[p].GetComponent<SpriteRenderer>().color = getColorFromFieldStrength(combinedElectricField.magnitude);
            if (electricFieldProjAlongPlane.magnitude >= electricFieldProjOntoPlaneNorm.magnitude)
            {
                arrowObjects[p].GetComponent<SpriteRenderer>().sprite = pointingArrow;
                Quaternion toRotate = Quaternion.LookRotation(electricFieldProjAlongPlane, this.transform.up);
                arrowObjects[p].transform.rotation = toRotate;
                arrowObjects[p].transform.Rotate(90, 0, 0, Space.Self);
            }
            else
            {
                if (Vector3.Angle(getPlaneNormal(), electricFieldProjOntoPlaneNorm) == 0)
                {
                    arrowObjects[p].GetComponent<SpriteRenderer>().sprite = outArrow;
                }
                else
                {
                    arrowObjects[p].GetComponent<SpriteRenderer>().sprite = inArrow;
                }
            }
        }
    }
    private void displayMagneticFieldObjects(List<MagneticField> magneticFieldList)
    {
        for (int p = 0; p < fieldPoints.Count; p++)
        {
            Vector3 combinedMagneticField = Vector3.zero;
            foreach (MagneticField magneticField in magneticFieldList)
            {
                combinedMagneticField += magneticField.GetField(fieldPoints[p]);
            }
            if (combinedMagneticField.magnitude == 0)
            {
                arrowObjects[p].GetComponent<SpriteRenderer>().sprite = null;
                continue;
            }
            Vector3 magneticFieldProjOntoPlaneNorm = Vector3.Dot(combinedMagneticField, getPlaneNormal()) * getPlaneNormal();
            Vector3 magneticFieldProjAlongPlane = combinedMagneticField - magneticFieldProjOntoPlaneNorm;
            arrowObjects[p].GetComponent<SpriteRenderer>().color = getColorFromFieldStrength(combinedMagneticField.magnitude);
            if (magneticFieldProjAlongPlane.magnitude >= magneticFieldProjOntoPlaneNorm.magnitude)
            {
                arrowObjects[p].GetComponent<SpriteRenderer>().sprite = pointingArrow;
                Quaternion toRotate = Quaternion.LookRotation(magneticFieldProjAlongPlane, this.transform.up);
                arrowObjects[p].transform.rotation = toRotate;
                arrowObjects[p].transform.Rotate(90, 0, 0, Space.Self);
            }
            else
            {
                if (Vector3.Angle(getPlaneNormal(), magneticFieldProjOntoPlaneNorm) == 0)
                {
                    arrowObjects[p].GetComponent<SpriteRenderer>().sprite = outArrow;
                }
                else
                {
                    arrowObjects[p].GetComponent<SpriteRenderer>().sprite = inArrow;
                }
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
                float xPos = (localPosition[0] - GetWidth() / 2 + unitWidth / 2) + c * unitWidth;
                float zPos = (localPosition[1] - GetLength() / 2 + unitLength / 2) + r * unitLength;
                Vector3 localPoint = new Vector3(xPos, 0.01f, zPos);
                fieldPoints.Add(transform.TransformPoint(localPoint));
            }
        }
    }
    private void setArrowObjects()
    {
        arrowObjects = new List<GameObject>();
        for(int i = 0; i < fieldPoints.Count; i++)
        {
            GameObject arrowObject = new GameObject("FieldArrow" + i, typeof(SpriteRenderer));
            arrowObject.transform.SetParent(this.transform);
            arrowObject.transform.position = fieldPoints[i];
            arrowObject.transform.LookAt(arrowObject.transform.position - this.transform.up);
            arrowObjects.Add(arrowObject);
        }
    }
    private float GetLength() // A standard plane is 10 x 10 unit
    {
        return transform.localScale[0] * 10;
    }
    private float GetWidth()
    {
        return transform.localScale[2] * 10; 
    }
    private Vector3 getPlaneNormal()
    {
        return this.transform.up;
    }
    private static Gradient GetGradient()
    {
        if (gradient == null)
        {
            Gradient rainbowGradient = new Gradient();
            Color orange = new Color(255, 141, 0, 1);
            //Color indigo = new Color(75, 0, 130);
            //Color violet = new Color(118, 1, 136);
            //Color[] rainbow = { Color.red, orange, Color.yellow, Color.green, Color.blue, indigo, violet };
            Color[] rainbow = { Color.blue, Color.green, Color.yellow, orange, Color.red };
            GradientColorKey[] colorKey = new GradientColorKey[rainbow.Length];
            for (int c = 0; c < rainbow.Length; c++)
            {
                colorKey[c].color = rainbow[c];
                colorKey[c].time = c / (rainbow.Length - 1.0f);
            }
            GradientAlphaKey[] alphaKey = new GradientAlphaKey[2];
            alphaKey[0].alpha = 1.0f;
            alphaKey[0].time = 0.0f;
            alphaKey[1].alpha = 1.0f;
            alphaKey[1].time = 1.0f;
            rainbowGradient.SetKeys(colorKey, alphaKey);
            gradient = rainbowGradient;
        }
        return gradient;
    }
    private void debugDisplayPoints()
    {
        foreach (Vector3 point in fieldPoints)
        {
            Debug.DrawRay(point, transform.TransformDirection(Vector3.up), Color.red);
        }
    }
    private void debugDisplayBounds()
    {
        Vector3 topLeft = transform.TransformPoint(new Vector3(-GetWidth() / 2, 0, GetLength() / 2));
        Vector3 topRight = transform.TransformPoint(new Vector3(GetWidth() / 2, 0, GetLength() / 2));
        Vector3 botLeft = transform.TransformPoint(new Vector3(-GetWidth() / 2, 0, -GetLength() / 2));
        Vector3 botRight = transform.TransformPoint(new Vector3(GetWidth() / 2, 0, -GetLength() / 2));
        Debug.DrawRay(topLeft, GetWidth() * transform.TransformDirection(Vector3.right), Color.red);
        Debug.DrawRay(topRight, GetLength() * transform.TransformDirection(Vector3.back), Color.red);
        Debug.DrawRay(botRight, GetWidth() * transform.TransformDirection(Vector3.left), Color.red);
        Debug.DrawRay(botLeft, GetLength() * transform.TransformDirection(Vector3.forward), Color.red);
    }
    private void debugDisplayGrid()
    {
        for (int r = 0; r <= numRows; r++)
        {
            float zPos = (-GetLength() / 2) + r * unitLength;
            Vector3 rowPoint = transform.TransformPoint(new Vector3(-GetWidth() / 2, 0, zPos));
            Debug.DrawRay(rowPoint, GetWidth() * transform.TransformDirection(Vector3.right), Color.green);
        }
        for (int c = 0; c <= numCols; c++)
        {
            float xPos = (-GetWidth() / 2) + c * unitWidth;
            Vector3 colPoint = transform.TransformPoint(new Vector3(xPos, 0, -GetLength() / 2));
            Debug.DrawRay(colPoint, GetLength() * transform.TransformDirection(Vector3.forward), Color.green);
        }
    }
}