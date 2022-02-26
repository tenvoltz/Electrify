using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalDetector : MonoBehaviour
{
    [SerializeField] private GameObject targetObject;
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == targetObject)
        {
            Debug.Log("The Goal is good", this);
            LevelManager.Instance.moveToLevelByGoal(LevelManager.Instance.getCurrentLevel() + 1);     
        }
    }
}
