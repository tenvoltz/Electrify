using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

[CustomEditor(typeof(InventoryList))]

//Thanks ForceX for the code
public class InventoryListInspector : Editor
{

    InventoryList inventoryLists;
    SerializedObject GetTarget;
    SerializedProperty ThisList;
    int ListSize;

    void OnEnable()
    {
        inventoryLists = (InventoryList)target;
        GetTarget = new SerializedObject(inventoryLists);
        ThisList = GetTarget.FindProperty("inventoryList"); // Find the List in our script and create a refrence of it
    }

    public override void OnInspectorGUI()
    {
        //Update our list
        GetTarget.Update();
        //Or add a new item to the List<> with a button

        if (GUILayout.Button("Add New"))
        {
            inventoryLists.inventoryList.Add(new InventoryListObject());
        }

        EditorGUILayout.Space();
        EditorGUILayout.Space();

        //Display our list to the inspector window

        for (int i = 0; i < ThisList.arraySize; i++)
        {
            SerializedProperty MyListRef = ThisList.GetArrayElementAtIndex(i);
            SerializedProperty MyItemType = MyListRef.FindPropertyRelative("itemType");
            SerializedProperty MySize = MyListRef.FindPropertyRelative("size");
            SerializedProperty MyChargeableObject = MyListRef.FindPropertyRelative("chargeableObject");
            SerializedProperty MyParticleType = MyListRef.FindPropertyRelative("particleType");
            SerializedProperty MyMagnitude = MyListRef.FindPropertyRelative("magnitude");
            SerializedProperty MyContactType = MyListRef.FindPropertyRelative("contactType");
            SerializedProperty MyMovableObject = MyListRef.FindPropertyRelative("movableObject");
            SerializedProperty MyMass = MyListRef.FindPropertyRelative("mass");
            SerializedProperty MyPivotableObject = MyListRef.FindPropertyRelative("pivotableObject");
            SerializedProperty MyPivotFromCenterAt = MyListRef.FindPropertyRelative("pivotFromCenterAt");
            SerializedProperty MyGoalableObject = MyListRef.FindPropertyRelative("goalableObject");
            SerializedProperty MyGoal = MyListRef.FindPropertyRelative("goal");

            EditorGUILayout.PropertyField(MyItemType);
            EditorGUILayout.PropertyField(MySize);
            EditorGUILayout.PropertyField(MyChargeableObject);
            if (MyChargeableObject.boolValue)
            {
                EditorGUILayout.PropertyField(MyParticleType);
                EditorGUILayout.PropertyField(MyMagnitude);
                EditorGUILayout.PropertyField(MyContactType);
            }
            EditorGUILayout.PropertyField(MyMovableObject);
            if (MyMovableObject.boolValue || MyPivotableObject.boolValue)
            {
                EditorGUILayout.PropertyField(MyMass);
            }
            EditorGUILayout.PropertyField(MyPivotableObject);
            if (MyPivotableObject.boolValue)
            {
                MyMovableObject.boolValue = true;
                EditorGUILayout.PropertyField(MyPivotFromCenterAt);
            }
            EditorGUILayout.PropertyField(MyGoalableObject);
            if (MyGoalableObject.boolValue)
            {
                EditorGUILayout.PropertyField(MyGoal);
            }
            EditorGUILayout.Space();

            //Remove this index from the List
            if (GUILayout.Button("Remove This Index (" + i.ToString() + ")"))
            {
                ThisList.DeleteArrayElementAtIndex(i);
            }
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
        }

        //Apply the changes to our list
        GetTarget.ApplyModifiedProperties();
    }
}
