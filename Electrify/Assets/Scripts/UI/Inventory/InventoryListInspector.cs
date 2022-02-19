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
            SerializedProperty MyObjectType = MyListRef.FindPropertyRelative("itemType");
            SerializedProperty MyParticleType = MyListRef.FindPropertyRelative("particleType");
            SerializedProperty MyMagnitude = MyListRef.FindPropertyRelative("magnitude");
            SerializedProperty MyMovable = MyListRef.FindPropertyRelative("movable");
            SerializedProperty MyMass = MyListRef.FindPropertyRelative("mass");

            EditorGUILayout.PropertyField(MyObjectType);
            EditorGUILayout.PropertyField(MyMovable);
            EditorGUILayout.PropertyField(MyParticleType);
            EditorGUILayout.PropertyField(MyMagnitude);
            if(MyMovable.boolValue) EditorGUILayout.PropertyField(MyMass);

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
